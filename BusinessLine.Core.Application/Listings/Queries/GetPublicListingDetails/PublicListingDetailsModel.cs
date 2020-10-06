using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLine.Core.Application.Listings.Queries.GetPublicListingDetails
{
    public class PublicListingDetailsModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int MaterialTypeId { get; set; }
        public float Weight { get; set; }
        public string MassUnit { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
    }
}
