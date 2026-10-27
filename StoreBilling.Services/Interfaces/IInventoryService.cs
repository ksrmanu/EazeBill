using System.Collections.Generic;
using StoreBilling.Services.DTOs;

namespace StoreBilling.Services.Interfaces
{
    public interface IInventoryService
    {
        decimal GetStock(int productId, int storeId);
        IEnumerable<ProductStockDto> GetStockOverview(int storeId);
        void RecordIn(int productId, int storeId, decimal qty, string refType, int refId, string remarks = null);
        void RecordOut(int productId, int storeId, decimal qty, string refType, int refId, string remarks = null);
    }
}

