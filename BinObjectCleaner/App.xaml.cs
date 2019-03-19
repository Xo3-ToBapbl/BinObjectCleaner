using BinObjectCleaner.Pages;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace BinObjectCleaner
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }


        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }

                ApplicationView.PreferredLaunchViewSize = new Size(640, 360);
                ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

                Window.Current.Activate();

                SetTitleBar();
            }
        }

        private void SetTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            var appDeepGrayColor = ((SolidColorBrush)App.Current.Resources["AppDeepGray"]).Color;
            var appDeepBlackColor = ((SolidColorBrush)App.Current.Resources["AppDeepBlack"]).Color;

            titleBar.BackgroundColor = appDeepGrayColor;
            titleBar.InactiveBackgroundColor = appDeepGrayColor;
            titleBar.ButtonBackgroundColor = appDeepGrayColor;
            titleBar.ButtonInactiveBackgroundColor = appDeepGrayColor;
            titleBar.ButtonHoverBackgroundColor = appDeepBlackColor;
            titleBar.ButtonHoverForegroundColor = Colors.White;
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            deferral.Complete();
        }
    }
}
