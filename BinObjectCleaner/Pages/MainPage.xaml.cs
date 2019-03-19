using BinObjectCleaner.Utilities;
using BinObjectCleaner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BinObjectCleaner.Pages
{
    public sealed partial class MainPage : Page
    {
        private readonly MainViewModel viewModel;


        public MainPage()
        {
            this.InitializeComponent();

            viewModel = this.DataContext as MainViewModel;
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await viewModel.InitializeAsync();
        }

        private void ToggleSwitchToggled(object sender, RoutedEventArgs e)
        {
            if (selectionModeSwitch.IsOn)
            {
                headerTransition.Begin();
                solutionsListView.SelectionMode = ListViewSelectionMode.Multiple;
            }
            else
            {
                headerTransitionNegative.Begin();
                solutionsListView.SelectionMode = ListViewSelectionMode.Single;
            }
        }
    }
}
