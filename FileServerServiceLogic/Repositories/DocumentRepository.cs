using FileServerServiceLogic.DataContext;
using FileServerServiceLogic.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly FileServerDataContext _fileServerDataContext;

        public DocumentRepository(FileServerDataContext fileServerDataContext)
        {
            _fileServerDataContext = fileServerDataContext;
        }

        public async Task<Guid> SaveDocument(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            _fileServerDataContext.Documents.Add(document);
            await _fileServerDataContext.SaveChangesAsync();
            return document.DocumentId;
        }

        public async Task<List<Document>> GetAllDocs(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return await _fileServerDataContext.Documents.ToListAsync();

        }

        public async Task<Document> GetSingleDocument(Guid fileGuid)
        {
            if (fileGuid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(fileGuid));
            }

            var entity = await _fileServerDataContext.Documents.FirstOrDefaultAsync(doc => doc.DocumentId == fileGuid);
            if (entity != null)
            {
                entity.NoOfDownloads++;
                await _fileServerDataContext.SaveChangesAsync();
            }

            return entity;


        }

        public async Task<Document> CheckShareAbleAndGetSingleDocument(Guid fileGuid)
        {
            if (fileGuid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(fileGuid));
            }

            var entity = await _fileServerDataContext.Documents.FirstOrDefaultAsync(doc => doc.DocumentId == fileGuid && doc.IsShareAble == true);

            if (entity != null)
            {
                if (entity.ShareExpiry >= DateTime.Now)
                {
                    entity.NoOfDownloads++;
                    await _fileServerDataContext.SaveChangesAsync();
                }
                else
                {
                    return null;
                }
            }

            return entity;
        }

        public async Task<Document> GenerateShareAbleLink(Guid fileId, int days, int hours)
        {
            if (fileId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(fileId));
            }

            var entity = await _fileServerDataContext.Documents.FirstOrDefaultAsync(doc => doc.DocumentId == fileId);
            entity.IsShareAble = true;
            var expiryTime = DateTime.Now.AddDays(days).AddHours(hours);
            entity.ShareExpiry = expiryTime;
            await _fileServerDataContext.SaveChangesAsync();
            return entity;

        }

    }
}
