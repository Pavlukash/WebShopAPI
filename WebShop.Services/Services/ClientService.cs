using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Contexts;
using WebShop.Domain.Entities;
using WebShop.Domain.Models;
using WebShop.Services.Interfaces;
using WebShop.Services.Mappers;
using Microsoft.EntityFrameworkCore;
using WebShop.Services.Auth;


namespace WebShop.Services.Services
{
    public class ClientService : IClientService
    {
        private WebShopApiContext WebShopApiContext { get; }

        public ClientService(WebShopApiContext context)
        {
            WebShopApiContext = context;
        }

        public async Task<IEnumerable<ClientDto>> GetClients(CancellationToken cancellationToken)
        {
            var clients = await WebShopApiContext.Clients
                .AsNoTracking()
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken);

            return clients;
        }

        public async Task<ClientDto> GetById(int id, CancellationToken cancellationToken)
        {
            var client = await WebShopApiContext.Clients
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (client == null)
            {
                throw new Exception();
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
                throw new Exception();
            }

            var hashedPassword = PasswordHasher.HashPassword(password);
            
            if (client.Password != hashedPassword)
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
                throw new Exception();
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

            WebShopApiContext.Clients.Remove(clientToDelete);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
