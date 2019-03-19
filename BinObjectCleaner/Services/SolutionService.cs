using BinObjectCleaner.Exceptions;
using BinObjectCleaner.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

namespace BinObjectCleaner.Services
{
    public class SolutionService
    {
        private FolderPicker _folderPicker;
        private ApplicationDataContainer _applicationDataContainer;
        private List<string> _ignorable = new List<string>
        {
            ".vs",
            ".git",
            ".idea",
            "views",
            "enums",
            "pages",
            "config",
            "assets",
            "models",
            "helpers",
            "packages",
            "settings",
            "services",
            "constants",
            "renderers",
            "resources",
            "viewmodels",
            "properties",
            "extensions",
        };
        private List<string> _searchable = new List<string>
        {
            "bin",
            "obj",
            "object",
        };


        public SolutionService()
        {
             _folderPicker = new FolderPicker();
             _folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
             _folderPicker.FileTypeFilter.Add(".");

            _applicationDataContainer = ApplicationData.Current.LocalSettings;
        }


        public async Task<IList<StorageFolder>> Load()
        {
            ICollection<string> solutionNames =  _applicationDataContainer.Values.Keys;
            IList<StorageFolder> solutionFolders = new List<StorageFolder>();

            if (solutionNames.Count != 0)
            {
                bool isExceptionOccure = false;
                string token = string.Empty;

                foreach(var solutionName in solutionNames)
                {
                    try
                    {
                        token = (string)_applicationDataContainer.Values[solutionName];
                        StorageFolder solutionFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);

                        if (solutionFolder.Name != solutionName)
                        {
                            _applicationDataContainer.Values.Remove(solutionName);
                            string solutionFolderToken = StorageApplicationPermissions.FutureAccessList.Add(solutionFolder);
                            _applicationDataContainer.Values.Add(solutionFolder.Name, solutionFolderToken);
                        }

                        solutionFolders.Add(solutionFolder);
                    }
                    catch(FileNotFoundException)
                    {
                        isExceptionOccure = true;
                        RemoveFromApplicationStorage(token, solutionName);
                    }
                }

                if (isExceptionOccure)
                {
                    throw new StorageException(Constants.Messages.StoragesRemoved);
                }
            }

            return solutionFolders;
        }

        public async Task<StorageFolder> Pick()
        {
            try
            {
                StorageFolder solutionFolder = await _folderPicker.PickSingleFolderAsync();

                if (solutionFolder != null)
                {
                    string solutionFolderToken = StorageApplicationPermissions.FutureAccessList.Add(solutionFolder);
                    _applicationDataContainer.Values.Add(solutionFolder.Name, solutionFolderToken);

                    return solutionFolder;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }

        public async Task Clear(IList<StorageFolder> solutionFolders)
        {
            if (solutionFolders != null)
            {
                bool isExceptionOccured = false;
                var exceptedSolutions = new List<StorageFolder>();

                foreach (StorageFolder solutionFolder in solutionFolders)
                {
                    try
                    {
                        await Clear(solutionFolder);
                    }
                    catch (StorageException)
                    {
                        isExceptionOccured = true;
                        exceptedSolutions.Add(solutionFolder);
                    }
                }

                if (isExceptionOccured)
                {
                    throw new StorageException(Constants.Messages.StoragesRemovedOrRenames, exceptedSolutions);
                }
            }
        }

        public async Task Clear(StorageFolder solutionFolder)
        {
            await CheckIfExist(solutionFolder.Name, solutionFolder.Path);

            IList<StorageFolder> clearableFolders = await GetSearchableFoldersAsync(solutionFolder);
            if (clearableFolders.Count != 0)
            {
                foreach (var clearableFolder in clearableFolders)
                {
                    var subItems = await clearableFolder.GetItemsAsync();

                    foreach(var subItem in subItems)
                    {
                        await subItem.DeleteAsync();
                    }
                }
            }
        }

        public void Remove(StorageFolder solutionFolder)
        {
            string token = (string)_applicationDataContainer.Values[solutionFolder.Name];
            RemoveFromApplicationStorage(token, solutionFolder.Name);
        }

        private async Task<IList<StorageFolder>> GetSearchableFoldersAsync(StorageFolder storageFolder)
        {
            List<StorageFolder> searchableFolders = new List<StorageFolder>();

            await GetNestedFoldersAsync(storageFolder, searchableFolders);

            return searchableFolders;
        }

        private async Task GetNestedFoldersAsync(StorageFolder folder, List<StorageFolder> searchableFolders)
        {
            IReadOnlyCollection<StorageFolder> subFolders = await folder.GetFoldersAsync();

            foreach (var subFolder in subFolders)
            {
                string subFolderName = subFolder.Name.ToLower();

                if (_searchable.Contains(subFolderName))
                {
                    searchableFolders.Add(subFolder);
                    continue;
                }
                else if (_ignorable.Contains(subFolderName))
                {
                    continue;
                }

                await GetNestedFoldersAsync(subFolder, searchableFolders);
            }
        }

        private async Task CheckIfExist(string name, string path)
        {
            try
            {
                var checkIfExist = await StorageFolder.GetFolderFromPathAsync(path);
            }
            catch (FileNotFoundException)
            {
                string token = (string)_applicationDataContainer.Values[name];
                RemoveFromApplicationStorage(token, name);

                throw new StorageException(Constants.Messages.StorageRemovedOrRenames);
            }
        }

        private void RemoveFromApplicationStorage(string token, string solutionName)
        {
            _applicationDataContainer.Values.Remove(solutionName);
            StorageApplicationPermissions.FutureAccessList.Remove(token);
        }
    }
}
