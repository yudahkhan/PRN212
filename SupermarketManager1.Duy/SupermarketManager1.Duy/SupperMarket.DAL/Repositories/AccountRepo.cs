using Microsoft.EntityFrameworkCore;
using SupperMarket.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupperMarket.DAL.Repositories
{
    public class AccountRepo
    {
        private SupermarketDb3Context? _ctx;

        // Lấy account theo username
        public Account? GetByUsername(string username)
        {
            _ctx = new();
            return _ctx.Accounts
                .Include(a => a.Role)
                .Include(a => a.Warehouse)
                .FirstOrDefault(a => a.Username == username);
        }

        // Validate login (username và password)
        public Account? ValidateLogin(string username, string password)
        {
            _ctx = new();
            var account = _ctx.Accounts
                .Include(a => a.Role)
                .Include(a => a.Warehouse)
                .FirstOrDefault(a => a.Username == username && a.Password == password);

            // Kiểm tra status
            if (account != null && account.Status == "Active")
            {
                return account;
            }

            return null;
        }

        // Lấy tất cả accounts
        public List<Account> GetAll()
        {
            _ctx = new();
            return _ctx.Accounts
                .Include(a => a.Role)
                .Include(a => a.Warehouse)
                .ToList();
        }

        // Lấy account theo ID
        public Account? GetById(int accountId)
        {
            _ctx = new();
            return _ctx.Accounts
                .Include(a => a.Role)
                .Include(a => a.Warehouse)
                .FirstOrDefault(a => a.AccountId == accountId);
        }

        // Create account
        public void Create(Account account)
        {
            _ctx = new();
            _ctx.Accounts.Add(account);
            _ctx.SaveChanges();
        }

        // Update account
        public void Update(Account account)
        {
            _ctx = new();
            _ctx.Accounts.Update(account);
            _ctx.SaveChanges();
        }

        // Delete account
        public void Delete(Account account)
        {
            _ctx = new();
            _ctx.Accounts.Remove(account);
            _ctx.SaveChanges();
        }
    }
}

