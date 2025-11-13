using Microsoft.EntityFrameworkCore;
using SupperMarket.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupperMarket.DAL.Repositories
{
    public class SaleRepo
    {
        private SupermarketDb3Context? _ctx;

        // Tạo Sale mới
        public void Create(Sale sale)
        {
            _ctx = new();
            _ctx.Sales.Add(sale);
            _ctx.SaveChanges();
        }

        // Lấy tất cả Sales
        public List<Sale> GetAll()
        {
            _ctx = new();
            return _ctx.Sales
                .Include(s => s.Account)
                .Include(s => s.Warehouse)
                .Include(s => s.ProductCodeNavigation)
                .OrderByDescending(s => s.SaleDate)
                .ToList();
        }

        // Lấy Sales theo Warehouse
        public List<Sale> GetByWarehouse(int warehouseId)
        {
            _ctx = new();
            return _ctx.Sales
                .Include(s => s.Account)
                .Include(s => s.Warehouse)
                .Include(s => s.ProductCodeNavigation)
                .Where(s => s.WarehouseId == warehouseId)
                .OrderByDescending(s => s.SaleDate)
                .ToList();
        }

        // Lấy Sales theo Account (Staff)
        public List<Sale> GetByAccount(int accountId)
        {
            _ctx = new();
            return _ctx.Sales
                .Include(s => s.Account)
                .Include(s => s.Warehouse)
                .Include(s => s.ProductCodeNavigation)
                .Where(s => s.AccountId == accountId)
                .OrderByDescending(s => s.SaleDate)
                .ToList();
        }

        // Lấy Sales theo ngày
        public List<Sale> GetByDate(DateTime date)
        {
            _ctx = new();
            return _ctx.Sales
                .Include(s => s.Account)
                .Include(s => s.Warehouse)
                .Include(s => s.ProductCodeNavigation)
                .Where(s => s.SaleDate.Date == date.Date)
                .OrderByDescending(s => s.SaleDate)
                .ToList();
        }

        // Lấy Sales theo khoảng thời gian
        public List<Sale> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            _ctx = new();
            return _ctx.Sales
                .Include(s => s.Account)
                .Include(s => s.Warehouse)
                .Include(s => s.ProductCodeNavigation)
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .OrderByDescending(s => s.SaleDate)
                .ToList();
        }
    }
}
