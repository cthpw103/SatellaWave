using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

namespace SatellaWave
{
    static class BSFile
    {

        public static byte GetSoftChannel(string SoftChannel, byte num)
        {
            // 0.1.2.3
            string[] OutputNum = SoftChannel.Split('.');
            byte Output = 0;

            Output |= Convert.ToByte(OutputNum[num]);
            return Output;
        }

        public static bool MakeDownloadFile(string filename, string chn, decimal trans_id, string folder) //BSX[CHN]-[NUM].bin
        {
            if (filename == "") //Filename Check
            {
                return false;
            }
            FileStream BSXStream = File.Open(filename, FileMode.Open);
            if (BSXStream.Length <= 0)
            {
                return false;
            }
            if (folder == "")
            {
                folder = ".";
            }

            byte tID = (byte)trans_id;

            UInt16 MaxParts = 1;
            long MaxPackets = 0;
            // 20 NOPE 127 packets max per file, 440 NOPE 2794 bytes max, a packet contains 22 bytes.
            byte CurrentPakT = 0;
            int test = 0;
            for (int x = 1; x < BSXStream.Length; x++)
            {
                test++;
                if ((x & 0xFFFF) == 0) { MaxParts++; MaxPackets += test / 22 - 1; test = 0; }
                if (test == 2784) { MaxParts++; MaxPackets += 127; test = 0; }
            }
            MaxPackets += test / 22 - 1;
            long PacketsLeft = MaxPackets;
            long PacketsDone = 0;

            UInt16 Num = 0;

            UInt16 _Fs = (UInt16)(MaxParts / 0x7F);

            long BSSize = BSXStream.Length;
            while (Num != MaxParts)
            {
                FileStream BSXOut = File.Open((folder+"\\BSX"+chn+"-"+(Num & 0x7F).ToString()+".bin"), FileMode.Create);
                // File Format
                CurrentPakT = 0;
                while (CurrentPakT < 126 && PacketsLeft >= 0)
                {
                    PacketsLeft--;
                    CurrentPakT++;
                }
                CurrentPakT++;
                //Header
                //Packets Test DATA
                long TestPakL = CurrentPakT;
                if (TestPakL > 1)
                {
                    TestPakL--;
                    while (TestPakL > 1)
                    {
                        TestPakL--;
                    }
                    TestPakL--;
                }
                PacketsDone += CurrentPakT;
                /*
                long x = 20 - CurrentPakT;
                while (x > 0)
                {
                    x--;
                }
                 */
                //Data Copy
                //Packet Header
                BSXOut.WriteByte(tID);
                BSXOut.WriteByte((byte)(Num & 0x7F));
                BSXOut.WriteByte(0);
                UInt16 size = 0;
                BSXOut.WriteByte((byte)((size / 0x100) & 0xFF));
                BSXOut.WriteByte((byte)(size & 0xFF));
                BSXOut.WriteByte(1);
                if (_Fs > 0) { BSXOut.WriteByte(0x7F); }
                else { BSXOut.WriteByte((byte)(MaxParts & 0x7F)); }
                long y = BSXStream.Position;
                BSXOut.WriteByte((byte)((y / 0x10000) & 0xFF));
                BSXOut.WriteByte((byte)((y / 0x100) & 0xFF));
                BSXOut.WriteByte((byte)(y & 0xFF));
                //Packet Data
                y = (CurrentPakT - 1) * 22 + 12;
                while (y > 0)
                {
                    BSXOut.WriteByte((byte)BSXStream.ReadByte());
                    y--;
                    if (BSXStream.Position < BSXStream.Length) { size++; }
                    
                    if ((BSXStream.Position & 0xFFFF) == 0)
                    {
                        //MaxParts++;
                        /*
                        while (y > 0)
                        {
                            BSXOut.WriteByte(0);
                            y--;
                        }
                        */
                        break;
                    }
                }
                BSXOut.Seek(0x03, SeekOrigin.Begin);
                BSXOut.WriteByte((byte)((size / 0x100) & 0xFF));
                BSXOut.WriteByte((byte)((size & 0xFF) + 5));
                //Finish, or continue.
                if ((Num & 0x7F) < 0x7F) { Num++; }
                else
                {
                    /*
                     * To convert from Decimal to Hex do...

                            string hexValue = decValue.ToString("X");
                            
                     * To convert from Hex to Decimal do either...

                            int decValue = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
                            or
                            int decValue = Convert.ToInt32(hexValue, 16);
                    */
                    int chan = Convert.ToInt32(chn, 16);
                    chan++;
                    chn = chan.ToString("X4");
                    Num++;
                    _Fs--;
                }
                //Normal Max Parts for Last.
                if (Num == MaxParts)
                {
                    BSXOut.Seek(0x06, SeekOrigin.Begin);
                    BSXOut.WriteByte((byte)(Num & 0x7F));
                }
                BSXOut.Close();
            }
            BSXStream.Close();
            return true;
        }
    }
}
