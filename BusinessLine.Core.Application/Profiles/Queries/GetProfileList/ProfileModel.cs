using System;

namespace BusinessLine.Core.Application.Profiles.Queries.GetProfileList
{
    public class ProfileModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
    }
}
