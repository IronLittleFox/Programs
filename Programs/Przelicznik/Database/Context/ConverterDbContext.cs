using Microsoft.EntityFrameworkCore;
using Przelicznik.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przelicznik.Database.Context
{
    //add-migration CreateDatabaseFirstMigration -outputdir Database\Migrations

    class ConverterDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("filename=converterDb.db");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitConverter> UnitConverters { get; set; }
    }
}
