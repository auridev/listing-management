﻿namespace Core.Application.Profiles.Commands.CreateProfile
{
    public sealed class CreateProfileModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DistanceUnit { get; set; }
        public string MassUnit { get; set; }
        public string CurrencyCode { get; set; }
    }
}
