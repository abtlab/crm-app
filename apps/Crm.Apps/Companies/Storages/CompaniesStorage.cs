﻿using Crm.Apps.Companies.Models;
using Crm.Infrastructure.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Companies.Storages
{
    public class CompaniesStorage : DbContext
    {
        private readonly OrmSettings _config;

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyBankAccount> CompanyBankAccounts { get; set; }

        public DbSet<CompanyAttribute> CompanyAttributes { get; set; }

        public DbSet<CompanyAttributeChange> CompanyAttributeChanges { get; set; }

        public DbSet<CompanyChange> CompanyChanges { get; set; }

        public DbSet<CompanyComment> CompanyComments { get; set; }

        public CompaniesStorage(IOptions<OrmSettings> options)
        {
            _config = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(_config.MainConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyAttributeLink>()
                .ToTable("CompanyAttributeLinks")
                .Property(x => x.CompanyAttributeId)
                .HasColumnName("AttributeId");
        }
    }
}