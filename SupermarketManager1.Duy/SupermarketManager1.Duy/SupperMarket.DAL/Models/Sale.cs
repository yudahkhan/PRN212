using System;
using System.Collections.Generic;

namespace SupperMarket.DAL.Models;

public partial class Sale
{
    public int SaleId { get; set; }

    public int AccountId { get; set; }

    public int WarehouseId { get; set; }  // ⭐ MỚI: Bán từ kho nào

    public string ProductCode { get; set; } = null!;

    public int QuantitySold { get; set; }  // ⭐ SỬA: NOT NULL

    public decimal UnitPrice { get; set; }  // ⭐ MỚI: Giá tại thời điểm bán

    public decimal TotalAmount { get; set; }  // ⭐ MỚI: Tổng tiền = QuantitySold × UnitPrice

    public DateTime SaleDate { get; set; }  // ⭐ SỬA: NOT NULL

    public virtual Account Account { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;  // ⭐ MỚI: Navigation property

    public virtual Product ProductCodeNavigation { get; set; } = null!;
}
