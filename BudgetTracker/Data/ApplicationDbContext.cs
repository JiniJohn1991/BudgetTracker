using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BudgetTracker.Models;
using BudgetTracker.Models.LoginViewModels;
using BudgetTracker.Models.BillDetailsViewModels;
using BudgetTracker.Models.MasterViewModels;

namespace BudgetTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserDetailsModel> UserDetails { get; set; }
        public DbSet<BillDetailsModel> BillDetails { get; set; }
        public DbSet<BillMasterModel> BillMaster { get; set; }
        public DbSet<ExpenseDetailsModel> ExpenseDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserDetailsModel>().ToTable("UserDetails");
            builder.Entity<BillDetailsModel>().ToTable("BillDetails");
            builder.Entity<BillMasterModel>().ToTable("BillMaster");
            builder.Entity< ExpenseDetailsModel>().ToTable("ExpenseDetails");
            base.OnModelCreating(builder);

        }
    }
}
