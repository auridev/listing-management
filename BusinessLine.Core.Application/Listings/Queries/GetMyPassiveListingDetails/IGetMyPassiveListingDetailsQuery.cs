﻿using LanguageExt;
using System;

namespace BusinessLine.Core.Application.Listings.Queries.GetMyPassiveListingDetails
{
    public interface IGetMyPassiveListingDetailsQuery
    {
        Option<MyPassiveListingDetailsModel> Execute(Guid userId, GetMyPassiveListingDetailsQueryParams queryParams);
    }
}