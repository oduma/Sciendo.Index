using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Lyrics.Provider
{
    class NoExecutionContextException : Exception
    {

        public NoExecutionContextException(string message=null):base(message)
        {
        }
    }
}
