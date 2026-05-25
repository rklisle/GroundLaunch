using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundLunch
{
    internal class Crc32
    {
        static uint[] crc32_tab = new uint[256];
        static public uint CalCRC32(Byte[] array, uint nDatalen)
        {
            Byte s_bInitFlag = 0;
            uint dGain = 0xFFFFFFFF;
            int i = 0;

            if (s_bInitFlag == 0)
            {
                s_bInitFlag = 1;
                for (i = 0; i < 256; i++)
                {
                    crc32_tab[i] = 0x00000000;
                }
                fun_Init_CRC32();
            }

            int pos = 0;
            while (nDatalen-- != 0)
            {
                dGain = (dGain >> 8) ^ crc32_tab[(dGain & 0xFF) ^ ((array[pos++]) & 0xFF)];
            }
            return dGain ^ 0xFFFFFFFF;
        }

        static void fun_Init_CRC32()
        {
            uint ulPolynomial = 0x04c11db7;
            uint i = 0, j = 0;

            for (i = 0; i <= 0xFF; i++) /*256 Values representing ascii character ades*/
            {
                crc32_tab[i] = fun_Reflect(i, 8) << 24;
                for (j = 0; j < 8; j++)
                {
                    crc32_tab[i] = (crc32_tab[i] << 1) ^ ((crc32_tab[i] & (1 << 31)) != 0 ? ulPolynomial : 0);
                }
                crc32_tab[i] = fun_Reflect(crc32_tab[i], 32);
            }
            return;
        }

        static uint fun_Reflect(uint refe, Byte ch)
        {
            uint value = 0;
            int i = 0;
            for (i = 1; i < (ch + 1); i++)
            {
                if ((refe & 1) != 0)
                    value |= (uint)(1 << (ch - i));

                refe >>= 1;
            }
            return value;
        }
    }
}
