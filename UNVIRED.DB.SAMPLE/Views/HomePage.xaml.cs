﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unvired.Kernel.Utils;
using Unvired.Kernel.UWP.Log;
using Unvired.Kernel.UWP.Login;
using UNVIRED_REST_SAMPLE.Utility;
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
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            ApplicationVersion.Text = LoginParameters.AssemblyVersion;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            SettingSplitView.IsPaneOpen = true;
        }

        private void EmailBtn_Click(object sender, RoutedEventArgs e)
        {
            Logger.SendLogViaEmail();
        }

        private void SendLogToServerBtn_Click(object sender, RoutedEventArgs e)
        {
            Logger.SendLogToServer();
        }

        private void ViewLogBtn_Click(object sender, RoutedEventArgs e)
        {
            Logger.GetLogViewer();
        }

        private async void ClearLogsBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clearLogConfirmationDialog = Util.CommonContentDialog(Util.GetString("Alert"), "This will clear all the Logs associated with this application. Do you want to clear log", "Yes", "Cancel");
                var clearLogResult = await clearLogConfirmationDialog.ShowAsync();

                if (clearLogResult == ContentDialogResult.Primary)
                {
                    Logger.DeleteLogs();
                }
            }
            catch (Exception ex)
            {
                Logger.E($"Exception caught while clearing the application logs. Message {ex.Message}");
            }
        }

        private async void ClearDataBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clearDataConfirmationDialog = Util.CommonContentDialog("Clear Application Data", "This will clear all application related data. Are you sure you want to continue?", Util.GetString("Yes"), Util.GetString("Cancel"));
                var clearDataResult = await clearDataConfirmationDialog.ShowAsync();
                if (clearDataResult == ContentDialogResult.Primary)
                {
                    Task clearData = FrameworkHelper.ClearData();
                    await clearData;
                }
            }
            catch (Exception ex)
            {
                Logger.E($"Exception caught while clearing the application data. Message {ex.Message}");
            }
        }
    }
}
