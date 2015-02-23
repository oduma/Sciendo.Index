using System;

namespace Sciendo.Lyrics.Provider.Service
{
    class NoExecutionContextException : Exception
    {

        public NoExecutionContextException(string message=null):base(message)
        {
        }
    }
}
