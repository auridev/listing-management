using System;
using System.Collections.Generic;

namespace Core.Application.Listings.Queries.GetPublicListings
{
    public sealed class GetPublicListingsQuery : IGetPublicListingsQuery
    {
        private readonly IListingDataService _dataService;
        public GetPublicListingsQuery(IListingDataService dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public ICollection<PublicListingModel> Execute(Guid userId, GetPublicListingsQueryParams queryParams)
        {
            if (queryParams == null)
                return new PublicListingModel[0];

            return _dataService.GetPublic(userId, queryParams);
        }
    }
}
