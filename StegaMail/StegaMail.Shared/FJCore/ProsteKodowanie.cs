using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StegaMail.FJCore {
    public static class ProsteKodowanie {
        public static bool Code = false;
        public static short Color = -1;

        public static void CodeBlock(ref int[] dane, int index) {
            byte atom = Plik.ReadByte(index);
            CodeValue(ref dane, atom);
        }

        public static byte DecodeBlock(float[] dane) {
            byte result = 0;

            for(int i = 0; i < 8; i++) {
                if(Math.Abs((int)dane[i]) % 2 == 0) {
                    result += (byte)(1 << i);
                }
            }

            return result;
        }

        public static void CodeSize(ref int[] dane, int index) {
            byte[] size = Plik.GetShortSize();
            CodeValue(ref dane, size[index]);
        }

        public static int DecodeSize(float[] dane) {
            byte[] size = { (byte)dane[0], (byte)dane[1], (byte)dane[2], (byte)dane[3] };
            return Plik.ConvertSize(size);
        }

        static void CodeValue(ref int[] dane, byte atom) {
            byte j = 1;

            for(int i = 0; i < 8; i++) {
                //nieparzyste - 0
                //parzyste - 1

                if(((atom & j) != 0 && Math.Abs((int)dane[i]) % 2 == 1) ||
                    ((atom & j) == 0 && Math.Abs((int)dane[i]) % 2 == 0)) {
                    if(dane[i] == 0) {
                        dane[i] += 1;
                    }
                    else {
                        dane[i] += 1 * Math.Sign((int)dane[i]);
                    }
                }

                j = (byte)(j << 1);
            }
        }

    }
}
