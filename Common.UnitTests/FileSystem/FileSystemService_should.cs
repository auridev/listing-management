using Common.FileSystem;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit;

namespace Common.UnitTests.FileSystem
{
    public class FileSystemService_should
    {
        private readonly FileSystemService _sut;

        public FileSystemService_should()
        {
            _sut = new FileSystemService();
        }

        [Fact]
        public void have_DirectoryExists_method()
        {
            // Act
            bool exists = _sut.DirectoryExists("aaa");

            // Assert
            exists.Should().BeFalse();
        }

        [Fact]
        public void return_false_if_specified_directory_does_not_exist()
        {
            // Arrange
            string directory = GetFullDirectoryPath("dir_1");
            DeleteDirectoryIfExists(directory);

            // Act
            bool exists = _sut.DirectoryExists(directory);

            // Assert
            exists.Should().BeFalse();
        }

        [Fact]
        public void return_true_if_specified_directory_exists()
        {
            // Arrange
            string directory = GetFullDirectoryPath("dir_2");
            DeleteDirectoryIfExists(directory);
            CreateDirectory(directory);
            
            // Act
            bool exists = _sut.DirectoryExists(directory);

            // Assert
            exists.Should().BeTrue();
        }

        [Fact]
        public void throw_exception_if_input_for_DirectoryExists_is_not_valid()
        {
            Action action = () => _sut.DirectoryExists(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void create_directory_if_it_does_not_exist()
        {
            // Arrange
            string directory = GetFullDirectoryPath("dir_3");
            DeleteDirectoryIfExists(directory);

            // Act
            _sut.EnsureDirectoryExists(directory);

            // Assert
            bool exists = _sut.DirectoryExists(directory);
            exists.Should().BeTrue();
        }

        [Fact]
        public void throw_exception_if_input_for_EnsureDirectoryExists_is_not_valid()
        {
            Action action = () => _sut.EnsureDirectoryExists(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void write_bytes_to_file()
        {
            // Arrange
            string directory = GetFullDirectoryPath("file_dir_1");
            DeleteDirectoryIfExists(directory);
            CreateDirectory(directory);
            var bytes = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 };
            var fileName = Path.Combine(directory, "my_file");

            // Act
            _sut.WriteBytesToFile(fileName, bytes);

            // Assert
            bool fileExists = FileExists(fileName);

            fileExists.Should().BeTrue();
        }

        [Fact]
        public void write_same_bytes_to_file()
        {
            // Arrange
            string directory = GetFullDirectoryPath("file_dir_2");
            DeleteDirectoryIfExists(directory);
            CreateDirectory(directory);
            var bytes = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 };
            var fileName = Path.Combine(directory, "my_file");

            // Act
            _sut.WriteBytesToFile(fileName, bytes);

            // Assert
            byte[] writenBytes = ReadFile(fileName);
            writenBytes.Should().BeEquivalentTo(bytes);
        }

        [Fact]
        public void create_parent_directory_of_the_file_if_it_does_not_exist()
        {
            // Arrange
            string directory = GetFullDirectoryPath("file_dir_3");
            DeleteDirectoryIfExists(directory);
            var bytes = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50 };
            var fileName = Path.Combine(directory, "my_file_3");

            // Act
            _sut.WriteBytesToFile(fileName, bytes);

            // Assert
            bool parentDirectoryExists = _sut.DirectoryExists(directory);
            parentDirectoryExists.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(InvalidInput))]
        public void throw_exception_if_input_for_WriteBytesToFile_is_not_valid(string fileName, byte[] bytes)
        {
            Action action = () => _sut.WriteBytesToFile(fileName, bytes);

            action.Should().Throw<ArgumentNullException>();
        }

        public static IEnumerable<object[]> InvalidInput => new List<object[]>
        {
            new object[] { null, new byte[] { } },
            new object[] { "adads", null }
        };

        // helpers
        private string GetFullDirectoryPath(string name)
        {
            return Path.Combine(GetExecutingDirectory().FullName, name);
        }

        private void DeleteDirectoryIfExists(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

        }

        private DirectoryInfo GetExecutingDirectory()
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return new FileInfo(location.AbsolutePath).Directory;
        }

        private bool FileExists(string path)
        {
            return File.Exists(path);
        }

        private byte[] ReadFile(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}
