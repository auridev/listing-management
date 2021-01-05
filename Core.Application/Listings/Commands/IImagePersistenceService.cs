using Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Core.Application.Listings.Commands
{
    public interface IImagePersistenceService
    {
        void AddAndSave(Guid parentReference, ICollection<ImageContent> imageContents, DateTag dateTag);
    }
}
