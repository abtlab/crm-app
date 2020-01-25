﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Decimal;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Products.Helpers;
using Crm.Apps.Products.Storages;
using Crm.Apps.Products.v1.Models;
using Crm.Apps.Products.v1.RequestParameters;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Products.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ProductsStorage _storage;

        public ProductsService(ProductsStorage storage)
        {
            _storage = storage;
        }

        public Task<Product> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Products
                .AsNoTracking()
                .Include(x => x.Status)
                .Include(x => x.AttributeLinks)
                .Include(x => x.CategoryLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Product>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Products
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<List<Product>> GetPagedListAsync(
            ProductGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            var temp = await _storage.Products
                .AsNoTracking()
                .Include(x => x.Status)
                .Include(x => x.AttributeLinks)
                .Include(x => x.CategoryLinks)
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.ParentProductId.IsEmpty() || x.ParentProductId == request.ParentProductId) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (request.VendorCode.IsEmpty() || x.VendorCode == request.VendorCode) &&
                    (request.MinPrice.IsEmpty() || x.Price >= request.MinPrice.Value) &&
                    (request.MaxPrice.IsEmpty() || x.Price <= request.MaxPrice) &&
                    (!request.IsHidden.HasValue || x.IsHidden == request.IsHidden) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .SortBy(request.SortBy, request.OrderBy)
                .ToListAsync(ct);

            return temp
                .Where(x => x.FilterByAdditional(request))
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToList();
        }

        public async Task<Guid> CreateAsync(Guid userId, Product product, CancellationToken ct)
        {
            var newProduct = new Product();
            var change = newProduct.CreateWithLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = product.AccountId;
                x.ParentProductId = product.ParentProductId;
                x.Type = product.Type;
                x.StatusId = product.StatusId;
                x.Name = product.Name;
                x.VendorCode = product.VendorCode;
                x.Price = product.Price;
                x.Image = product.Image;
                x.IsHidden = product.IsHidden;
                x.IsDeleted = product.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.AttributeLinks = product.AttributeLinks?
                    .Select(l => new ProductAttributeLink
                    {
                        ProductId = x.Id,
                        ProductAttributeId = l.ProductAttributeId,
                        Value = l.Value,
                        CreateDateTime = DateTime.UtcNow
                    })
                    .ToList();
                x.CategoryLinks = product.CategoryLinks?
                    .Select(l => new ProductCategoryLink
                    {
                        ProductId = x.Id,
                        ProductCategoryId = l.ProductCategoryId,
                        CreateDateTime = DateTime.UtcNow
                    })
                    .ToList();
            });

            var entry = await _storage.AddAsync(newProduct, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid productId, Product oldProduct, Product newProduct, CancellationToken ct)
        {
            var change = oldProduct.UpdateWithLog(productId, x =>
            {
                x.AccountId = newProduct.AccountId;
                x.ParentProductId = newProduct.ParentProductId;
                x.Type = newProduct.Type;
                x.StatusId = newProduct.StatusId;
                x.Name = newProduct.Name;
                x.VendorCode = newProduct.VendorCode;
                x.Price = newProduct.Price;
                x.Image = newProduct.Image;
                x.IsHidden = newProduct.IsHidden;
                x.IsDeleted = newProduct.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
                x.AttributeLinks = newProduct.AttributeLinks;
                x.CategoryLinks = newProduct.CategoryLinks;
            });

            _storage.Update(oldProduct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task HideAsync(Guid productId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ProductChange>();

            await _storage.Products
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(productId, x =>
                {
                    x.IsHidden = true;
                    x.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task ShowAsync(Guid productId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ProductChange>();

            await _storage.Products
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(productId, x =>
                {
                    x.IsHidden = false;
                    x.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid productId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ProductChange>();

            await _storage.Products
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(productId, x =>
                {
                    x.IsDeleted = true;
                    x.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid productId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ProductChange>();

            await _storage
                .Products.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(productId, x =>
                {
                    x.IsDeleted = false;
                    x.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}