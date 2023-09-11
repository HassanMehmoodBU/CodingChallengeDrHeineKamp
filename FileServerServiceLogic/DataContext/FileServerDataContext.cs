using FileServerServiceLogic.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FileServerServiceLogic.DataContext
{
    public class FileServerDataContext : IdentityDbContext<User>
    {

        public FileServerDataContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Document> Documents { get; set; }

        public DbSet<ShareLog> ShareLogs { get; set; }

    }
}
