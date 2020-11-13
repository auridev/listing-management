namespace Core.Application.Listings.Commands.CreateNewListing
{
    public class CreateNewListingModel
    {
        public string Title { get; set; }
        public int MaterialTypeId { get; set; }
        public float Weight { get; set; }
        public string MassUnit { get; set; }
        public string Description { get; set; }
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
        public NewImageModel[] Images { get; set; }
    }
}
