using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask
{
    public interface IDirectoryWrapper
    {
        IEnumerable<string> EnumerateFileSystemEntries(string path);
    }

    public class DirectoryWrapper : IDirectoryWrapper
    {
        public IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            return Directory.EnumerateFileSystemEntries(path);
        }
    }


}
