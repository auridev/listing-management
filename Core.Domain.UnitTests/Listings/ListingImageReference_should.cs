using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using FluentAssertions;
using System;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Domain.UnitTests.Listings
{
    public class ListingImageReference_should
    {
        private readonly FileName fileName = FileName.Create("name.ext")
            .Right(fileName => fileName)
            .Left(_ => throw InvalidExecutionPath.Exception);

        private readonly FileSize fileSize = FileSize.Create(34_000_000L)
            .Right(fileSize => fileSize)
            .Left(_ => throw InvalidExecutionPath.Exception);

        private readonly ListingImageReference _sut;


        public ListingImageReference_should()
        {
            _sut = new ListingImageReference(Guid.NewGuid(), Guid.NewGuid(), fileName, fileSize);
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
            _sut.FileSize.Should().Be(fileSize);
        }
    }
}
