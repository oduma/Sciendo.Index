using System;

namespace Sciendo.Music.Contracts.Monitoring
{
    public interface IFolderMonitor
    {
        void Stop();
        Action<string,ProcessType> ProcessFile { set; }
        bool More { get; }
        void Start();
    }
}
