using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Entities.Models
{
    public class ShareLog
    {
        public int Id { get; set; }

        public int DocumentId { get; set; }

        public DateTime ExpireTime { get; set; }

        public DateTime StartTime { get; set; }

        public bool IsShareActive { get; set; }

    }
}
