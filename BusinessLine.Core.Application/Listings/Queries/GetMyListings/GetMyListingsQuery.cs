using System;
using System.Collections.Generic;

namespace BusinessLine.Core.Application.Listings.Queries.GetMyListings
{
    public sealed class GetMyListingsQuery : IGetMyListingsQuery
    {
        private readonly IListingDataService _dataService;
        public GetMyListingsQuery(IListingDataService dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public ICollection<MyListingModel> Execute(Guid userId, GetMyListingsQueryParams queryParams)
        {
            if (queryParams == null)
                return new MyListingModel[0];

            return _dataService.GetMy(userId, queryParams);
        }
    }
}
