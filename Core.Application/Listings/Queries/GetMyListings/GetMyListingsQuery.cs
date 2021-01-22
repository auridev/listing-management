using Core.Application.Helpers;
using System;

namespace Core.Application.Listings.Queries.GetMyListings
{
    public sealed class GetMyListingsQuery : IGetMyListingsQuery
    {
        private readonly IListingReadOnlyRepository _repository;
        public GetMyListingsQuery(IListingReadOnlyRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public PagedList<MyListingModel> Execute(Guid userId, GetMyListingsQueryParams queryParams)
        {
            if ((queryParams == null) || (userId == default))
                return PagedList<MyListingModel>.CreateEmpty();

            return _repository.GetMy(userId, queryParams);
        }
    }
}
