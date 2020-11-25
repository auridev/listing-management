namespace Common.FileSystem
{
    public interface IFileSystemService
    {
        bool DirectoryExists(string directory);

        void EnsureDirectoryExists(string directory);

        void WriteBytesToFile(string fileName, byte[] bytes);
    }
}
