using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Contexts;
using WebShop.Domain.Entities;
using WebShop.Domain.Models;
using WebShop.Services.Interfaces;
using WebShop.Services.Mappers;

namespace WebShop.Services.Services
{
    public class DiscountService : IDiscountService

    {
        private WebShopApiContext WebShopApiContext { get; }

        public DiscountService(WebShopApiContext context)
        {
            WebShopApiContext = context;
        }

        public async Task<IEnumerable<DiscountDto>> GetDiscounts(bool isAdmin, CancellationToken cancellationToken)
        {
            var discounts = await WebShopApiContext.Discounts
                .AsNoTracking()
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken);

            if (isAdmin == false)
            {
                throw new ArgumentException("You are not an admin");
            }
            
            return discounts;
        }

        public async Task<IEnumerable<DiscountDto>> GetClientsDiscounts(int id, CancellationToken cancellationToken)
        {
            var discounts = await WebShopApiContext.Discounts
                .AsNoTracking()
                .Where(x => x.ClientId == id)
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken);

            if (discounts.Any() == false) 
            {
                throw new NullReferenceException();
            }

            return discounts;
        }

        public async Task<DiscountDto> GetById(int id, bool isAdmin, CancellationToken cancellationToken)
        {
            var discount = await WebShopApiContext.Discounts
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (isAdmin == false)
            {
                throw new ArgumentException("You are not an admin");
            }

            if (discount == null)
            {
                throw new NullReferenceException();
            }

            var result = discount.ToDto();

            return result;
        }

        public async Task<DiscountDto> Create(DiscountDto newDiscountEntity, bool isAdmin, CancellationToken cancellationToken)
        {
            if (isAdmin == false)
            {
                throw new ArgumentException("You are not an admin");
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

        public async Task<bool> Update(int id, DiscountDto discountEntity, bool isAdmin, CancellationToken cancellationToken)
        {
            var discountToUpdate = await WebShopApiContext.Discounts
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (isAdmin == false)
            {
                throw new ArgumentException("You are not an admin");
            }

            if (discountToUpdate == null)
            {
                throw new NullReferenceException();
            }

            discountToUpdate.ProductId = discountEntity.ProductId;
            discountToUpdate.Discount = discountEntity.Discount;

            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> Delete(int id, bool isAdmin, CancellationToken cancellationToken)
        {
            var discountToDelete = await WebShopApiContext.Discounts
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (isAdmin == false)
            {
                throw new ArgumentException("You are not an admin");
            }

            if (discountToDelete == null)
            {
                throw new NullReferenceException();
            }

            WebShopApiContext.Discounts.Remove(discountToDelete);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
        
        