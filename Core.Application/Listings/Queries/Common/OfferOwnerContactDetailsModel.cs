using System;

namespace Core.Application.Listings.Queries.Common
{
    public class OfferOwnerContactDetailsModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
