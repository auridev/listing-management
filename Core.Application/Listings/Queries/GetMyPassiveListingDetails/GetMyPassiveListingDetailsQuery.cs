using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyPassiveListingDetails
{
    public sealed class GetMyPassiveListingDetailsQuery : IGetMyPassiveListingDetailsQuery
    {
        private readonly IListingDataService _dataService;

        public GetMyPassiveListingDetailsQuery(IListingDataService dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public Option<MyPassiveListingDetailsModel> Execute(Guid userId, GetMyPassiveListingDetailsQueryParams queryParams)
        {
            if (queryParams == null)
                return Option<MyPassiveListingDetailsModel>.None;

            return _dataService.FindMyPassive(userId, queryParams);
        }
    }
}
