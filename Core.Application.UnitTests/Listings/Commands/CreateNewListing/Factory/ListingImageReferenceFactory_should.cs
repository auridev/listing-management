using Common.Helpers;
using Core.Application.Listings.Commands.CreateNewListing.Factory;
using Core.Domain.Listings;
using Core.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt;
using System;
using System.Collections.Generic;
using Test.Helpers;
using Xunit;

namespace BusinessLine.Core.Application.UnitTests.Listings.Commands.CreateNewListing.Factory
{
    public class ListingImageReferenceFactory_should
    {
        private static FileName _fileName = FileName.Create("first.bmp")
                .Right(value => value)
                .Left(_ => throw InvalidExecutionPath.Exception);
        private static FileSize _fileSize = FileSize.Create(2_000L)
                .Right(value => value)
                .Left(_ => throw InvalidExecutionPath.Exception);
        private ListingImageReferenceFactory _sut = new ListingImageReferenceFactory();

        [Fact]
        public void return_EitherRight_with_ListingImageReference_on_success()
        {
            // act
            Either<Error, ListingImageReference> eitherReference = _sut.Create(
                Guid.NewGuid(),
                _fileName,
                _fileSize);

            //assert
            eitherReference
                .Right(reference => reference.Should().NotBeNull())
                .Left(_ => throw InvalidExecutionPath.Exception);
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { default, _fileName, _fileSize, "parentReference" },
            new object[] { Guid.NewGuid(), null, _fileSize, "fileName" },
            new object[] { Guid.NewGuid(), _fileName, null, "fileSize" },
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void return_EitherLeft_with_proper_error_when_arguments_are_invalid(Guid parentReference, FileName fileName, FileSize fileSize, string errorMessage)
        {
            // act
            Either<Error, ListingImageReference> eitherReference = _sut.Create(
                parentReference,
                fileName,
                fileSize);

            //assert
            eitherReference
                .Right(_ => throw InvalidExecutionPath.Exception)
                .Left(error => 
                {
                    error.Should().BeOfType<Error.Invalid>();
                    error.Message.Should().Be(errorMessage);
                });
        }
    }
}
