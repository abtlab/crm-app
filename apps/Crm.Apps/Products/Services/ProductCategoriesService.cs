using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Helpers;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.Parameters;
using Crm.Apps.Products.Storages;
using Crm.Utils.Guid;
using Crm.Utils.String;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Products.Services
{
    public class ProductCategoriesService : IProductCategoriesService
    {
        private readonly ProductsStorage _storage;

        public ProductCategoriesService(ProductsStorage storage)
        {
            _storage = storage;
        }

        public Task<ProductCategory> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.ProductCategories.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<ProductCategory>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.ProductCategories.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<ProductCategory>> GetPagedListAsync(ProductCategoryGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.ProductCategories.Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{parameter.Name}%")) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, ProductCategory category, CancellationToken ct)
        {
            var newCategory = new ProductCategory();
            var change = newCategory.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = category.AccountId;
                x.Name = category.Name;
                x.IsDeleted = category.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newCategory, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, ProductCategory oldCategory, ProductCategory newCategory,
            CancellationToken ct)
        {
            var change = oldCategory.WithUpdateLog(userId, x =>
            {
                x.Name = newCategory.Name;
                x.IsDeleted = newCategory.IsDeleted;
            });

            _storage.Update(oldCategory);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ProductCategoryChange>();

            await _storage.ProductCategories.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ProductCategoryChange>();

            await _storage.ProductCategories.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}