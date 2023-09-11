using FileServerServiceLogic.Contracts.DocumentsUpload;
using FileServerServiceLogic.Entities.Models;
using FileServerServiceLogic.Repositories;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Managers
{
    public class DocumentUploadManager : IDocumentUploadManager
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentUploadManager(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<ExtDocumentUploadResult> uploadDocuments(string fileName, string userName, Guid userId)
        {
            try
            {
                if (fileName == null || userName == null)
                {
                    throw new ArgumentNullException(nameof(fileName));
                }

                Document toUpload = new Document
                {
                    DocumentId = Guid.NewGuid(),
                    DocumentName = fileName,
                    DocumentType = "file",
                    NoOfDownloads = 0,
                    IsShareAble = false,
                    ShareAbleLink = null,
                    UploadDateTime = DateTime.Now,
                    UploadedBy = userId

                };

                var docGuid = await _documentRepository.SaveDocument(toUpload);
                return new ExtDocumentUploadResult
                {
                    DocumentId = docGuid,
                    DocumentName = fileName
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Document> generateLink(Guid fileID, int days, int hours)
        {
            try
            {
                var result = await _documentRepository.GenerateShareAbleLink(fileID, days, hours);
                if (result == null)
                {
                    throw new ArgumentNullException(nameof(fileID));
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
