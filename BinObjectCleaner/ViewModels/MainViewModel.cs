using BinObjectCleaner.Exceptions;
using BinObjectCleaner.Services;
using BinObjectCleaner.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace BinObjectCleaner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly SolutionService _solutionService;
        private readonly NotificationService _notificationService;

        public RangeObservableCollection<StorageFolder> SolutionFolders { get; private set; }

        public ICommand AddSolutionCommand { get; }
        public ICommand CleanSolutionsCommand { get; }
        public ICommand RemoveSolutionsCommand { get; }
        public ICommand ExitCommand { get; }


        public MainViewModel()
        {
            _solutionService = new SolutionService();
            _notificationService = new NotificationService();

            SolutionFolders = new RangeObservableCollection<StorageFolder>();

            AddSolutionCommand = new DelegateCommand(AddSolution);
            CleanSolutionsCommand = new DelegateCommand<IList<object>>(CleanSolutions);
            RemoveSolutionsCommand = new DelegateCommand<IList<object>>(RemoveSolutions);
            ExitCommand = new DelegateCommand(ExitFromApplication);
        }


        public async Task InitializeAsync()
        {
            try
            {
                IList<StorageFolder> solutionFolders = await _solutionService.Load();
                SolutionFolders.AddRange(solutionFolders);
            }
            catch (StorageException e)
            {
                _notificationService.SendToast(e.Message);
            }
        }

        private async void AddSolution()
        {
            StorageFolder solutionFolder = await _solutionService.Pick();

            if (solutionFolder != null)
            {
                SolutionFolders.Add(solutionFolder);
            }
        }

        public void RemoveSolutions(IList<object> solutionFolders)
        {
            foreach(StorageFolder solutionFolder in solutionFolders)
            {
                _solutionService.Remove(solutionFolder);
            }

            SolutionFolders.RemoveRange(solutionFolders.Cast<StorageFolder>());
        }

        public async void CleanSolutions(IList<object> solutionFolders)
        {
            if (solutionFolders.Count == 0) return;

            try
            {
                IsBusy = true;

                await _solutionService.Clear(solutionFolders.Cast<StorageFolder>().ToList());
                _notificationService.SendToast(Constants.Messages.StoragesSuccessfullyCleared);
            }
            catch (StorageException e)
            {
                _notificationService.SendToast(Constants.Messages.CompleteWithErrors);

                if (e.ExceptedFolders != null)
                {
                    foreach(var exceptedFolder in e.ExceptedFolders)
                    {
                        SolutionFolders.Remove(exceptedFolder);
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ExitFromApplication()
        {
            App.Current.Exit();
        }
    }
}