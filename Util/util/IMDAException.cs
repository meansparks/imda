using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Util.util
{
    [SerializableAttribute]
    public class IMDAException : Exception, ISerializable
    {
        public IMDAException()
        {
        }
        public IMDAException(string message) : base(message)
        {
        }
        public IMDAException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
