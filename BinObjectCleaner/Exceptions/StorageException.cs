using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BinObjectCleaner.Exceptions
{
    public class StorageException : Exception
    {
        public IList<StorageFolder> ExceptedFolders { get; private set; }

        public StorageException() : base() { }
        public StorageException(string message) : base(message) { }
        public StorageException(string message, IList<StorageFolder> exceptedFolders) : base(message)
        {
            ExceptedFolders = exceptedFolders;
        }
    }
}
