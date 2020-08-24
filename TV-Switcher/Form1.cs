using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TV_Switcher
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, int Msg, IntPtr wParm, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [StructLayout(LayoutKind.Sequential)]
        struct COPYDATASTRUCT
        {
            public IntPtr dwData;    // Any value the sender chooses.  Perhaps its main window handle?
            public int cbData;       // The count of bytes in the message.
            public IntPtr lpData;    // The address of the message.
        }
         
        const int WM_COPYDATA = 0x004A;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct MYSTRUCT
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 127)]
            public string mystring;
        }

        MYSTRUCT mystruct;

        Random Rnd = new Random();

        int RndTime = 0;
        int RndChannel = 0;
        int ChannelMax = 43;
        int TimeMin = 15;
        int TimeMax = 60;
        
        bool RunStatus = false;

        // Allocate a pointer to an arbitrary structure on the global heap.
        public static IntPtr IntPtrAlloc<T>(T param)
        {
            IntPtr retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, retval, false);
            return retval;
        }

        // Free a pointer to an arbitrary structure from the global heap.
        public static void IntPtrFree(ref IntPtr preAllocated)
        {
            if (IntPtr.Zero == preAllocated)
                throw (new NullReferenceException("Go Home"));
            Marshal.FreeHGlobal(preAllocated);
            preAllocated = IntPtr.Zero;
        }

        public Form1()
        {
            InitializeComponent();
        }
 
        private void button1_Click(object sender, EventArgs e)
        {
            //Start gedrückt:
            if (!RunStatus)
            {
                RunStatus = true;
                //1. Zufälliges Programm ermitteln
                RndChannel = Rnd.Next(1, ChannelMax);
                //3. Timer mit zufälliger Zahl stellen
                RndTime = Rnd.Next(TimeMin, TimeMax);
                //2. senden (Programmnummer)
                textBox1.Text = Convert.ToString(RndChannel);
                sendChannel(RndChannel);
            }
            else
            {
                //do nothing
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Timer abgelaufen (1sec.)
            if (RunStatus)
            {
                if (RndTime == 0)
                {
                    //1. Zufälliges Programm ermitteln
                    RndChannel = Rnd.Next(1, ChannelMax);
                    //3. Timer mit zufälliger Zahl stellen
                    RndTime = Rnd.Next(TimeMin, TimeMax);
                    //2. senden (Programmnumer)
                    textBox1.Text = Convert.ToString(RndChannel);
                    sendChannel(RndChannel);
                }
                else
                {
                    RndTime--;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Stop gedrückt
            RunStatus = false;
        }

        private void sendChannel(int ChannelNumber)
        {
            if (ChannelNumber <= 9)
            {
                irsend(ChannelNumber);
                System.Threading.Thread.Sleep(500);
            }
            else
            {
                if (ChannelNumber >=10 && ChannelNumber <= 99)
                {
                    irsend(ChannelNumber % 100 / 10);
                    System.Threading.Thread.Sleep(500);
                    irsend(ChannelNumber % 10);
                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    
                   
                    irsend(ChannelNumber % 1000 / 100);
                    System.Threading.Thread.Sleep(500);
                    irsend(ChannelNumber % 100 / 10);
                    System.Threading.Thread.Sleep(500);
                    irsend(ChannelNumber % 10);
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        private void irsend(int ChannelDigit)
        {
            mystruct.mystring = "LED-Controller-#1 "+Convert.ToString(ChannelDigit)+" 0";

            IntPtr hWnd = (IntPtr)FindWindow(null,"WinLirc");

            if (hWnd!=IntPtr.Zero)
		    {
                //MessageBox.Show("irsend da");
                IntPtr buffer = IntPtrAlloc(mystruct);
			    COPYDATASTRUCT cpd = new COPYDATASTRUCT();
                cpd.dwData = IntPtr.Zero;
			    cpd.cbData = (Marshal.SizeOf(mystruct)+1);
			    cpd.lpData = buffer;
                IntPtr copyDataBuff = IntPtrAlloc(cpd);
                SendMessage(hWnd, WM_COPYDATA, GetModuleHandle(null), copyDataBuff);
                IntPtrFree(ref copyDataBuff);
                IntPtrFree(ref buffer);
                
            }
		    else
		    {
                    MessageBox.Show("irsend: Error! WinLIRC not running?"); //Error
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            TimeMin = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            TimeMax = (int)numericUpDown2.Value;
        }
        
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            ChannelMax = (int)numericUpDown3.Value;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            sendChannel(1);
        }
 
        private void button4_Click(object sender, EventArgs e)
        {
            sendChannel(2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sendChannel(3);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sendChannel(4);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            sendChannel(5);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            sendChannel(6);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            sendChannel(7);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            sendChannel(8);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            sendChannel(9);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            sendChannel(0);
        }

        private void button15_Click_1(object sender, EventArgs e)
        {   //RND
            //1. Zufälliges Programm ermitteln
            RndChannel = Rnd.Next(1, ChannelMax);
            //3. Timer mit zufälliger Zahl stellen
            textBox1.Text = Convert.ToString(RndChannel);
            sendChannel(RndChannel);
        }
    }
}
