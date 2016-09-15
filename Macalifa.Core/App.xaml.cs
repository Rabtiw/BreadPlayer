﻿using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
namespace Macalifa
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        CoreWindowLogic logic = new CoreWindowLogic();
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.EnteredBackground += App_EnteredBackground;
        }

        private void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            logic.Stringify();
            deferral.Complete();
            
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;                
            }
#endif
            LoadFrame(e, e.Arguments);           
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            //throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            logic.Stringify();
            await Task.Delay(100);
            deferral.Complete();
        }

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Shell.Play(args.Files[0]);
            }
            else
            {
                LoadFrame(args, args.Files[0]);
            }
        }

        void LoadFrame(IActivatedEventArgs args, object arguments)
        {
            Frame rootFrame = Window.Current.Content as Frame;
           
            // Do not repeat app initialization when the Window already has content
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }               
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(Shell), arguments);
            }

            // Ensure the current window is active
            Window.Current.Activate();
            if (args.Kind != ActivationKind.File)
            {
                logic.Replay();
            }
            else
            {
                logic.Replay(true);
            }
        }
    }
}
