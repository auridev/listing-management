using Common.ApplicationSettings;
using Common.FileSystem;
using Core.Application.Listings.Commands;
using Core.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace Persistence.Commands.Listings
{
    public class ImagePersistenceService : IImagePersistenceService
    {
        private readonly string _location;
        private readonly IFileSystemService _fileSystemService;
        public ImagePersistenceService(IOptions<ImageRepositorySettings> settings, IFileSystemService fileSystemService)
        {
            _location = settings.Value.Location ??
                throw new ArgumentNullException(nameof(settings));
            _fileSystemService = fileSystemService ??
                throw new ArgumentNullException(nameof(settings));
        }

        public void AddAndSave(Guid parentReference, ICollection<ImageContent> imageContents, DateTag dateTag)
        {
            if (parentReference == default)
                throw new ArgumentNullException(nameof(parentReference));
            if(imageContents == null)
                throw new ArgumentNullException(nameof(imageContents));
            if (dateTag == null)
                throw new ArgumentNullException(nameof(dateTag));


            // make sure the directory structure's there
            string directory = GetFullDirectoryName(parentReference);
            _fileSystemService.EnsureDirectoryExists(directory);

            // save images
            SaveImages(directory, imageContents);
        }

        private string GetFullDirectoryName(Guid parentReference)
            => Path.Combine(this._location, Convert.ToString(parentReference));

        private void SaveImages(string directory, ICollection<ImageContent> imageContents)
        {
            foreach (ImageContent imageContent in imageContents)
            {
                SaveImage(directory, imageContent);
            }
        }

        private void SaveImage(string directory, ImageContent imageContents)
        {
            string fullFileName =
                GetFullFileName(directory, imageContents.FileName);

            _fileSystemService
                .WriteBytesToFile(fullFileName, imageContents.Content);
        }

        private string GetFullFileName(string directory, FileName fileName)
            => Path.Combine(directory, fileName.Value);
    }
}
