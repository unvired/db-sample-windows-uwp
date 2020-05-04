using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UNVIRED_REST_SAMPLE.Utility
{
    public class Util
    {
      
            private static ContentDialog _progressContentDialog;
            private static ContentDialog _confirmationDialog;
            private static ContentDialog _submitConfirmationDialog;
            private static ResourceLoader _resourceLoader;

            public static void ShowProgressDialog(string message = "Please wait...", string title = "Processing")
            {
                try
                {
                    if (_progressContentDialog != null)
                    {
                        _progressContentDialog?.Hide();
                        _progressContentDialog = null;
                    }

                    var stackPanel = new StackPanel
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    var progressRing = new ProgressRing
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 92, 162)),
                        IsActive = true
                    };
                    var textBlock = new TextBlock { Text = message };

                    stackPanel.Children.Add(progressRing);
                    stackPanel.Children.Add(textBlock);

                    _progressContentDialog = new ContentDialog
                    {
                        Title = title,
                        Content = stackPanel,
                        Width = Window.Current.Bounds.Width
                    };

                    _progressContentDialog.ShowAsync().GetResults();
                }
                catch (Exception e)
                {
                  //  Logger.E($"Exception caught while showing content dialog for {message}.error{e.Message}");
                }
            }

            internal static void HideProgressDialog()
            {
                _progressContentDialog?.Hide();
            }

            public static ContentDialog CommonContentDialog(string title, string message, string primaryButtonText, string secondaryButtonText = "")
            {
                HideProgressDialog();
                if (_confirmationDialog != null)
                {
                    _confirmationDialog.Hide();
                    _confirmationDialog = null;
                }

                var textBlock = new TextBlock();
                var grid = new Grid { Height = 40, Width = 500, HorizontalAlignment = HorizontalAlignment.Stretch };

                textBlock.Text = message;
                textBlock.TextWrapping = TextWrapping.Wrap;
                grid.Children.Add(textBlock);

                _confirmationDialog = new ContentDialog
                {
                    Title = title,
                    Content = grid,
                    PrimaryButtonText = primaryButtonText,
                    SecondaryButtonText = secondaryButtonText,
                };
                return _confirmationDialog;
            }

            public static string GetString(string fieldName)
            {
                if (_resourceLoader == null)
                    _resourceLoader = new ResourceLoader();
                if (string.IsNullOrEmpty(fieldName)) return "";
                var fieldValue = _resourceLoader.GetString(fieldName);
                return string.IsNullOrEmpty(fieldValue) ? fieldName : fieldValue;
            }



           

            public static ContentDialog AlertContentDialog(string title, string message, string closeButtonText)
            {
                HideProgressDialog();
                var textBlock = new TextBlock();
                var grid = new Grid { Height = 40, Width = 500, HorizontalAlignment = HorizontalAlignment.Stretch };
                textBlock.Text = message;
                textBlock.TextWrapping = TextWrapping.Wrap;
                grid.Children.Add(textBlock);
                return new ContentDialog
                {
                    Title = title,
                    Content = grid,
                    CloseButtonText = closeButtonText
                };
            }

            public static ContentDialog SucessAlert(string title, string message, string closeButtonText)
            {
                HideProgressDialog();
                var grid = new Grid { Height = 190, Width = 400, HorizontalAlignment = HorizontalAlignment.Stretch };
                RowDefinition row1 = new RowDefinition();
                RowDefinition row2 = new RowDefinition();
                RowDefinition row3 = new RowDefinition();

                row1.Height = new GridLength(100);
                row2.Height = new GridLength(40);
                row3.Height = new GridLength(50);

                grid.RowDefinitions.Add(row1);
                grid.RowDefinitions.Add(row2);
                grid.RowDefinitions.Add(row3);

                var textBlock = new TextBlock()
                {
                    Text = title,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 18,
                    FontWeight = FontWeights.SemiBold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 10),
                };
                var normalTextBox = new TextBlock()
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 16,
                    FontWeight = FontWeights.Normal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 10),
                };
                var image = new Image()
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Images/Success_Message.png")),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Height = 80,
                    Width = 80,
                };

                grid.Children.Add(textBlock);
                grid.Children.Add(image);
                grid.Children.Add(normalTextBox);
                Grid.SetRow(image, 0);
                Grid.SetRow(textBlock, 1);
                Grid.SetRow(normalTextBox, 2);

                return new ContentDialog
                {
                    Content = grid,
                    CloseButtonText = closeButtonText,
                };
            }

            public static ContentDialog FailureAlert(string title, string message, string closeButtonText)
            {
                HideProgressDialog();
                var grid = new Grid { Height = 190, Width = 400, HorizontalAlignment = HorizontalAlignment.Stretch };
                RowDefinition row1 = new RowDefinition();
                RowDefinition row2 = new RowDefinition();
                RowDefinition row3 = new RowDefinition();

                row1.Height = new GridLength(100);
                row2.Height = new GridLength(40);
                row3.Height = new GridLength(50);

                grid.RowDefinitions.Add(row1);
                grid.RowDefinitions.Add(row2);
                grid.RowDefinitions.Add(row3);

                var textBlock = new TextBlock()
                {
                    Text = title,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 18,
                    FontWeight = FontWeights.SemiBold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 10),
                };
                var normalTextBox = new TextBlock()
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 16,
                    FontWeight = FontWeights.Normal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 10),
                };
                var image = new Image()
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Images/Error_Message.png")),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Height = 80,
                    Width = 80,
                };

                grid.Children.Add(textBlock);
                grid.Children.Add(image);
                grid.Children.Add(normalTextBox);
                Grid.SetRow(image, 0);
                Grid.SetRow(textBlock, 1);
                Grid.SetRow(normalTextBox, 2);

                return new ContentDialog
                {
                    Content = grid,
                    CloseButtonText = closeButtonText,
                };
            }

            public static ContentDialog InformationAlert(string title, string message, string closeButtonText)
            {
                HideProgressDialog();
                var grid = new Grid { Height = 190, Width = 400, HorizontalAlignment = HorizontalAlignment.Stretch };
                RowDefinition row1 = new RowDefinition();
                RowDefinition row2 = new RowDefinition();
                RowDefinition row3 = new RowDefinition();

                row1.Height = new GridLength(100);
                row2.Height = new GridLength(40);
                row3.Height = new GridLength(50);

                grid.RowDefinitions.Add(row1);
                grid.RowDefinitions.Add(row2);
                grid.RowDefinitions.Add(row3);

                var textBlock = new TextBlock()
                {
                    Text = title,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 18,
                    FontWeight = FontWeights.SemiBold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 10),
                };
                var normalTextBox = new TextBlock()
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 16,
                    FontWeight = FontWeights.Normal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 10),
                };
                var image = new Image()
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Images/Info_Message.png")),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Height = 80,
                    Width = 80,
                };

                grid.Children.Add(textBlock);
                grid.Children.Add(image);
                grid.Children.Add(normalTextBox);
                Grid.SetRow(image, 0);
                Grid.SetRow(textBlock, 1);
                Grid.SetRow(normalTextBox, 2);

                return new ContentDialog
                {
                    Content = grid,
                    CloseButtonText = closeButtonText,
                    Width = Window.Current.Bounds.Width
                };
            }

            public static ContentDialog SubmitConfirmationDialog(string title = "Enter the Remarks for this Document", string primaryButtonText = "Submit", string secondaryButtonText = "Cancel")
            {
                HideProgressDialog();
                if (_submitConfirmationDialog != null)
                {
                    _submitConfirmationDialog.Hide();
                    _submitConfirmationDialog = null;
                }

                var grid = new Grid { Height = 190, Width = 400, HorizontalAlignment = HorizontalAlignment.Stretch };

                var textBlock = new TextBox()
                {
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 18,
                    FontWeight = FontWeights.Normal,
                    Height = 170,
                    Width = 400,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 10),
                };



                grid.Children.Add(textBlock);
                Grid.SetRow(textBlock, 0);

                _submitConfirmationDialog = new ContentDialog
                {
                    Title = title,
                    Content = grid,
                    PrimaryButtonText = primaryButtonText,
                    SecondaryButtonText = secondaryButtonText,
                };
                return _submitConfirmationDialog;
            }




           
        }
}
