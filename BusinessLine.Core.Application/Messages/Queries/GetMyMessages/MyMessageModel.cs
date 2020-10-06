using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLine.Core.Application.Messages.Queries.GetMyMessages
{
    public class MyMessageModel
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
