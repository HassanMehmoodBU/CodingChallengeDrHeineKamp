using FileServerServiceLogic.Contracts.DocumentsDownload;
using FileServerServiceLogic.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Providers
{
    public interface IDocumentsProvider
    {
        Task<List<Document>> GetAllDocs(Guid userId);
        Task<ExtDownloadFileData> GetFileData(Guid fileGuid);
        Task<ExtDownloadFileData> CheckShareAbleAndGetFileData(Guid fileGuid);
    }
}