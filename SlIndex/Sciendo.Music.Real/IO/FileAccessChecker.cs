using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Real.IO
{
    public static class FileAccessChecker
    {
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    stream = file.Open(FileMode.Open,
                         FileAccess.Read, FileShare.None);
                else
                    stream = file.Open(FileMode.Open,
                         FileAccess.ReadWrite, FileShare.None);

            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }


    }
}
