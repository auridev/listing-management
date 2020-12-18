using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyNewListingDetails
{
    public sealed class GetMyNewListingDetailsQuery : IGetMyNewListingDetailsQuery
    {
        private readonly IListingReadOnlyRepository _repository;
        public GetMyNewListingDetailsQuery(IListingReadOnlyRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public Option<MyNewListingDetailsModel> Execute(Guid userId, Guid listingId)
        {
            return _repository.FindMyNew(userId, listingId);
        }
    }
}
