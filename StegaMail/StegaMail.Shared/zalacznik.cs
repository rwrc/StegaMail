using System;
using System.Collections.Generic;
using System.Text;

namespace StegaMail
{
    public class zalacznik
    {
        public string Plik { get; private set; }
        public string Rozmiar { get; set; }
        public byte[] dane { get; private set; }

        public zalacznik(string p,string r,byte[] d)
        {
            Plik = p;
            Rozmiar = r;
            dane = d;

        }
    }
}
