using Core.Application.Listings.Commands.AcceptOffer;
using Core.Application.Listings.Commands.ActivateNewListing;
using Core.Application.Listings.Commands.ActivateSuspiciousListing;
using Core.Application.Listings.Commands.AddFavorite;
using Core.Application.Listings.Commands.AddLead;
using Core.Application.Listings.Commands.CreateNewListing;
using Core.Application.Listings.Commands.CreateNewListing.Factory;
using Core.Application.Listings.Commands.DeactivateActiveListing;
using Core.Application.Listings.Commands.DeactivateNewListing;
using Core.Application.Listings.Commands.DeactivateSuspiciousListing;
using Core.Application.Listings.Commands.MarkNewListingAsSuspicious;
using Core.Application.Listings.Commands.MarkOfferAsSeen;
using Core.Application.Listings.Commands.ReactivatePassiveListing;
using Core.Application.Listings.Commands.ReceiveOffer;
using Core.Application.Listings.Commands.ReceiveOffer.Factory;
using Core.Application.Listings.Commands.RemoveFavorite;
using Core.Application.Listings.Queries.GetMyActiveListingDetails;
using Core.Application.Listings.Queries.GetMyClosedListingDetails;
using Core.Application.Listings.Queries.GetMyExpiredListingDetails;
using Core.Application.Listings.Queries.GetMyListings;
using Core.Application.Listings.Queries.GetMyNewListingDetails;
using Core.Application.Listings.Queries.GetMyPassiveListingDetails;
using Core.Application.Listings.Queries.GetPublicListingDetails;
using Core.Application.Listings.Queries.GetPublicListings;
using Core.Application.Messages.Queries.GetMyMessageDetails;
using Core.Application.Messages.Queries.GetMyMessages;
using Core.Application.Profiles.Commands.CreateProfile;
using Core.Application.Profiles.Commands.CreateProfile.Factory;
using Core.Application.Profiles.Commands.MarkProfileAsIntroduced;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateProfileCommand, CreateProfileCommand>();
            services.AddScoped<IMarkProfileAsIntroducedCommand, MarkProfileAsIntroducedCommand>();

            services.AddScoped<IProfileFactory, ProfileFactory>();
            services.AddScoped<IGetMyMessageDetailsQuery, GetMyMessageDetailsQuery>();
            services.AddScoped<IGetMyMessagesQuery, GetMyMessagesQuery>();


            // Listings
            services.AddScoped<IAcceptOfferCommand, AcceptOfferCommand>();
            services.AddScoped<IActivateNewListingCommand, ActivateNewListingCommand>();
            services.AddScoped<IActivateSuspiciousListingCommand, ActivateSuspiciousListingCommand>();
            services.AddScoped<IAddFavoriteCommand, AddFavoriteCommand>();
            services.AddScoped<IAddLeadCommand, AddLeadCommand>();
            services.AddScoped<ICreateNewListingCommand, CreateNewListingCommand>();
            services.AddScoped<IDeactivateActiveListingCommand, DeactivateActiveListingCommand>();
            services.AddScoped<IDeactivateNewListingCommand, DeactivateNewListingCommand>();
            services.AddScoped<IDeactivateSuspiciousListingCommand, DeactivateSuspiciousListingCommand>();
            services.AddScoped<IMarkNewListingAsSuspiciousCommand, MarkNewListingAsSuspiciousCommand>();
            services.AddScoped<IMarkOfferAsSeenCommand, MarkOfferAsSeenCommand>();
            services.AddScoped<IReactivatePassiveListingCommand, ReactivatePassiveListingCommand>();
            services.AddScoped<IReceiveOfferCommand, ReceiveOfferCommand>();
            services.AddScoped<IRemoveFavoriteCommand, RemoveFavoriteCommand>();
            
            services.AddScoped<IGetMyActiveListingDetailsQuery, GetMyActiveListingDetailsQuery>();
            services.AddScoped<IGetMyClosedListingDetailsQuery, GetMyClosedListingDetailsQuery>();
            services.AddScoped<IGetMyExpiredListingDetailsQuery, GetMyExpiredListingDetailsQuery>();
            services.AddScoped<IGetMyListingsQuery, GetMyListingsQuery>();
            services.AddScoped<IGetMyNewListingDetailsQuery, GetMyNewListingDetailsQuery>();
            services.AddScoped<IGetMyPassiveListingDetailsQuery, GetMyPassiveListingDetailsQuery>();
            services.AddScoped<IGetPublicListingDetailsQuery, GetPublicListingDetailsQuery>();
            services.AddScoped<IGetPublicListingsQuery, GetPublicListingsQuery>();

            services.AddScoped<INewListingFactory, NewListingFactory>();
            services.AddScoped<IOfferFactory, OfferFactory>();

            return services;
        }
    }
}
