using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyClosedListingDetails
{
    public sealed class GetMyClosedListingDetailsQuery : IGetMyClosedListingDetailsQuery
    {
        private readonly IListingReadOnlyRepository _repository;

        public GetMyClosedListingDetailsQuery(IListingReadOnlyRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public Option<MyClosedListingDetailsModel> Execute(Guid userId, Guid listingId)
        {
            return _repository.FindMyClosed(userId, listingId);
        }
    }
}
