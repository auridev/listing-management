using Core.Application.Listings.Commands.CreateNewListing.Factory;
using Core.Domain.Common;
using Core.Domain.Listings;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.CreateNewListing.Factory
{
    public class ListingImageReferenceFactory_should
    {
        [Fact]
        public void create_ListingImageReferences()
        {
            // arrange
            var parentReference = Guid.NewGuid();
            var fileName = FileName.Create("first.bmp");
            var fileSize = FileSize.Create(2_000L);
            var sut = new ListingImageReferenceFactory();

            // act
            ListingImageReference reference = sut.Create(
                parentReference,
                fileName,
                fileSize);

            //assert
            reference.Should().NotBeNull();
        }
    }
}
