using System;
using System.Collections.Generic;

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

        public ICollection<MyListingModel> Execute(Guid userId, GetMyListingsQueryParams queryParams)
        {
            if (queryParams == null)
                return new MyListingModel[0];

            return _repository.GetMy(userId, queryParams);
        }
    }
}
