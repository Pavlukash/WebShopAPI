using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Contexts;
using WebShop.Domain.Models;
using WebShop.Services.Interfaces;
using WebShop.Services.Mappers;
using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Entities;
using WebShop.Services.Extentions;

namespace WebShop.Services.Services
{
    public class ClientService : IClientService
    {
        private WebShopApiContext WebShopApiContext { get; }
        
        private ICurrentUserService CurrentUserService { get; }

        public ClientService(WebShopApiContext context, ICurrentUserService currentUserService)
        {
            WebShopApiContext = context;
            CurrentUserService = currentUserService;
        }

        public async Task<IEnumerable<ClientDto>> GetClients(CancellationToken cancellationToken)
        {
            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }
            
            var clients = await WebShopApiContext.Clients
                .AsNoTracking()
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken); 
            
            if (clients == null)
            {
                throw new NullReferenceException();
            }
            
            return clients;
        }

        public async Task<ClientDto> GetById(int id, CancellationToken cancellationToken)
        {
            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }
            
            var client = await WebShopApiContext.Clients
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);
            
            var result = client.ToDto();

            return result;
        }

        public async Task<bool> GiveDiscount(int clientId, int discountId, CancellationToken cancellationToken)
        {
            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }
            
            var discount = await WebShopApiContext.Discounts
                .Where(x => x.Id == discountId)
                .FirstOrNotFoundAsync(cancellationToken);

            var client = await WebShopApiContext.Clients
                .Where(x => x.Id == clientId)
                .FirstOrNotFoundAsync(cancellationToken);
            
            var newEntity = new ClientsDiscountsEntity
            {
                ClientId = client.Id,
                DiscountId = discount.Id,
            };

            WebShopApiContext.ClientsDiscounts.Add(newEntity);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> Update(int id, ClientDto clientEntity, CancellationToken cancellationToken)
        {
            var clientToUpdate = await WebShopApiContext.Clients
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);

            clientToUpdate.FirstName = clientEntity.FirstName ?? clientToUpdate.FirstName;
            clientToUpdate.LastName = clientEntity.LastName ?? clientToUpdate.LastName;
            clientToUpdate.Email = clientEntity.Email ?? clientToUpdate.Email;
            clientToUpdate.PhoneNumber = clientEntity.PhoneNumber ?? clientToUpdate.PhoneNumber;

            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            var clientToDelete = await WebShopApiContext.Clients
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);
            
            WebShopApiContext.Clients.Remove(clientToDelete);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        
        public async Task<bool> BanUser(int id, CancellationToken cancellationToken)
        {
            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }
            
            var userToBan = await WebShopApiContext.Clients
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);

            if (userToBan.IsBaned)
            {
                throw new Exception("The user has already been banned");
            }

            userToBan.IsBaned = true;
            
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        
        public async Task<bool> UnbanUser(int id, CancellationToken cancellationToken)
        {
            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }
            
            var userToUnban = await WebShopApiContext.Clients
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);
            
            if (userToUnban.IsBaned == false)
            {
                throw new Exception("The user is not banned");
            }

            userToUnban.IsBaned = false;
            
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
