using System;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Unvired.Common.WinRT.Interface;
using Unvired.Kernel.UWP.Login;
using UNVIRED.DB.SAMPLE.Views;
using Utils;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace UNVIRED.DB.SAMPLE
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : ILoginListener
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            this.UnhandledException += App_UnhandledException;
        }

        private void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            Window.Current.Content = rootFrame;
            Window.Current.Activate();

            LoginParameters.AssemblyName = GetType().GetTypeInfo().Assembly.FullName;
            LoginParameters.AppTitle = "Unvired DB Sample";
            LoginParameters.AppName = AppConstants.APP_NAME;
            LoginParameters.Company = "unvired";
            LoginParameters.ShowCompany = true;
            LoginParameters.Protocol = LoginParameters.Protocols.https;
            LoginParameters.Url = "sandbox.unvired.io/UMP?local";
            LoginParameters.AssemblyVersion = GetType().GetTypeInfo().Assembly.GetName().Version.ToString(4);
            LoginParameters.ShowBrandLogo = true;



            using (var xr = XmlReader.Create(@"metadata.xml"))
            {
                var xDocument = XDocument.Load(xr);
                LoginParameters.MetaDataXml = xDocument;
            }
            LoginParameters.LoginListener = this;

            await AuthenticationService.Login();
        }



        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        public void LoginSuccessful()
        {
            //Check for Mobile User in local db, if does not exist the navigate to SAPLoginPage so that user can authenticate with SAP and
            // download Mobile User along with other customization data.
            // Mobile User has SAP username which is required to fetch data from SAP for that user

            if (Window.Current.Content is Frame frame && !frame.Navigate(typeof(RootPage), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight }))
            {
                throw new Exception("Failed to create initial page");
            }
        }

        public void LoginCancelled()
        {
        }

        public void LoginFailure(string errorMessage)
        {
        }

        public void AuthenticateAndActivationSuccessful()
        {
            if (Window.Current.Content is Frame frame && !frame.Navigate(typeof(RootPage), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight }))
            {
                throw new Exception("Failed to create initial page");
            }
        }

        public void AuthenticateAndActivationFailure(string errorMessage)
        {
        }

        public void DemoLoginSuccessful()
        {
        }

        public void ActivationRequired()
        {
        }

        public void LocalAuthenticationRequired(string userName)
        {
        }
    }
}