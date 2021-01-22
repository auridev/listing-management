using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetPublicListingDetails
{
    public sealed class GetPublicListingDetailsQuery : IGetPublicListingDetailsQuery
    {
        private readonly IListingReadOnlyRepository _repository;

        public GetPublicListingDetailsQuery(IListingReadOnlyRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public Option<PublicListingDetailsModel> Execute(Guid userId, Guid listingId)
        {
            if ((userId == default) || (listingId == default))
                return Option<PublicListingDetailsModel>.None;

            return _repository.FindPublic(userId, listingId);
        }
    }
}
