using Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unvired.Kernel.Sync;
using Unvired.Kernel.Utils;
using Unvired.Kernel.UWP.Constants;
using Unvired.Kernel.UWP.Core;
using Unvired.Kernel.UWP.Database;
using Unvired.Kernel.UWP.Log;
using Unvired.Kernel.UWP.Login;
using Unvired.Kernel.UWP.Model;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UNVIRED.DB.SAMPLE.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private CONTACT_HEADER _searchContactInput;
        public CONTACT_HEADER SearchContactInput
        {
            get => _searchContactInput;
            set
            {
                _searchContactInput = value;
                RaisePropertyChanged("SearchContactInput");
            }
        }
        private CONTACT_HEADER _searchContactOutPut;
        public CONTACT_HEADER SearchContactOutPut
        {
            get => _searchContactOutPut;
            set
            {
                _searchContactOutPut = value;
                RaisePropertyChanged("SearchContactOutPut");
            }
        }
        internal static readonly DataManagerImpl AppDataManager = ApplicationManager.Instance.GetDataManager();
        private string _searchString;
        public string SearchString
        {
            get => _searchString;
            set
            {
                _searchString = value;
                RaisePropertyChanged("SearchString");
                SearchContact(_searchString);
            }
        }
        public HomePage()
        {
            this.InitializeComponent();
            if (ContactHeaderCollection == null) ContactHeaderCollection = new ObservableCollection<List<CONTACT_HEADER>>();
            SearchContactInput = new CONTACT_HEADER();
            SearchContactOutPut = new CONTACT_HEADER();
            SearchSplitView.IsPaneOpen = false;
            PopulateContact();
            contactGrid.Visibility = Visibility.Collapsed;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<List<CONTACT_HEADER>> _contactHeaderCollection;
        public ObservableCollection<List<CONTACT_HEADER>> ContactHeaderCollection
        {
            get => _contactHeaderCollection;
            set
            {
                _contactHeaderCollection = value;
                RaisePropertyChanged("ContactHeaderCollection");
            }
        }

        public void PopulateContact()
        {
            try
            {
                ContactList.ItemsSource = AppDataManager.Get<CONTACT_HEADER>().OrderBy(x => x.ContactName).ToList();
            }
            catch (Exception e)
            {

                Logger.E($"Exception caught while fetching the Contact {e.Message}");
            }
        }

        public void SearchContact(string searchString)
        {
            try
            {
                var query = $"SELECT * from {typeof(CONTACT_HEADER).Name} WHERE ContactName LIKE '%{searchString}%' OR Phone LIKE '%{searchString}%'";
                ContactList.ItemsSource = AppDataManager.ExecuteDeferredQuery(typeof(CONTACT_HEADER).Name, query);
            }
            catch (Exception e)
            {

                Logger.E($"Exception caught while searching the Contact {e.Message}");
            }
        }

        private async void SearchContactClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(contactNameInputText.Text) && string.IsNullOrEmpty(contactIdInputText.Text))
                {
                    validationfield.Visibility = Visibility.Visible;
                    validationMessage.Text = "Please enter at least one Field";
                    return;
                }
                else
                {
                    SearchContactInput.ContactName = contactNameInputText.Text;
                    SearchContactInput.ContactId = string.IsNullOrEmpty(contactIdInputText.Text) ? 0 : int.Parse(contactIdInputText.Text);
                    validationfield.Visibility = Visibility.Collapsed;

                    await CallPAForContact(SearchContactInput);

                }

            }
            catch (Exception ex)
            {
                Logger.E($"Exception caught while Searching contacts {ex.Message}");

            }

        }
        private async Task CallPAForContact(CONTACT_HEADER weatherHeaderInput)
        {
            Util.ShowProgressDialog("Please wait getting the weather");
            try
            {

                var iSyncResponse = await SyncEngine.Instance.SubmitInForeground(MessageRequestType.RQST,
              weatherHeaderInput, null, AppConstants.PA_GET_CONTACT, false);
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
                    var result = Util.InformationAlert("Information", "Successfully Downloaded", "OK");
                    await result.ShowAsync();
                    contactGrid.Visibility = Visibility.Visible;
                    BindResponseFromUMP(response.DataBEs);
                }

                else
                {
                    await Util.InformationAlert("Alert..!", $"Please enter City Name", "OK").ShowAsync();
                }
            }
            catch (Exception e)
            {
                Logger.E($"Exception caught while getting document {e.Message}");
            }
            Util.HideProgressDialog();
        }
        private void BindResponseFromUMP(Dictionary<string, Dictionary<DataStructure, List<DataStructure>>> dataBEs)
        {
            try
            {

                for (int i = 0; i < dataBEs.Count; i++)
                {
                    var item = dataBEs.ElementAt(i);
                    if (item.Key.Equals("CONTACT"))
                    {
                        for (int j = 0; j < item.Value.Count; j++)
                        {
                            var headerData = (item.Value).ElementAt(j);

                            SearchContactOutPut = (CONTACT_HEADER)headerData.Key;
                            contactNameText.Text = SearchContactOutPut.ContactName;
                            phoneNumberText.Text = SearchContactOutPut.Phone;
                            emailText.Text = SearchContactOutPut.Email;
                        }
                    }
                }


            }
            catch (Exception e)
            {
                Logger.E($"Exception caught while binding data to the physical inventory detail page {e.Message}");
            }
        }



        private void GetContacts_Click(object sender, RoutedEventArgs e)
        {
            SearchSplitView.IsPaneOpen = true;
            validationfield.Visibility = Visibility.Collapsed;
            contactGrid.Visibility = Visibility.Collapsed;
            contactIdInputText.Text = contactNameInputText.Text = string.Empty;
        }

        private async void closeSpliview_Click(object sender, RoutedEventArgs e)
        {
            if (SearchContactOutPut.ContactId == null)
            {
                SearchSplitView.IsPaneOpen = false;
            }
            else
            {
                var confirmationDialog = Util.CommonContentDialog(Util.GetString("Alert"), "Do you want to save this result?", Util.GetString("Yes"), Util.GetString("No"));
                var result = await confirmationDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    try
                    {
                        AppDataManager.InsertOrUpdateBasedOnGid<CONTACT_HEADER>(SearchContactOutPut);
                        ContactList.ItemsSource = AppDataManager.Get<CONTACT_HEADER>().OrderBy(x => x.ContactName).ToList();
                    }
                    catch (Exception ex)
                    {
                        Logger.E($"Exception caught while adding a contact {ex.Message}");

                    }
                    SearchSplitView.IsPaneOpen = false;
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    SearchSplitView.IsPaneOpen = false;
                }
            }

        }

        private void contactIdInputText_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(contactIdInputText.Text) && contactIdInputText.Text.Length > 0)
                {
                    bool isNumeric = Util.IsNumeric(contactIdInputText.Text);
                    if (!isNumeric)
                    {
                        contactIdInputText.Text = contactIdInputText.Text.Substring(0, contactIdInputText.Text.Length - 1);
                        contactIdInputText.Select(contactIdInputText.Text.Length, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.E($"Exception caught while last focus command executing {ex.Message}");
            }
        }
    }
}
