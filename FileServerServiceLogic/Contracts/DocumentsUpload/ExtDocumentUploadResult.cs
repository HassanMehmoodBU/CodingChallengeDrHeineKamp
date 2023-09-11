using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Contracts.DocumentsUpload
{
    public class ExtDocumentUploadResult
    {
        public Guid DocumentId { get; set; }

        public string DocumentName { get; set; }
    }
}
