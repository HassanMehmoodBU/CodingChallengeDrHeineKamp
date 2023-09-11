using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileServerServiceLogic.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string dataType, string key) : base($"Data entity {dataType} was not found by Key {key}")
        {

        }
    }
}
