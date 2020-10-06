using System;
using System.Collections.Generic;

namespace BusinessLine.Core.Application.Messages.Commands.SendMessage
{
    public class SendMessageModel
    {
        public Guid Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public KeyValuePair<string, string>[] Params { get; set; }
    }
}
