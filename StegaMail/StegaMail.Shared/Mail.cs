using System;
using System.Collections.Generic;
using System.Text;
using POPMessage = OpenPop.Mime.Message;

namespace StegaMail
{
    public class Mail
    {
        public int id { get; private set; }
        public string Temat { get; set; }
        public string Od { get; set; }
        public string Data { get; set; }
        public POPMessage mail { get; set; }

        public Mail(int id,string Temat,string Od,string Data)
        {
            this.id = id;
            this.Temat = Temat;
            this.Od = Od;
            this.Data = Data;

        }

        public Mail(int id, string Temat, string Od, string Data,POPMessage m)
        {
            this.id = id;
            this.Temat = Temat;
            this.Od = Od;
            this.Data = Data;
            mail = m;

        }
    }
}
