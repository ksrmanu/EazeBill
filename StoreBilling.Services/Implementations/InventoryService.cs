using System;
using System.Collections.Generic;
using System.Linq;
using StoreBilling.Common;
using StoreBilling.Data;
using StoreBilling.Data.Entities;
using StoreBilling.Services.DTOs;
using StoreBilling.Services.Interfaces;

namespace StoreBilling.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly StoreDbContext _db;

        public InventoryService(StoreDbContext db)
        {
            _db = db;
        }

        public decimal GetStock(int productId, int storeId)
        {
            return _db.InventoryTransactions
                .Where(t => t.ProductId == productId && t.StoreId == storeId && !t.IsDeleted)
                .Select(t => (decimal?)(t.QtyIn - t.QtyOut))
                .Sum() ?? 0m;
        }

        public IEnumerable<ProductStockDto> GetStockOverview(int storeId)
        {
            return _db.Products
                .Where(p => p.IsActive && !p.IsDeleted)
                .Select(p => new ProductStockDto
                {
                    ProductId = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    Category = p.Category.Name,
                    MRP = p.MRP,
                    MinStock = p.MinStock,
                    PurchaseRate = p.PurchaseRate,
                    Stock = _db.InventoryTransactions
                        .Where(t => t.ProductId == p.Id && t.StoreId == storeId)
                        .Sum(t => (decimal?)(t.QtyIn - t.QtyOut)) ?? 0m
                }).ToList();
        }

        public void RecordIn(int productId, int storeId, decimal qty, string refType, int refId, string remarks = null)
        {
            _db.InventoryTransactions.Add(new InventoryTransaction
            {
                ProductId = productId,
                StoreId = storeId,
                QtyIn = qty,
                QtyOut = 0,
                RefType = refType,
                RefId = refId,
                Remarks = remarks
            });
        }

        public void RecordOut(int productId, int storeId, decimal qty, string refType, int refId, string remarks = null)
        {
            var available = GetStock(productId, storeId);
            if (available < qty)
            {
                throw new InvalidOperationException("Insufficient stock available for this product.");
            }

            _db.InventoryTransactions.Add(new InventoryTransaction
            {
                ProductId = productId,
                StoreId = storeId,
                QtyIn = 0,
                QtyOut = qty,
                RefType = refType,
                RefId = refId,
                Remarks = remarks
            });
        }
    }
}