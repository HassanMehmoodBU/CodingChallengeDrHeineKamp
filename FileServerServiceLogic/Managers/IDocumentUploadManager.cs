using FileServerServiceLogic.Contracts.DocumentsUpload;
using FileServerServiceLogic.Entities.Models;
using System;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Managers
{
    public interface IDocumentUploadManager
    {
        Task<ExtDocumentUploadResult> uploadDocuments(string fileName, string userName, Guid userId);

        Task<Document> generateLink(Guid fileID, int days, int hours);
    }
}