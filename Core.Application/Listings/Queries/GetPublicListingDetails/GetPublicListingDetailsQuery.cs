using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetPublicListingDetails
{
    public sealed class GetPublicListingDetailsQuery : IGetPublicListingDetailsQuery
    {
        private readonly IListingDataService _dataService;

        public GetPublicListingDetailsQuery(IListingDataService dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public Option<PublicListingDetailsModel> Execute(Guid userId, GetPublicListingDetailsQueryParams queryParams)
        {
            if (queryParams == null)
                return Option<PublicListingDetailsModel>.None;

            return _dataService.FindPublic(userId, queryParams);
        }
    }
}
