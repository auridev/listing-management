using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Messages.Queries.GetMyMessageDetails
{
    public class MyMessageDetailsModel
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public KeyValuePair<string, string>[] Params { get; set; }
        public bool Seen { get; set; }
    }
}
