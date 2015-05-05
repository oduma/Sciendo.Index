using System;
using System.Collections.Generic;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Real.Procesors.Configuration;

namespace Sciendo.Music.Real.Procesors.Common
{
    public abstract class FilesProcessorBase<TIn>
    {

        public AgentConfigurationSource CurrentConfiguration { get; protected set; }

        public int Counter { get; protected set; }

        protected FilesProcessorBase()
        {
            Counter = 0;
        }

        public void ResetCounter()
        {
            LoggingManager.Debug("Reseting Counter...");
            Counter = 0;
            LoggingManager.Debug("Counter reseted.");
        }

        public abstract void ProcessFilesBatch(IEnumerable<string> files, Action<Status, string> progressEvent);

        protected abstract IEnumerable<T> TransformFiles<T>(IEnumerable<string> files, Func<TIn, string, T> specfifcTranformMethod);

    }
}
