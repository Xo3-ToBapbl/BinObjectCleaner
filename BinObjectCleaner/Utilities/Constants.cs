using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinObjectCleaner.Utilities
{
    public static class Constants
    {
        public static string AppName = "Bin-Object Cleaner";

        public static class KeyNames
        {
            public static string Solution = "Solution";
            public static string ClearableFolders = "ClearableFolders";
        }

        public static class Messages
        {
            public static string StoragesRemoved = "One or more saved storage folders were deleted from disk. Storages folders were removed from application";
            public static string StorageRemovedOrRenames = "Storage folder was removed or renamed";
            public static string StoragesRemovedOrRenames = "One or more storage folders were removed or renamed";
            public static string StoragesSuccessfullyCleared = "One or more solutions successfully cleaned";
            public static string CompleteWithErrors = "Solutions cleaning complete with error: one or more storage folders were removed or renamed";
        }
    }
}
