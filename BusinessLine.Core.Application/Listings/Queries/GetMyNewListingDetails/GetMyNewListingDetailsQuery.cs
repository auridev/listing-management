using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyNewListingDetails
{
    public sealed class GetMyNewListingDetailsQuery : IGetMyNewListingDetailsQuery
    {
        private readonly IListingDataService _dataService;
        public GetMyNewListingDetailsQuery(IListingDataService dataService)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public Option<MyNewListingDetailsModel> Execute(Guid userId, GetMyNewListingDetailsQueryParams queryParams)
        {
            if (queryParams == null)
                return Option<MyNewListingDetailsModel>.None;

            return _dataService.FindMyNew(userId, queryParams);
        }
    }
}
