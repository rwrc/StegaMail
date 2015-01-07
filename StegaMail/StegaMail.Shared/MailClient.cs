using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using POPMessage = OpenPop.Mime.Message;
using Windows.UI.Popups;

namespace StegaMail
{
    class MailClient
    {
        public ObservableCollection<Mail> listy = new ObservableCollection<Mail>();
        private Pop3Client client = null;
        static public MailClient last;

        public ObservableCollection<Mail> Listy
        {
            get
            {
                return listy;
            }
        }

        public MailClient()
        {
            client = new Pop3Client();
            last = this;
        }

        public async Task Connect()
        {

                await Task.Run(() =>
                {
#if WINDOWS_APP
                    if (client == null)
                        client = new Pop3Client();
            client.Connect("poczta.o2.pl", 995, true);
            client.Authenticate("hackaton", "hackaton");
#endif
                });

        }


        public async Task<int> GetMailsCount()
        {
            if(client==null||client.Connected==false)
            await Connect();
            
            return await Task.Run<int>(() => client.GetMessageCount());

        }
        public async Task<OpenPop.Mime.Message> GetMail(Mail x)
        {
            return await Task.Run<OpenPop.Mime.Message>(() =>
            {
                try
                {
                    return client.GetMessage(x.id);
                }
                catch (NotSupportedException e) { return null; }
            });

        }

        public async Task<OpenPop.Mime.Message> GetMail(int x)
        {
            return await Task.Run<OpenPop.Mime.Message>(() =>
            {
                try
                {
                    return client.GetMessage(x);
                }
                catch (NotSupportedException e) { return null; }
            });

        }

        public async Task<OpenPop.Mime.Header.MessageHeader> GetHead(int x)
        {
            return await Task.Run<OpenPop.Mime.Header.MessageHeader>(() =>
            {
                try
                {
                    return client.GetMessageHeaders(x);
                }
                catch (NotSupportedException e) { return null; }
            });

        }

        public void DelMail(Mail i)
        {
            client.DeleteMessage(i.id);
            listy.Remove(i);

        }

        public async Task GetMails()
        {
            int count=await GetMailsCount();


              //  for (int i = count; (i > count - 100) && (i > 0); i--)
              //  {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    {
                        for (int i = count; (i > 0) && (i > 0); i--)
                        {
                            var m = await GetHead(i);
                            //var m = client.GetMessageHeaders(i);
                            string tmp = m.Subject.Replace("Please purchase MailForWindowsStore.dll license at http://www.limilabs.com/mail-for-windows-store", "DajNotatki;Brak tematu");
                            Regex r = new Regex("DajNotatki;(.*)");
                            if (r.IsMatch(tmp))
                            {
                              //  var mail = await GetMail(i);
                                listy.Add(new Mail(i, tmp.Substring(11), "", m.DateSent.ToString()/*,mail*/,null));
                            }
                            //await Task.Delay(10000);
                        }
                    }); 

                //}
                           

        }
    }
}
