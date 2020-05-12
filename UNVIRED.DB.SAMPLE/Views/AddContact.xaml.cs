using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unvired.Kernel.Sync;
using Unvired.Kernel.UWP.Constants;
using Unvired.Kernel.UWP.Core;
using Unvired.Kernel.UWP.Database;
using Unvired.Kernel.UWP.Log;
using Unvired.Kernel.UWP.Sync;
using UNVIRED_REST_SAMPLE.Utility;
using Utils;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static Unvired.Kernel.UWP.Model.DataStructure;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UNVIRED.DB.SAMPLE.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddContact : Page
    {
        internal static readonly DataManagerImpl AppDataManager = ApplicationManager.Instance.GetDataManager();
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private CONTACT_HEADER _contactHeader;
        public CONTACT_HEADER ContactHeader
        {
            get => _contactHeader;
            set
            {
                _contactHeader = value;
                RaisePropertyChanged("ContactHeader");
            }

        }
        public AddContact()
        {
            this.InitializeComponent();
            if (ContactHeader == null) ContactHeader = new CONTACT_HEADER();
        }

        private async void Add_Contact_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ContactHeader.ContactName) || string.IsNullOrEmpty(ContactHeader.Email) || string.IsNullOrEmpty(ContactHeader.Phone))
            {
                var validationDialog = Util.InformationAlert("Alert..!", "Please enter the all fields.", "OK");
                await validationDialog.ShowAsync();
                return;
            }

            if (!ConnectionHelper.HasAnyInternetConnection())
            {
                var connectionDialog = Util.AlertContentDialog("No Internet", "No Internet ConnectionPlease Try Again Later", "OK");
                await connectionDialog.ShowAsync();
                return;
            }
            await CallPAForCreateContact(ContactHeader);
        }

        private async Task CallPAForCreateContact(CONTACT_HEADER contactHeader)
        {
            Util.ShowProgressDialog("Please wait creating contact");
            try
            {
                contactHeader.ContactId = 0;
                contactHeader.OBJECT_STATUS = OBJECT_STATUS_ENUM.ADD;
                AppDataManager.InsertOrUpdateBasedOnGid(contactHeader);
            }
            catch (Exception e)
            {
                Logger.E($"Exception caught while saving Notification History in database. Message:{e.Message}");
                Logger.E($"StackTrace - {e.StackTrace}");
            }

            try
            {

                var iSyncResponse = await SyncEngine.Instance.SubmitInForeground(MessageRequestType.RQST,
              contactHeader, null, AppConstants.PA_CREATE_CONTACT, true);
                var response = (SyncBEResponse)iSyncResponse;
                var infoMessages = response.InfoMessages;

                if (infoMessages != null && infoMessages.Any())
                {
                    foreach (var infoMessage in infoMessages)
                    {
                        if (!infoMessage.Category.Equals((ISyncResponse.RESPONSE_STATUS.FAILURE).ToString())) continue;
                        Util.HideProgressDialog();
                        var result = Util.FailureAlert("Error", infoMessage.Message, "OK");
                        await result.ShowAsync();
                        Logger.E($"Error occur while receiving the response {infoMessage.Message}");
                        return;
                    }
                }

                if (response.ResponseStatus == ISyncResponse.RESPONSE_STATUS.SUCCESS)
                {
                    var result = Util.SucessAlert("Success", $"Successfully add the {contactHeader.ContactName} in SAP ", "OK");
                    await result.ShowAsync();
                    ContactHeader.Email = ContactHeader.ContactName = ContactHeader.Phone = string.Empty;
                }

            }
            catch (Exception e)
            {
                Logger.E($"Exception caught while getting document {e.Message}");
            }
            Util.HideProgressDialog();
        }
    }
}
