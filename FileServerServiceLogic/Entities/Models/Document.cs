using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Entities.Models
{
    public class Document
    {
        public Guid DocumentId { get; set; }

        public string DocumentName { get; set; }

        public string DocumentType { get; set; }

        public DateTime UploadDateTime { get; set; }

        public int NoOfDownloads { get; set; }

        public bool IsShareAble { get; set; }

        public string ShareAbleLink { get; set; }

        public DateTime ShareExpiry { get; set; }

        public Guid UploadedBy { get; set; }


    }
}
