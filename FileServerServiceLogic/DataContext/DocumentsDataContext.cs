using FileServerServiceLogic.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerServiceLogic.DataContext
{
    public class DocumentsDataContext : DbContext
    {
        public DocumentsDataContext(DbContextOptions<DocumentsDataContext> options) : base(options) { }

        public DbSet<Document> Documents { get; set; }

        public DbSet<ShareLog> ShareLogs { get; set; }

    }
}
