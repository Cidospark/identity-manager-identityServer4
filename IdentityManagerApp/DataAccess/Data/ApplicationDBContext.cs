using System;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options){ }

        public DbSet<User> Users { get; set; }
    }
}

