﻿using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyActiveListingDetails
{
    public sealed class GetMyActiveListingDetailsQuery : IGetMyActiveListingDetailsQuery
    {
        private readonly IListingReadOnlyRepository _repository;

        public GetMyActiveListingDetailsQuery(IListingReadOnlyRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public Option<MyActiveListingDetailsModel> Execute(Guid userId, Guid listingId)
        {
            return _repository.FindMyActive(userId, listingId);
        }
    }
}
