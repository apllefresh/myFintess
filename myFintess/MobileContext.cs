using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace myFintess
{
    class MobileContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Training> Trainings { get; set; }

        //public MobileContext()
        //{
        //    Database.EnsureCreated();
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=mobile.db");
        }
    }
}
