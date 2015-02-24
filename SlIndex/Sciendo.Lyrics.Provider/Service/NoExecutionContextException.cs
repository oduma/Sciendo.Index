using System;

namespace Sciendo.Lyrics.Provider.Service
{
    [Serializable]
    class NoExecutionContextException : Exception
    {

        public NoExecutionContextException(string message=null):base(message)
        {
        }
    }
}
