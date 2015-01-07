using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using OpenPop.Mime;
using OpenPop.Mime.Header;
using OpenPop.Pop3;
using OpenPop.Pop3.Exceptions;
using OpenPop.Common.Logging;

using Windows.UI.Popups;
using Windows.UI.Xaml.Shapes;
using System.Threading.Tasks;
using POPMessage = OpenPop.Mime.Message;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegaMail
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<zalacznik> zal = new ObservableCollection<zalacznik>();
        MailClient _mailClient;

        public ObservableCollection<zalacznik> Zal
        {
            get
            {
                return zal;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            _mailClient = new MailClient();
            lv.DataContext = _mailClient;
            zalaczniki.DataContext = this;


        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            webrow.Height = new GridLength(0);
            ListRow.Height = new GridLength(1, GridUnitType.Star);
        }

        private async void lv_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            //var x = (sender as ListViewItem).DataContext as Mail;
            var x = (lv.SelectedItem as Mail);
            if (x == null) return;
           // POPMessage m = client.GetMessage(x.id);
            POPMessage m = await _mailClient.GetMail(x);
            // MessageDialog ms = new MessageDialog(System.Text.Encoding.UTF8.GetString(m.MessagePart.Body, 0, m.MessagePart.Body.GetLength(0)), x.Temat);
            MessagePart plainText = m!=null?m.FindFirstHtmlVersion():null;
            string content = "";
            if (plainText != null)
            {
                content = plainText.GetBodyAsText();
                Web.NavigateToString(content);
                
            }
            zal.Clear();
            if(m!=null)
                foreach (var attachment in m.FindAllAttachments())
                {
                    zal.Add(new zalacznik(attachment.FileName, (attachment.Body.Length / 1024).ToString() + "KB",attachment.Body));
                  
                }

                webrow.Height = new GridLength(1, GridUnitType.Star);
                ListRow.Height = new GridLength(1, GridUnitType.Star);
            
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            #if WINDOWS_APP
            try
            {
            await _mailClient.Connect();
            await _mailClient.GetMails();
            }
            catch (Exception ee)
            {
                MessageDialog ms = new MessageDialog("Błąd Połączenia.");
                ms.ShowAsync();
            }
#endif
        }

        private async void zalaczniki_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileSavePicker save = new Windows.Storage.Pickers.FileSavePicker();
            zalacznik z=zalaczniki.SelectedItem as zalacznik;
            if(z==null) return;
            save.SuggestedFileName = z.Plik;
            int i=z.Plik.LastIndexOf('.');

            save.FileTypeChoices.Add(new KeyValuePair<string, IList<string>>("Oryginalny typ pliku", new List<string>() { z.Plik.Substring(i)}));
            Windows.Storage.StorageFile file = await save.PickSaveFileAsync();
            if (file == null) return;
            Stream s = await file.OpenStreamForWriteAsync();
            using (BinaryWriter BinaryStream = new BinaryWriter(s))
            {
                BinaryStream.Write(z.dane);
                BinaryStream.Flush();
            }
            

        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Create));
        }


        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
              var m =  lv.SelectedItem as Mail;
              if (m == null) return;
              _mailClient.DelMail(m);

        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            webrow.Height = new GridLength(0);
            ListRow.Height = new GridLength(1, GridUnitType.Star);
        }

        private async void Page_GotFocus(object sender, RoutedEventArgs e)
        {
           await _mailClient.GetMails();
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            try { 
                zal.Clear();
                _mailClient.Listy.Clear();
            #if WINDOWS_APP
                await MailClient.last.GetMails();
            #endif
            }
            catch (Exception ee)
            {
                MessageDialog ms = new MessageDialog("Błąd Połączenia.");
                ms.ShowAsync();
            }
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(settings));
        }

        private void Web_LoadCompleted(object sender, NavigationEventArgs e)
        {

        }
    }
}
