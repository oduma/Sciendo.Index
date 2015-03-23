using System;

namespace Sciendo.Music.Contracts.Monitoring
{
    public interface IFolderMonitor
    {
        void Stop();
        Func<string, int>[] ProcessFile { set; }
        bool More { get; }
        void Start();
    }
}
