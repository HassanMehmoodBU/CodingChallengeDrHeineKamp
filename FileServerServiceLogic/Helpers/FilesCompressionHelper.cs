using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Helpers
{
    public static class FilesCompressionHelper
    {
        public static byte[] ZipFilesBatch(Tuple<string, byte[]>[] batch)
        {
            using (var result = new MemoryStream())
            {
                ZipFilesBatchStream(result, batch);

                return result.ToArray();
            }
        }

        public static Stream ZipFilesBatchStream(Tuple<string, byte[]>[] batch)
        {
            using (var result = new MemoryStream())
            {
                return ZipFilesBatchStream(result, batch);
            }
        }

        private static Stream ZipFilesBatchStream(Stream result, Tuple<string, byte[]>[] batch)
        {
            using (var archive = new ZipArchive(result, ZipArchiveMode.Create, true))
            {
                foreach (var file in batch)
                {
                    var entry = archive.CreateEntry(file.Item1);

                    using (var entryStream = entry.Open())
                    {
                        entryStream.Write(file.Item2, 0, file.Item2.Length);
                    }
                }
            }

            return result;
        }
    }
}

