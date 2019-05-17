﻿using Samples.Models;
using Shiny.Net.Http;
using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Samples.ShinyDelegates
{
    public class HttpTransferDelegate : IHttpTransferDelegate
    {
        readonly CoreDelegateServices services;
        public HttpTransferDelegate(CoreDelegateServices services) => this.services = services;


        public async void OnError(HttpTransfer transfer, Exception ex)
            => await this.CreateHttpTransferEvent(transfer, "ERROR: " + ex);


        public async void OnCompleted(HttpTransfer transfer)
        {
            if (!transfer.IsUpload && Path.GetExtension(transfer.LocalFilePath) == "db")
            {
                try
                {
                    using (var conn = new SQLiteConnection(transfer.LocalFilePath))
                    {
                        var count = conn.Execute("SELECT COUNT(*) FROM sqlite_master WHERE type='table'");
                        await this.CreateHttpTransferEvent(transfer, $"COMPLETE - SQLITE PASSED ({count} tables)");
                    }
                }
                catch (Exception ex)
                {
                    await this.CreateHttpTransferEvent(transfer, $"COMPLETE - SQLITE FAILED - " + ex);
                }
            }
            else
            {
                await this.CreateHttpTransferEvent(transfer, "COMPLETE");
            }
        }


        async Task CreateHttpTransferEvent(HttpTransfer transfer, string description)
        {
            await this.services.Connection.InsertAsync(new HttpEvent
            {
                Identifier = transfer.Identifier,
                IsUpload = transfer.IsUpload,
                FileSize = transfer.FileSize,
                Uri = transfer.Uri,
                Description = description,
                DateCreated = DateTime.Now
            });
            await this.services.SendNotification(
                "HTTP Transfer",
                description,
                x => x.UseNotificationsHttpTransfers
            );
        }
    }
}
