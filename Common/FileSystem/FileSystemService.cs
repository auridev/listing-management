using System;
using System.IO;

namespace Common.FileSystem
{
    public class FileSystemService : IFileSystemService
    {
        public bool DirectoryExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            return Directory.Exists(path);
        }

        public void EnsureDirectoryExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            Directory.CreateDirectory(path);
        }

        public void WriteBytesToFile(string fileName, byte[] bytes)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            // Create the parent directory if there's none
            var fileInfo = new FileInfo(fileName);
            EnsureDirectoryExists(fileInfo.Directory.FullName);

            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Write, 4096))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
