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

        public ICollection<PublicListingModel> Execute(Guid userId, GetPublicListingsQueryParams queryParams)
        {
            if (queryParams == null)
                return new PublicListingModel[0];

            return _repository.GetPublic(userId, queryParams);
        }
    }
}
