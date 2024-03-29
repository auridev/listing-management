﻿using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyExpiredListingDetails
{
    public sealed class GetMyExpiredListingDetailsQuery : IGetMyExpiredListingDetailsQuery
    {
        private readonly IListingReadOnlyRepository _repository;

        public GetMyExpiredListingDetailsQuery(IListingReadOnlyRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public Option<MyExpiredListingDetailsModel> Execute(Guid userId, Guid listingId)
        {
            if ((userId == default) || (listingId == default))
                return Option<MyExpiredListingDetailsModel>.None;

            return _repository.FindMyExpired(userId, listingId);
        }
    }
}
