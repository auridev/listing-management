using Core.Application.Helpers;
using System;
using System.Collections.Generic;

namespace Core.Application.Listings.Queries.GetPublicListings
{
    public sealed class GetPublicListingsQuery : IGetPublicListingsQuery
    {
        private readonly IListingReadOnlyRepository _repository;
        public GetPublicListingsQuery(IListingReadOnlyRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public PagedList<PublicListingModel> Execute(Guid userId, GetPublicListingsQueryParams queryParams)
        {
            if ((queryParams == null) || (userId == default))
                return PagedList<PublicListingModel>.CreateEmpty();

            return _repository.GetPublic(userId, queryParams);
        }
    }
}
