using FileServerServiceLogic.Contracts.DocumentsDownload;
using FileServerServiceLogic.Entities.Models;
using FileServerServiceLogic.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Providers
{
    public class DocumentsProvider : IDocumentsProvider
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DocumentsProvider(IDocumentRepository documentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _documentRepository = documentRepository;
            _hostingEnvironment = webHostEnvironment;
        }

        public async Task<List<Document>> GetAllDocs(Guid userId)
        {
            var docs = await _documentRepository.GetAllDocs(userId);

            if (docs == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return docs.Where(dc => dc.UploadedBy == userId).ToList();

        }

        public async Task<ExtDownloadFileData> GetFileData(Guid fileGuid)
        {
            var doc = await _documentRepository.GetSingleDocument(fileGuid);

            if (doc == null)
            {
                throw new ArgumentNullException(nameof(fileGuid));
            }

            ExtDownloadFileData result = new ExtDownloadFileData
            {
                DocumentContents = await ReadFileToArray(doc.DocumentName),
                DocumentId = doc.DocumentId,
                DocumentName = doc.DocumentName

            };

            return result;
        }

        public async Task<ExtDownloadFileData> CheckShareAbleAndGetFileData(Guid fileGuid)
        {
            var doc = await _documentRepository.CheckShareAbleAndGetSingleDocument(fileGuid);

            if (doc == null)
            {
                throw new ArgumentNullException(nameof(fileGuid));
            }

            ExtDownloadFileData result = new ExtDownloadFileData
            {
                DocumentContents = await ReadFileToArray(doc.DocumentName),
                DocumentId = doc.DocumentId,
                DocumentName = doc.DocumentName

            };

            return result;
        }

        private async Task<byte[]> ReadFileToArray(string fileName)
        {
            string webRootPath = _hostingEnvironment.ContentRootPath;
            string uploadsDir = Path.Combine(webRootPath, "uploads");
            string fullPath = Path.Combine(uploadsDir, fileName);

            using (var fs = File.OpenRead(fullPath))
            {
                var result = new byte[fs.Length];
                await fs.ReadAsync(result, 0, (int)fs.Length);
                return result;
            }

        }

    }
}
