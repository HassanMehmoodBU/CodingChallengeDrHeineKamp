using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Helpers
{
    public static class FileExtensionHelper
    {
        static FileExtensionHelper()
        {
            ContentTypes = new Dictionary<string, string>
            {
                {".doc", "application/msword"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel" },
                {".xlsx", "	application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                {".txt", "text/plain" },
                {".pdf", "application/pdf" }
            };
        }

        public static string GetContentType(string extension)
        {
            if (ContentTypes.TryGetValue(extension.ToLower(), out var contentType))
            {
                return contentType;
            }

            return BinaryDefaultContentType;
        }

        private static readonly IDictionary<string, string> ContentTypes;
        private const string BinaryDefaultContentType = "application/octet-stream";
    }
}
