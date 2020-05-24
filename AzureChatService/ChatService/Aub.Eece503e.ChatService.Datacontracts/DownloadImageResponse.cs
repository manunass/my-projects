using System;
using System.Collections.Generic;
using System.Text;

namespace Aub.Eece503e.ChatService.Datacontracts
{
    public class DownloadImageResponse
    {
        public DownloadImageResponse(Byte[] bytes)
        {
            ImageData = bytes;
        }
        public byte[] ImageData { get; set; }
    }
}
