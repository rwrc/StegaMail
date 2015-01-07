using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Storage.Streams;
//using System.Windows.Forms;

namespace StegaMail.FJCore {
    public static class Plik {
        static byte[] file = null;
        static byte[] outfile = null;

        public static async Task /*static void*/ LoadFile(string path){
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(path));
            StorageFile plik = null;

            try
            {
                plik = await folder.GetFileAsync(Path.GetFileName(path));
            }
            catch (FileNotFoundException)
            {
                //MessageBox.Show("Podany plik nie istnieje!");
            }

            IRandomAccessStream stream = await plik.OpenReadAsync();

            using (DataReader dataReader = new DataReader(stream)){
                file = new byte[stream.Size];
                await dataReader.LoadAsync((uint)stream.Size);
                dataReader.ReadBytes(file);
            }
            
            /*if(fileExists(path)) {
                file = File.ReadAllBytes(path);
                //MessageBox.Show("Załadowano pomyślnie!　:)");
            }
            else {
                MessageBox.Show("Podany plik nie istnieje!");
            }*/
        }

        public static byte ReadByte(int index) {
            if(file == null || file.Length <= index) {
                throw new Exception(Error.EMPTY);
                //return (byte)0;
            }
            
            return file[index];
        }

        public static int GetSize() {
            if(file == null) {
                throw new Exception(Error.EMPTY);
                //return (int)0;
            }

            return file.Length;
        }

        public static byte[] GetShortSize() {
            if(file == null) {
                throw new Exception(Error.EMPTY);
            }

            byte[] wynik = new byte[4];
            wynik = BitConverter.GetBytes(file.Length);

            return wynik;
        }

        public static int ConvertSize(byte[] dane) {
            int result = 0;

            if(dane != null && dane.Length == 4) {
                result = BitConverter.ToInt32(dane, 0);
            }

            return result;
        }

        public static async Task /*static void*/ SaveResult(string ścieżka) {
            //File.WriteAllBytes(ścieżka, outfile);
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(ścieżka)); //Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile plik = await folder.CreateFileAsync(Path.GetFileName(ścieżka), CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(plik, outfile);
        }

        public static void AddResult(byte[] dane) {
            outfile = dane;
        }

    }
}
