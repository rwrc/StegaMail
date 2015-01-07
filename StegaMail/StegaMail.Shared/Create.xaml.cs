using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using OpenPop.Pop3;
//using OpenSmtp.Mail;
//using OpenSmtp;
//using LightBuzz.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.Headers;
using Limilabs.Client.SMTP;
using Limilabs.Mail.MIME;
using Windows.Storage.Pickers;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegaMail
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Create : Page
    {
        public Create()
        {
            this.InitializeComponent();
            zalaczniki.DataContext = this;
        }

        public ObservableCollection<Windows.Storage.StorageFile> zal = new ObservableCollection<Windows.Storage.StorageFile>();
        MailClient _mailClient;

        public ObservableCollection<Windows.Storage.StorageFile> Zal
        {
            get
            {
                return zal;
            }
        }

        private async void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {

            #if WINDOWS_APP
            Windows.Storage.Pickers.FileOpenPicker fileOpenPicker = new Windows.Storage.Pickers.FileOpenPicker();
#endif
#if WINDOWS_PHONE_APP
	FileOpenPicker fileOpenPicker = new FileOpenPicker();
fileOpenPicker.FileTypeFilter.Add(".docx");

#endif
            //fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;    
  
// Set ViewMode    
//fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;    
  
// Filter for file types. For example, if you want to open text files,  
// you will add .txt to the list.  
  
fileOpenPicker.FileTypeFilter.Clear();    
fileOpenPicker.FileTypeFilter.Add(".doc");
fileOpenPicker.FileTypeFilter.Add(".txt"); 
fileOpenPicker.FileTypeFilter.Add(".jpeg");    
fileOpenPicker.FileTypeFilter.Add(".jpg");    
fileOpenPicker.FileTypeFilter.Add(".bmp");  
  
// Open FileOpenPicker  
            //TODO PLIKI
            StorageFile file = null;
#if WINDOWS_APP
    file = await fileOpenPicker.PickSingleFileAsync();
#endif

            #if WINDOWS_PHONE_APP
            fileOpenPicker.PickSingleFileAndContinue(); 
#endif
            
if (file != null)
{
    zal.Add(file);
    
        
    // Set BitmapImage Source    
    //ImageViewer.Source = bitmapImage;     
    //save = new Windows.Storage.Pickers.FileSavePicker();
    //zalacznik z = zalaczniki.SelectedItem as zalacznik;
    // if (z == null) return;
}
        }

        private async void Image_Tapped_1(object sender, TappedRoutedEventArgs e)
        {



            MailBuilder builder = new MailBuilder();
	        builder.From.Add(new MailBox("hackaton@o2.pl", ""));
	        builder.To.Add(new MailBox("Hackaton@o2.pl", ""));
	        builder.Subject = "Test";
	        builder.Text = note.Text;
	 
	        // Read attachment from disk, add it to Attachments collection
            MimeData attachment;
            foreach (StorageFile f in Zal)
                attachment = await builder.AddAttachment(f);
	 
	        IMail email = builder.Create();
	 
	        // Send the message
	        using (Limilabs.Client.SMTP.Smtp smtp = new Smtp())
	        {
	            await smtp.Connect("poczta.o2.pl");   // or ConnectSSL for SSL
	            await smtp.UseBestLoginAsync("hackaton", "hackaton");   // remove if not needed
	 
	            smtp.SendMessage(email);
	 
	            smtp.Close();
	        }
            Frame.GoBack();

            
            


/*

            EmailClient client = new EmailClient
            {
                Server = "example.com",
                Port = 25,
                Username = "info@example.com",
                Password = "Pa$$w0rd",
                From = "you@example.com",
                To = "someone@anotherdomain.com",
                SSL = false,
                Subject = "Subject line of your message",
                Message = "This is an email sent from a WinRT app!"
            };
            

            await client.SendAsync();

            */

/*

            OpenSmtp.Mail.Smtp c = new Smtp("poczta.o2.pl", "hackaton", "3264128");
            c.SendMail(new MailMessage());

            OpenSmtp.Mail.EmailAddress add = new EmailAddress("hackaton@o2.pl");
            MailMessage msg = new MailMessage();
            msg.AddRecipient("hackaton@o2.pl", AddressType.To);
           // msg.To.Add(add);
            msg.From = new EmailAddress("hackaton@o2.pl");
            msg.Subject = title.Text;
            msg.Body = note.Text;
            //c.Credentials = new System.Net.NetworkCredential(txtYourEmailAddr.Text, txtYourPassword.Text);
            foreach (StorageFile f in Zal)
                msg.AddAttachment(new Attachment((await f.OpenReadAsync()).AsStreamForRead(), f.Name));
            c.SendMail(msg);*/
        }

        private void Image_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
