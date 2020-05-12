using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unvired.Kernel.UWP.Log;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UNVIRED.DB.SAMPLE.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RootPage : Page
    {
        public RootPage()
        {
            this.InitializeComponent();
        }
        private void rootNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            try
            {
                if (args.IsSettingsInvoked)
                {
                    contentFrame.Navigate(typeof(SettingPage));
                }

                var ItemContent = args.InvokedItemContainer.Tag;
                if (ItemContent != null)
                {
                    switch (ItemContent)
                    {
                        case "Common_Home_Page":
                            contentFrame.Navigate(typeof(HomePage));
                            break;
                        case "AddContact_Page":
                            contentFrame.Navigate(typeof(AddContact));

                            break;


                    }
                }
            }
            catch (Exception ex)
            {
                Logger.E($"Exception caught while navigation Item clicked {ex.Message}");
            }


        }

        private void rootNavigationView_Loaded(object sender, RoutedEventArgs e)

        {
            try
            {
                foreach (NavigationViewItemBase item in rootNavigationView.MenuItems)
                {
                    if (item is NavigationViewItem && item.Tag.ToString() == "Common_Home_Page")
                    {
                        rootNavigationView.SelectedItem = item;
                        break;
                    }
                }
                contentFrame.Navigated += On_Navigated;
                contentFrame.Navigate(typeof(HomePage));
            }
            catch (Exception ex)
            {
                Logger.E($"Exception caught while navigation page loaded {ex.Message}");
            }
        }
        private void rootNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
        }

        private void On_Navigated(object sender, NavigationEventArgs args)
        {
            rootNavigationView.IsBackEnabled = contentFrame.CanGoBack;

            foreach (NavigationViewItemBase item in rootNavigationView.MenuItems)
            {
                if (item is NavigationViewItem && item.Name.ToString() == args.SourcePageType.Name.ToString())
                {
                    rootNavigationView.SelectedItem = item;
                    break;
                }
            }

            //else if (contentFrame.SourcePageType != null)
            //{
            //  var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);
            //
            // rootNavigationView.SelectedItem = rootNavigationView.MenuItems;
            //.OfType<muxc.NavigationViewItem>()
            //.First(n => n.Tag.Equals(item.Tag));

            //}
        }
        private bool On_BackRequested()
        {
            if (!contentFrame.CanGoBack)
                return false;

            foreach (NavigationViewItemBase item in rootNavigationView.MenuItems)
            {
                if (item is NavigationViewItem && item.Name.ToString() == contentFrame.SourcePageType.Name.ToString())
                {
                    rootNavigationView.SelectedItem = item;
                    break;
                }
            }

            // Don't go back if the nav pane is overlayed.
            //if (rootNavigationView.IsPaneOpen &&
            //    (rootNavigationView.DisplayMode == NavigationViewDisplayMode.Compact ||
            //     rootNavigationView.DisplayMode ==NavigationViewDisplayMode.Minimal))
            //    return false;

            contentFrame.GoBack();
            return true;
        }

        private void rootNavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            On_BackRequested();
        }

    }

}
