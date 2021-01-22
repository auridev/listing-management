using Common.ApplicationSettings;
using Common.FileSystem;
using Common.Helpers;
using Core.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Persistence.Commands.Listings.UnitTests
{
    public class ImagePersistenceService_should
    {
        private readonly AutoMocker _mocker;
        private readonly ImagePersistenceService _sut;

        private static Guid _parentReference = Guid.NewGuid();
        private static ICollection<ImageContent> _imageContents = new List<ImageContent>()
        {
            (ImageContent)ImageContent.Create("a1.a", new byte[] { 0x10 }),
            (ImageContent)ImageContent.Create("b2.b", new byte[] { 0x20 }),
            (ImageContent)ImageContent.Create("c3.c", new byte[] { 0x30 })
        };
        private static DateTag _dateTag = (DateTag)DateTag.Create(DateTimeOffset.UtcNow);

        public ImagePersistenceService_should()
        {
            _mocker = new AutoMocker();

            IOptions<ImageRepositorySettings> settings =
                Options.Create(new ImageRepositorySettings() { Location = "aaa" });
            _mocker.Use(settings);

            _mocker
                .GetMock<IFileSystemService>();

            _sut = _mocker.CreateInstance<ImagePersistenceService>();
        }

        [Fact]
        public void ensure_correct_parent_directory_exists()
        {
            string expectedParentDirectory = Path.Combine("aaa", Convert.ToString(_parentReference));

            _sut.AddAndSave(_parentReference, _imageContents, _dateTag);

            _mocker
                .GetMock<IFileSystemService>()
                .Verify(s => s.EnsureDirectoryExists(expectedParentDirectory), Times.Once);
        }

        [Fact]
        public void save_all_images_to_parent_directory()
        {
            // Arrange
            string expectedParentDirectory = Path.Combine("aaa", Convert.ToString(_parentReference));
            string firstFileFullPath = Path.Combine(expectedParentDirectory, "a1.a");
            string secondFileFullPath = Path.Combine(expectedParentDirectory, "b2.b");
            string thirdFileFullPath = Path.Combine(expectedParentDirectory, "c3.c");

            // Act
            _sut.AddAndSave(_parentReference, _imageContents, _dateTag);

            // Assert
            _mocker
                .GetMock<IFileSystemService>()
                .Verify(s => s.WriteBytesToFile(firstFileFullPath, new byte[] { 0x10 }), Times.Once);

            _mocker
                .GetMock<IFileSystemService>()
                .Verify(s => s.WriteBytesToFile(secondFileFullPath, new byte[] { 0x20 }), Times.Once);

            _mocker
                .GetMock<IFileSystemService>()
                .Verify(s => s.WriteBytesToFile(thirdFileFullPath, new byte[] { 0x30 }), Times.Once);
        }

        public static IEnumerable<object[]> InvalidArguments => new List<object[]>
        {
            new object[] { _parentReference, _imageContents, null },
            new object[] { _parentReference, null, _dateTag },
            new object[] { default, _imageContents, _dateTag }
        };

        [Theory]
        [MemberData(nameof(InvalidArguments))]
        public void thrown_an_exception_if_arguments_are_not_valid(Guid parentReference, ICollection<ImageContent> imageContents, DateTag dateTag)
        {
            Action createAction = () => _sut.AddAndSave(parentReference, imageContents, dateTag);

            createAction.Should().Throw<ArgumentNullException>();
        }
    }
}
