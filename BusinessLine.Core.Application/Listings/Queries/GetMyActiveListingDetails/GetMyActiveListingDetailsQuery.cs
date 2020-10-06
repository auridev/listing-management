using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetMyActiveListingDetails
{
    public sealed class GetMyActiveListingDetailsQuery : IGetMyActiveListingDetailsQuery
    {
        private readonly IListingDataService _dataService;

        public GetMyActiveListingDetailsQuery(IListingDataService dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public Option<MyActiveListingDetailsModel> Execute(Guid userId, GetMyActiveListingDetailsQueryParams queryParams)
        {
            if (queryParams == null)
                return Option<MyActiveListingDetailsModel>.None;

            return _dataService.FindMyActive(userId, queryParams);
        }
    }
}
