using Sciendo.Music.Contracts.Solr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Real.Feedback
{
    public static class DocumentsExtensions
    {
        public static string Stringify(this Document[] documents)
        {
            if (documents == null || documents.Length <= 0)
                return string.Empty;
            return documents[0].FilePathId.Substring(0,documents[0].FilePathId.LastIndexOf('\\'));
            
        }
    }
}
