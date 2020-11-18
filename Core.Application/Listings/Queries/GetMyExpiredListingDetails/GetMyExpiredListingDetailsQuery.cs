using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyExpiredListingDetails
{
    public sealed class GetMyExpiredListingDetailsQuery : IGetMyExpiredListingDetailsQuery
    {
        private readonly IListingDataService _dataService;

        public GetMyExpiredListingDetailsQuery(IListingDataService dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public Option<MyExpiredListingDetailsModel> Execute(Guid userId, GetMyExpiredListingDetailsQueryParams queryParams)
        {
            if (queryParams == null)
                return Option<MyExpiredListingDetailsModel>.None;

            return _dataService.FindMyExpired(userId, queryParams);
        }
    }
}
