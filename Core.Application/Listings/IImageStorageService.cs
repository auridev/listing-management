using Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace Core.Application.Listings
{
    public interface IImageStorageService
    {
        void Save(Guid parentReference, ICollection<ImageContent> imageContents, DateTag dateTag);

        ICollection<ImageContent> Get(Guid parentReference);
    }
}
