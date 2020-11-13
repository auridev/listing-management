using Core.Domain.Common;
using Core.Domain.Listings;
using FluentAssertions;
using System;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class ListingImageReference_should
    {

        private readonly ListingImageReference _sut;
        public ListingImageReference_should()
        {
            _sut = new ListingImageReference(
                Guid.NewGuid(),
                Guid.NewGuid(),
                FileName.Create("name.ext"),
                FileSize.Create(34_000_000L));
        }

        [Fact]
        public void have_Id_property()
        {
            _sut.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void have_ParentReference_property()
        {
            _sut.ParentReference.Should().NotBeEmpty();
        }

        [Fact]
        public void have_FileName_property()
        {
            _sut.FileName.Value.Should().Be("name.ext");
        }

        [Fact]
        public void have_FileSize_property()
        {
            _sut.FileSize.Should().Be(FileSize.Create(34_000_000L));
        }

    }
}
