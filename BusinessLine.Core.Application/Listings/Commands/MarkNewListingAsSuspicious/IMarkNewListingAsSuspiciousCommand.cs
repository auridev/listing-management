namespace Core.Application.Listings.Commands.MarkNewListingAsSuspicious
{
    public interface IMarkNewListingAsSuspiciousCommand
    {
        void Execute(MarkNewListingAsSuspiciousModel model);
    }
}