using Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace Core.Application.Listings.Queries
{
    public interface IImageDataService
    {
        ICollection<ImageContent> Get(Guid parentReference);
    }
}
