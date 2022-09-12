using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Contexts;
using WebShop.Domain.Entities;
using WebShop.Domain.Models;
using WebShop.Services.Extentions;
using WebShop.Services.Interfaces;
using WebShop.Services.Mappers;

namespace WebShop.Services.Services
{
    public class DiscountService : IDiscountService

    {
        private WebShopApiContext WebShopApiContext { get; }
        private ICurrentUserService CurrentUserService { get; }

        public DiscountService(WebShopApiContext context, ICurrentUserService currentUserService)
        {
            WebShopApiContext = context;
            CurrentUserService = currentUserService;
        }

        public async Task<IEnumerable<DiscountDto>> GetDiscounts(CancellationToken cancellationToken)
        {
             bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
             if (isAdmin == false)
             {
                 throw new UnauthorizedAccessException("You are not an admin");
             }
             
             var discounts = await WebShopApiContext.Discounts
                .AsNoTracking()
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken);
             
            return discounts;
        }

        public async Task<IEnumerable<DiscountDto>> GetClientsDiscounts(int id, CancellationToken cancellationToken)
        {
            var discounts = await WebShopApiContext.ClientsDiscounts
                .AsNoTracking()
                .Include(x => x.DiscountEntity)
                .Where(x => x.ClientId == id)
                .Select(x => x.DiscountEntity.ToDto())
                .ToListAsync(cancellationToken);

            return discounts;
        }

        public async Task<DiscountDto> GetById(int id, CancellationToken cancellationToken)
        {
            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }
            
            var discount = await WebShopApiContext.Discounts
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);

            var result = discount.ToDto();

            return result;
        }

        public async Task<DiscountDto> Create(DiscountDto newDiscountEntity, CancellationToken cancellationToken)
        {
            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }

            var newEntity = new DiscountEntity()
            {
                ProductId = newDiscountEntity.ProductId,
                Discount = newDiscountEntity.Discount
            };

            WebShopApiContext.Discounts.Add(newEntity);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            var result = newEntity.ToDto();

            return result;
        }

        public async Task<bool> Update(int id, DiscountDto discountEntity,CancellationToken cancellationToken)
        {
            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }
            
            var discountToUpdate = await WebShopApiContext.Discounts
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);

            discountToUpdate.ProductId = discountEntity.ProductId;
            discountToUpdate.Discount = discountEntity.Discount;

            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }
            
            var discountToDelete = await WebShopApiContext.Discounts
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);

            WebShopApiContext.Discounts.Remove(discountToDelete);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
        
        