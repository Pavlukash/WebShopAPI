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

namespace WebShop.Services.Services
{
    public class ClientService : IClientService
    {
        private WebShopApiContext WebShopApiContext { get; }

        public ClientService(WebShopApiContext context)
        {
            WebShopApiContext = context;
        }

        public async Task<IEnumerable<ClientDto>> GetClients(bool isAdmin,CancellationToken cancellationToken)
        {
            var clients = await WebShopApiContext.Clients
                .AsNoTracking()
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken); 
            
            if (clients == null)
            {
                throw new NullReferenceException();
            }
            
            if (isAdmin == false)
            {
                throw new ArgumentException("You are not an admin");
            }
            
            return clients;
        }

        public async Task<ClientDto> GetById(int id, bool isAdmin, CancellationToken cancellationToken)
        {
            var client = await WebShopApiContext.Clients
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (isAdmin == false)
            {
                throw new ArgumentException("You are not an admin");
            }

            if (client == null)
            {
                throw new NullReferenceException("The client doesn't exist");
            }

            var result = client.ToDto();

            return result;
        }
        
        public async Task<ClientDto> GetByEmailAndPassword(string email, string password, CancellationToken cancellationToken)
        {
            var client = await WebShopApiContext.Clients
                .AsNoTracking()
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (client == null)
            {
                throw new NullReferenceException("The client doesn't exist");
            }

            /*var hashedPassword = PasswordHasher.HashPassword(password);
            
            if (client.Password != hashedPassword)
            {
                throw new Exception();
            }*/

            if (client.Password != password)
            {
                throw new Exception();
            }
            
            var result = client.ToDto();

            return result;
        }

        public async Task<bool> Update(int id, ClientDto clientEntity, CancellationToken cancellationToken)
        {
            var clientToUpdate = await WebShopApiContext.Clients
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (clientToUpdate == null)
            {
                throw new NullReferenceException("The client doesn't exist");
            }

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
                .FirstOrDefaultAsync(cancellationToken);
            
            if (clientToDelete == null)
            {
                throw new NullReferenceException("The client doesn't exist");
            }

            WebShopApiContext.Clients.Remove(clientToDelete);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        
        public async Task<bool> BanUser(int id, bool isAdmin, CancellationToken cancellationToken)
        {
            var userToBan = await WebShopApiContext.Clients
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (isAdmin == false)
            {
                throw new ArgumentException("You are not an admin");
            }

            if (userToBan == null)
            {
                throw new NullReferenceException("The client doesn't exist");
            }

            if (userToBan.IsBaned)
            {
                throw new Exception("The user has already been banned");
            }

            userToBan.IsBaned = true;
            
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        
        public async Task<bool> UnbanUser(int id, bool isAdmin, CancellationToken cancellationToken)
        {
            var userToUnban = await WebShopApiContext.Clients
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (isAdmin == false)
            {
                throw new ArgumentException("You are not an admin");
            }

            if (userToUnban == null)
            {
                throw new NullReferenceException("The client doesn't exist");
            }

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
