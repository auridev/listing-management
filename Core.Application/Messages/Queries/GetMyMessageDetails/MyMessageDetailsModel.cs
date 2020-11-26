using System;

namespace Core.Application.Messages.Queries.GetMyMessageDetails
{
    public class MyMessageDetailsModel
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool Seen { get; set; }
    }
}
