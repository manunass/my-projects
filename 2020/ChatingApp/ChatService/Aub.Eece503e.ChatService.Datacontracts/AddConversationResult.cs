﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Aub.Eece503e.ChatService.Datacontracts
{
    public class AddConversationResult
    {
        public string Id { get; set; }
        public long CreatedUnixTime { get; set; }
    }
}
