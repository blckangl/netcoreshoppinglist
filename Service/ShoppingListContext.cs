using Microsoft.EntityFrameworkCore;
using shoppinglist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shoppinglist.Service
{
    public class ShoppingListContext :DbContext
    {

        public DbSet<Person> persons { get; set; }
        public DbSet<ShoppingItem> items { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=shoppinglist;user=root;password=");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Age).IsRequired();
            });


            modelBuilder.Entity<ShoppingItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.qte).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValue(new DateTime());
            });

        }

    }
}
