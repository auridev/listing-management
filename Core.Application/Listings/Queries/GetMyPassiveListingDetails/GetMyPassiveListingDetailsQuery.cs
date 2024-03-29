﻿using LanguageExt;
using System;

namespace Core.Application.Listings.Queries.GetMyPassiveListingDetails
{
    public sealed class GetMyPassiveListingDetailsQuery : IGetMyPassiveListingDetailsQuery
    {
        private readonly IListingReadOnlyRepository _repository;

        public GetMyPassiveListingDetailsQuery(IListingReadOnlyRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));
        }

        public Option<MyPassiveListingDetailsModel> Execute(Guid userId, Guid listingId)
        {
            if ((userId == default) || (listingId == default))
                return Option<MyPassiveListingDetailsModel>.None;

            return _repository.FindMyPassive(userId, listingId);
        }
    }
}
