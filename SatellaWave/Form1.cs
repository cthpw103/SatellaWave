using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
            comboBox8.SelectedIndex = 0;
            comboBox9.SelectedIndex = 0;
            comboBox10.SelectedIndex = 0;
            comboBox11.SelectedIndex = 0;
            comboBox12.SelectedIndex = 0;
            comboBox13.SelectedIndex = 0;
            comboBox14.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Save the file!
            saveFileDialog1.Title = "Save as BS Server File...";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                string BSFile = saveFileDialog1.FileName;
                //SatellaWave.BSFile.SaveFile(tabControl1.SelectedIndex, BSFile);
                int Type = tabControl1.SelectedIndex;
                //NOTE: WITHOUT HEADER.
                FileStream BSTownFile = File.Open(BSFile, FileMode.Create);
                UInt16 CurSize = 0;
                if (Type == 0)      // Channel Map
                {
                    BSTownFile.WriteByte((byte)numericUpDown15.Value);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    //CurSize = (UInt16)(24 - 5);
                    //BSTownFile.WriteByte((byte)(CurSize >> 8));
                    //BSTownFile.WriteByte((byte)CurSize);
                    BSTownFile.WriteByte(0x1);
                    BSTownFile.WriteByte(0xb3);

                    BSTownFile.WriteByte(0x53);
                    BSTownFile.WriteByte(0x46);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte((byte)listBox5.Items.Count);
                    BSTownFile.WriteByte((byte)(listBox5.Items.Count + 0x53 + 0x46));

                    int MainSoftChn1 = 10000;
                    int MainSoftChn2 = 10000;
                    for (int x = 0; x < listBox5.Items.Count; x++)
                    {
                        listBox5.SelectedIndex = x;
                        string[] CurrentData = listBox5.SelectedItem.ToString().Split('/');
                        if (SatellaWave.BSFile.GetSoftChannel(CurrentData[0], 0) != MainSoftChn1 || SatellaWave.BSFile.GetSoftChannel(CurrentData[0], 1) != MainSoftChn2)
                        {
                            //Search Mode
                            MainSoftChn1 = SatellaWave.BSFile.GetSoftChannel(CurrentData[0], 0);
                            MainSoftChn2 = SatellaWave.BSFile.GetSoftChannel(CurrentData[0], 1);
                            BSTownFile.WriteByte((byte)MainSoftChn1);
                            BSTownFile.WriteByte((byte)MainSoftChn2);
                            int NumberOfItems = 1;
                            for (int y = x + 1; y < listBox5.Items.Count; y++)
                            {
                                listBox5.SelectedIndex = y;
                                string[] DataCheck = listBox5.SelectedItem.ToString().Split('/');
                                if (SatellaWave.BSFile.GetSoftChannel(DataCheck[0], 0) == MainSoftChn1 && SatellaWave.BSFile.GetSoftChannel(DataCheck[0], 1) == MainSoftChn2)
                                {
                                    NumberOfItems++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            BSTownFile.WriteByte((byte)NumberOfItems);
                        }
                            //Add Data
                            BSTownFile.WriteByte(0);
                            BSTownFile.WriteByte(SatellaWave.BSFile.GetSoftChannel(CurrentData[0], 2));
                            BSTownFile.WriteByte(SatellaWave.BSFile.GetSoftChannel(CurrentData[0], 3));
                            BSTownFile.WriteByte(0);
                            BSTownFile.WriteByte(0);
                            BSTownFile.WriteByte(0);
                            BSTownFile.WriteByte(0);
                            BSTownFile.WriteByte(0);

                            BSTownFile.WriteByte((byte)(Convert.ToInt32(CurrentData[1]) >> 8));
                            BSTownFile.WriteByte((byte)(Convert.ToInt32(CurrentData[1]) & 255));

                            //Make Type Byte
                            byte TypeByte = 0;
                            if (CurrentData[2] == "Optional")
                            {
                                TypeByte = 1;
                            }
                            else if (CurrentData[2] == "Yes")
                            {
                                TypeByte = 2;
                            }

                            if (CurrentData[3] == "PSRAM")
                            {
                                TypeByte |= 4;
                            }
                            else if (CurrentData[3] == "FLASH")
                            {
                                TypeByte |= 8;
                            }
                            else if (CurrentData[3] == "FreeFLASH")
                            {
                                TypeByte |= 12;
                            }

                            BSTownFile.WriteByte(TypeByte);
                            //HW Channel Output
                            int HWChannel = Convert.ToInt32(CurrentData[4], 16);
                            BSTownFile.WriteByte((byte)HWChannel);
                            BSTownFile.WriteByte((byte)(HWChannel>>8));
                    }
                    MessageBox.Show("File done.");
                }
                else if (Type == 1) // Town Settings
                {
                    //Header
                    BSTownFile.WriteByte((byte)numericUpDown15.Value);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    CurSize = (UInt16)(24 + listBox1.Items.Count - 5);
                    BSTownFile.WriteByte((byte)(CurSize>>8));
                    BSTownFile.WriteByte((byte)CurSize);
                    BSTownFile.WriteByte(1);
                    BSTownFile.WriteByte(1);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);

                    //Town Data begins here.
                    BSTownFile.WriteByte(0); //Must not be 1
                    BSTownFile.WriteByte((byte)numericUpDown4.Value); //Town ID
                    BSTownFile.WriteByte((byte)numericUpDown5.Value); //Directory ID
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte((byte)((comboBox3.SelectedIndex << 4) | (Convert.ToInt32(checkBox1.Checked) << 6) | (Convert.ToInt32(checkBox2.Checked) << 7)));
                    BSTownFile.WriteByte(0);
                    //Loop for NPC Data
                    byte NPCTest = 0;
                    byte NPCPlace = 0;
                    for (int x = 0; x <= 63; x++)
                    {
                        NPCTest |= (byte)(Convert.ToByte(checkedListBox1.GetItemChecked(x)) << (NPCPlace));
                        NPCPlace += 1;
                        if (NPCPlace == 8)
                        {
                            BSTownFile.WriteByte(NPCTest);
                            NPCTest = 0;
                            NPCPlace = 0;
                        }
                    }
                    //For Season
                    BSTownFile.WriteByte((byte)((Convert.ToInt32(comboBox5.SelectedIndex > 0) << (comboBox5.SelectedIndex + 3)) | (Convert.ToInt32(comboBox4.SelectedIndex > 0) << (comboBox4.SelectedIndex - 9))));
                    BSTownFile.WriteByte((byte)(Convert.ToInt32(comboBox4.SelectedIndex > 0) << comboBox4.SelectedIndex));

                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);

                    BSTownFile.WriteByte((byte)listBox1.Items.Count);

                    for (int x = 0; x < listBox1.Items.Count; x++)
                    {
                        listBox1.SelectedIndex = x;
                        BSTownFile.WriteByte(Convert.ToByte(listBox1.SelectedItem.ToString()));
                    }

                    MessageBox.Show("File done.");
                }
                else if (Type == 2) // File
                {
                    openFileDialog1.FileName = "";
                    openFileDialog1.Title = "Open File to download...";
                    openFileDialog1.ShowDialog();

                    if (openFileDialog1.FileName != "")
                    {

                    BSTownFile.WriteByte((byte)numericUpDown7.Value);
                    BSTownFile.WriteByte(Convert.ToByte(checkBox4.Checked));
                    //File Name
                    byte[] BSText = Encoding.Unicode.GetBytes(textBox4.Text);
                    byte[] BSText2 = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding("shift-jis"), BSText);
                    byte BSTLength = 0;
                    if (BSText2.Length < 21) { BSTLength = (byte)BSText2.Length; }
                    else { BSTLength = 20; }
                    BSTownFile.Write(BSText2, 0, BSTLength);
                    for (int x = BSTLength; x < 21; x++)
                    {
                        BSTownFile.WriteByte(0);
                    }
                    //File Message/Item Stuff
                    if (radioButton1.Checked)
                    {
                        BSText = Encoding.Unicode.GetBytes(textBox5.Text);
                        BSText2 = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding("shift-jis"), BSText);
                        if (BSText2.Length < 255) { BSTLength = (byte)BSText2.Length; }
                        else { BSTLength = 254; }
                        BSTownFile.WriteByte((byte)(BSTLength + 1));
                        BSTownFile.Write(BSText2, 0, BSTLength);
                        BSTownFile.WriteByte(0);
                    }
                    else
                    {
                        BSText = Encoding.Unicode.GetBytes(textBox5.Text);
                        BSText2 = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding("shift-jis"), BSText);
                        if (BSText2.Length < 37) { BSTLength = (byte)BSText2.Length; }
                        else { BSTLength = 36; }
                        BSTownFile.WriteByte(0x79);
                        BSTownFile.Write(BSText2, 0, BSTLength);
                        for (int x = BSTLength; x < 37; x++)
                        {
                            BSTownFile.WriteByte(0);
                        }

                        BSText = Encoding.Unicode.GetBytes(textBox6.Text);
                        BSText2 = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding("shift-jis"), BSText);
                        if (BSText2.Length < 70) { BSTLength = (byte)BSText2.Length; }
                        else { BSTLength = 70; }
                        BSTownFile.Write(BSText2, 0, BSTLength);
                        for (int x = BSTLength; x < 71; x++)
                        {
                            BSTownFile.WriteByte(0);
                        }

                        int ItemPrice = (int)numericUpDown13.Value;
                        BSText = Encoding.ASCII.GetBytes(ItemPrice.ToString("D12"));
                        BSTownFile.Write(BSText, 0, 12);

                        BSTownFile.WriteByte((byte)(Convert.ToByte(checkBox5.Checked)));
                    }

                    BSTownFile.WriteByte(SatellaWave.BSFile.GetSoftChannel(textBox1.Text, 0));
                    BSTownFile.WriteByte(SatellaWave.BSFile.GetSoftChannel(textBox1.Text, 1));
                    BSTownFile.WriteByte(SatellaWave.BSFile.GetSoftChannel(textBox1.Text, 2));
                    BSTownFile.WriteByte(SatellaWave.BSFile.GetSoftChannel(textBox1.Text, 3));

                        FileStream BSFile_Length = File.OpenRead(openFileDialog1.FileName);
                        BSTownFile.WriteByte((byte)(BSFile_Length.Length >> 16));
                        BSTownFile.WriteByte((byte)(BSFile_Length.Length >> 8));
                        BSTownFile.WriteByte((byte)BSFile_Length.Length);
                        BSFile_Length.Close();

                        BSTownFile.WriteByte(0);
                        BSTownFile.WriteByte(0);
                        BSTownFile.WriteByte(0);

                        BSTownFile.WriteByte((byte)((comboBox7.SelectedIndex << 3) | (Convert.ToByte(checkBox3.Checked) << 2) | Convert.ToByte(checkBox4.Checked)));
                        BSTownFile.WriteByte(0);

                        BSTownFile.WriteByte((byte)(comboBox6.SelectedIndex << 2));
                        BSTownFile.WriteByte(0);
                        BSTownFile.WriteByte(0);

                        //Date; Month and Day
                        BSTownFile.WriteByte((byte)((comboBox8.SelectedIndex + 1) << 4));
                        BSTownFile.WriteByte((byte)((byte)numericUpDown8.Value << 3));

                        //Time
                        BSTownFile.WriteByte((byte)(((byte)numericUpDown9.Value << 3) | ((byte)numericUpDown10.Value >> 2)));
                        BSTownFile.WriteByte((byte)(((byte)numericUpDown10.Value << 5) | (byte)numericUpDown11.Value));
                        BSTownFile.WriteByte((byte)((byte)numericUpDown12.Value << 2));

                        //Include File
                        BSTownFile.WriteByte(SatellaWave.BSFile.GetSoftChannel(textBox7.Text, 0));
                        BSTownFile.WriteByte(SatellaWave.BSFile.GetSoftChannel(textBox7.Text, 1));
                        BSTownFile.WriteByte(SatellaWave.BSFile.GetSoftChannel(textBox7.Text, 2));
                        BSTownFile.WriteByte(SatellaWave.BSFile.GetSoftChannel(textBox7.Text, 3));

                        BSTownFile.WriteByte(0);
                        BSTownFile.WriteByte(0);
                    }
                    MessageBox.Show("File done.");
                }
                else if (Type == 3) // Folder
                {
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte((byte)listBox2.Items.Count);

                    byte[] BSText = Encoding.Unicode.GetBytes(textBox8.Text);
                    byte[] BSText2 = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding("shift-jis"), BSText);
                    byte BSTLength = 0;
                    if (BSText2.Length < 21) { BSTLength = (byte)BSText2.Length; }
                    else { BSTLength = 20; }
                    BSTownFile.Write(BSText2, 0, BSTLength);
                    for (int x = BSTLength; x < 21; x++)
                    {
                        BSTownFile.WriteByte(0);
                    }

                    BSText = Encoding.Unicode.GetBytes(textBox9.Text);
                    BSText2 = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding("shift-jis"), BSText);
                    BSTLength = 0;
                    if (BSText2.Length < 255) { BSTLength = (byte)BSText2.Length; }
                    else { BSTLength = 254; }
                    BSTownFile.WriteByte((byte)(BSTLength + 1));
                    BSTownFile.Write(BSText2, 0, BSTLength);
                    BSTownFile.WriteByte(0);
                    if (radioButton3.Checked)
                    {
                        BSTownFile.WriteByte((byte)((Convert.ToByte(checkBox6.Checked) << 2) | (comboBox9.SelectedIndex << 1) | 0));
                    }
                    else
                    {
                        BSTownFile.WriteByte((byte)((Convert.ToByte(checkBox6.Checked) << 2) | (comboBox9.SelectedIndex << 1) | 1));
                    }
                    if (comboBox9.SelectedIndex == 0)
                    {
                        BSTownFile.WriteByte((byte)comboBox10.SelectedIndex);
                    }
                    else
                    {
                        BSTownFile.WriteByte((byte)comboBox11.SelectedIndex);
                    }
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte((byte)comboBox12.SelectedIndex);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    //Add Files!
                    for (int x = 0; x < listBox2.Items.Count; x++)
                    {
                        listBox2.SelectedIndex = x;
                        FileStream BSFileCopy = File.OpenRead(listBox2.SelectedItem.ToString());
                        for (int y = 0; y < BSFileCopy.Length; y++)
                        {
                            BSTownFile.WriteByte((byte)BSFileCopy.ReadByte());
                        }
                        BSFileCopy.Close();
                    }
                    MessageBox.Show("File done.");
                }
                else if (Type == 4) // Expansion
                {

                }
                else if (Type == 5) // Directory
                {
                    BSTownFile.WriteByte((byte)numericUpDown14.Value);
                    BSTownFile.WriteByte((byte)listBox3.Items.Count);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);

                    for (int x = 0; x < listBox3.Items.Count; x++)
                    {
                        listBox3.SelectedIndex = x;
                        FileStream BSFileCopy = File.OpenRead(listBox3.SelectedItem.ToString());
                        for (int y = 0; y < BSFileCopy.Length; y++)
                        {
                            BSTownFile.WriteByte((byte)BSFileCopy.ReadByte());
                        }
                        BSFileCopy.Close();
                    }

                    BSTownFile.WriteByte((byte)listBox4.Items.Count);

                    for (int x = 0; x < listBox4.Items.Count; x++)
                    {
                        listBox4.SelectedIndex = x;
                        FileStream BSFileCopy = File.OpenRead(listBox4.SelectedItem.ToString());
                        for (int y = 0; y < BSFileCopy.Length; y++)
                        {
                            BSTownFile.WriteByte((byte)BSFileCopy.ReadByte());
                        }
                        BSFileCopy.Close();
                    }
                    MessageBox.Show("File done.");
                }
                else if (Type == 6) // Patch
                {

                }
                else if (Type == 7) // Welcome
                {
                    BSTownFile.WriteByte((byte)numericUpDown15.Value);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    CurSize = (UInt16)(textBox3.TextLength + 1 - 5);
                    BSTownFile.WriteByte((byte)(CurSize >> 8));
                    BSTownFile.WriteByte((byte)CurSize);
                    BSTownFile.WriteByte(1);
                    BSTownFile.WriteByte(1);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);
                    BSTownFile.WriteByte(0);

                    byte[] BSText = Encoding.Unicode.GetBytes(textBox3.Text);
                    byte[] BSText2 = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding("shift-jis"), BSText);
                    int BSTLength = 0;
                    if (BSText2.Length < 100) { BSTLength = (byte)BSText2.Length; }
                    else { BSTLength = 99; }
                    BSTownFile.Write(BSText2, 0, BSTLength);
                    BSTownFile.WriteByte(0);

                    MessageBox.Show("File done.");
                }
                else if (Type == 8) // Time
                {

                }
                else if (Type == 9) // Special
                {

                }
                BSTownFile.Close();
                BSTownFile.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(numericUpDown6.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Open BS File...";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                listBox2.Items.Add(openFileDialog1.FileName);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Open BS Folder...";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                listBox3.Items.Add(openFileDialog1.FileName);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                listBox3.Items.RemoveAt(listBox3.SelectedIndex);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Open BS Expansion...";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                listBox4.Items.Add(openFileDialog1.FileName);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                listBox4.Items.RemoveAt(listBox4.SelectedIndex);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //ROM Splitter
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Open ROM to split...";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                int SplitPart = 0;
                FileStream ROM_ = File.OpenRead(openFileDialog1.FileName);
                int PartsToDo = (int)(ROM_.Length / (440 * 126 + 430));
                int y = 0;
                FileStream ROMSplit = File.OpenWrite(openFileDialog1.FileName + "." + SplitPart.ToString() + ".bin");
                for (int x = 0; x < ROM_.Length; x++)
                {
                    ROMSplit.WriteByte((byte)ROM_.ReadByte());
                    y++;
                    if (x >= ROM_.Length)
                    {
                        ROMSplit.Close();
                        y = 0;
                    }
                    if (y == (440 * 126 + 430) && x < ROM_.Length)
                    {
                        y = 0;
                        ROMSplit.Close();
                        SplitPart++;
                        ROMSplit = File.OpenWrite(openFileDialog1.FileName + "." + SplitPart.ToString() + ".bin");
                    }
                }
            }
            MessageBox.Show("Split done.");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Open File to split to BSX Files...";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                if (SatellaWave.BSFile.MakeDownloadFile(openFileDialog1.FileName, textBox2.Text, numericUpDown15.Value, textBox10.Text))
                {
                    MessageBox.Show("Success!");
                }
                else
                {
                    MessageBox.Show("Error!");
                }
            /*
                int SplitPart = 0;
                FileStream ROM_ = File.OpenRead(openFileDialog1.FileName);
                int PartsToDo = (int)(ROM_.Length / 430);
                int FilesToMake = PartsToDo / 0x80;
                int y = 0;
                bool DoHeaderAgain = true;
                for (int x = 0; x < ROM_.Length; x++)
                {
                    FileStream BSTownFile = File.OpenWrite("BSX" + textBox2.Text + "." + SplitPart.ToString() + ".bin");
                    if (DoHeaderAgain)
                    {
                        BSTownFile.WriteByte((byte)numericUpDown15.Value);
                        BSTownFile.WriteByte((byte)(SplitPart & 0x7F));
                        BSTownFile.WriteByte(0);
                        UInt16 CurSize = (UInt16)(430 - 5);
                        if (PartsToDo == SplitPart)
                        {
                            CurSize = (UInt16)(ROM_.Length - x - 5);
                        }
                        BSTownFile.WriteByte((byte)(CurSize >> 8));
                        BSTownFile.WriteByte((byte)CurSize);
                        BSTownFile.WriteByte(1);
                        BSTownFile.WriteByte(1);
                        BSTownFile.WriteByte(0);
                        BSTownFile.WriteByte(0);
                        BSTownFile.WriteByte(0);
                    }
                    BSTownFile.WriteByte((byte)ROM_.ReadByte());
                    y++;
                    if (x >= ROM_.Length)
                    {
                        BSTownFile.Close();
                    }
                    if (y == 430)
                    {
                        y = 0;
                        BSTownFile.Close();
                        SplitPart++;
                        DoHeaderAgain = true;
                    }
             
                } */
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            listBox5.Items.Add(textBox1.Text + "/" + numericUpDown16.Value + "/" + comboBox14.Text + "/" + comboBox13.Text + "/" + textBox2.Text);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (listBox5.SelectedIndex != -1)
            {
                listBox5.Items.RemoveAt(listBox5.SelectedIndex);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = "";
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath != "")
            {
                textBox10.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
