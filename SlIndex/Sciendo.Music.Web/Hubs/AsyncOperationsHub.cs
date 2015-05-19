﻿using Microsoft.AspNet.SignalR;
using Sciendo.Music.DataProviders;
using Sciendo.Music.DataProviders.Models.Indexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sciendo.Music.Web.Hubs
{
    public class AsyncOperationsHub:Hub
    {
        public void StartIndexing(string fromPath)
        {
            try
            {
                SciendoConfiguration.Container.Resolve<IDataProvider>(
                            SciendoConfiguration.IndexingConfiguration.CurrentDataProvider)
                            .StartIndexing(fromPath, StartIndexingOnDemandCompletedCallback);

            }
            catch (Exception ex)
            {
                StartIndexingOnDemandCompletedCallback(this, new IndexOnDemandCompletedEventArgs(new object[0] { }, ex, false, null));
            }
        }

        private void StartIndexingOnDemandCompletedCallback(object arg1, IndexOnDemandCompletedEventArgs arg2)
        {
            IndexingResult result;
            if (arg2.Error == null)
            {
                result = new IndexingResult { NumberOfDocuments = arg2.Result.ToString() };
            }
            else
            {
                result = new IndexingResult { NumberOfDocuments = "", Error = arg2.Error.Message };
            }
            Clients.All.returnCompletedMessage(result);
        }


        public void StartAcquiringLyrics(string fromPath, bool retryExisting)
        {
            try
            {
                SciendoConfiguration.Container.Resolve<IDataProvider>(
                        SciendoConfiguration.IndexingConfiguration.CurrentDataProvider)
                        .StartAcquyringLyrics(fromPath, retryExisting, StartAcquiringLyricsCompletedCallback);
            }
            catch (Exception ex)
            {
                StartAcquiringLyricsCompletedCallback(this, new AcquireLyricsOnDemandForCompletedEventArgs(new object[0] { }, ex, false, null));
            }
        }

        private void StartAcquiringLyricsCompletedCallback(object arg1, AcquireLyricsOnDemandForCompletedEventArgs arg2)
        {
            IndexingResult result;
            if (arg2.Error == null)
            {
                result = new IndexingResult { NumberOfDocuments = arg2.Result.ToString() };
            }
            else
            {
                result = new IndexingResult { NumberOfDocuments = "", Error = arg2.Error.Message };
            }
            Clients.All.returnCompletedMessage(result);
        }

    }
}