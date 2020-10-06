using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetMyClosedListingDetails
{
    public sealed class GetMyClosedListingDetailsQuery : IGetMyClosedListingDetailsQuery
    {
        private readonly IListingDataService _dataService;

        public GetMyClosedListingDetailsQuery(IListingDataService dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public Option<MyClosedListingDetailsModel> Execute(Guid userId, GetMyClosedListingDetailsQueryParams queryParams)
        {
            if (queryParams == null)
                return Option<MyClosedListingDetailsModel>.None;

            return _dataService.FindMyClosed(userId, queryParams);
        }
    }
}
