using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Contracts.ShareAbleLink
{
    public class ExtShareAbleLink
    {
        public string documentId { get; set; }
        public int Days { get; set; }
        public int Hours { get; set; }

    }
}
