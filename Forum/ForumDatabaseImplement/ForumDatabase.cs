using System;
using System.Collections.Generic;
using System.Text;
using ForumDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumDatabaseImplement
{
    class ForumDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ForumDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Models.Object> Objects { set; get; }
        public virtual DbSet<Topic> Topics { set; get; }
        public virtual DbSet<Thread> Threads { set; get; }
        public virtual DbSet<Person> Persons { set; get; }
        public virtual DbSet<Message> Messages { set; get; }
    }
}
