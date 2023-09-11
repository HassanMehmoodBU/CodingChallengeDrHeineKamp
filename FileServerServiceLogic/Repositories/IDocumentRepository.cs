using FileServerServiceLogic.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Repositories
{
    public interface IDocumentRepository
    {
        Task<Guid> SaveDocument(Document document);

        Task<List<Document>> GetAllDocs(Guid userId);

        Task<Document> GetSingleDocument(Guid fileGuid);
        Task<Document> CheckShareAbleAndGetSingleDocument(Guid fileGuid);

        Task<Document> GenerateShareAbleLink(Guid fileId, int days, int hours);
    }
}