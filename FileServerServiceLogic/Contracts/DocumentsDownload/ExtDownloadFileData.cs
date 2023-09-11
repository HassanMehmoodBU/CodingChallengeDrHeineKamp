using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Contracts.DocumentsDownload
{
    public class ExtDownloadFileData
    {
        public Guid DocumentId { get; set; }

        public string DocumentName { get; set; }

        public byte[] DocumentContents { get; set; }
    }
}
