using Cheetah;
using CheetahDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CeetahDAL
{
    public class DataContext: DbContext
    {
        protected readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            //options.UseNpgsql("Host=localhost; Database=test; Username=postgres; Password=g1p2l3a4");
            //  options.UseSqlServer(_configuration.GetConnectionString("sqlConnectionString"));
            //יש לשנות לפי הדטהבייס
            options.UseSqlServer(@"server=(localDb)\msSQlLocalDb;database=CheetahHenyEitan;Trusted_Connection=True");

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

    }
}
