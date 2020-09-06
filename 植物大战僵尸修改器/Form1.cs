using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 植物大战僵尸修改器
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void txt_light_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(e.KeyChar == 8 || e.KeyChar >= '0' && e.KeyChar <= '9');
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, ref int buffer, int size, int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, ref int lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(int processAccess, bool bInheritHandle, int processId);

        [DllImport("user32.dll")]
        public static extern bool GetWindowThreadProcessId(int hWnd, ref int lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(int hObject);


        public const int PROCESS_ALL_ACCESS = 2035711;
        private int ppid;
        private int hwnd;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ppid = int.MinValue;
            hwnd = FindWindow(null, "植物大战僵尸中文版");
            if (hwnd != 0)
            {
                label1.Text = "已检测到游戏运行";
            }
            GetWindowThreadProcessId(hwnd, ref ppid); // 取与窗口关联的进程标识
            int hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, ppid); // 进程句柄
            if (checkBox2.Checked == true)
            {
                int esi = 0, edi = 0;
                // 基址(base_addr) 007794F8
                ReadProcessMemory(hProcess, 0x6a9ec0, ref esi, 4, 0);
                // mov edi,[esi+00000868]
                ReadProcessMemory(hProcess, esi + int.Parse("768", NumberStyles.HexNumber), ref edi, 4, 0);
                // mov [edi+00005578],esi
                int lightSize = Convert.ToInt32(textBox1.TextLength <= 0 ? "0" : textBox1.Text); // 阳光大小
                WriteProcessMemory(hProcess, edi + int.Parse("5560", NumberStyles.HexNumber), ref lightSize, 4, 0); // 修改阳光
            }
            CloseHandle(hProcess); // 关闭内核对象
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = false;
            ppid = int.MinValue;
            hwnd = FindWindow(null, "植物大战僵尸中文版");
            GetWindowThreadProcessId(hwnd, ref ppid); // 取与窗口关联的进程标识
            int hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, ppid); // 进程句柄
            int esi = 0, edi = 0;
            // 基址(base_addr) 007794F8
            ReadProcessMemory(hProcess, 0x6a9ec0, ref esi, 4, 0);
            // mov edi,[esi+00000868]
            ReadProcessMemory(hProcess, esi + int.Parse("768", NumberStyles.HexNumber), ref edi, 4, 0);
            // mov [edi+00005578],esi
            int lightSize = Convert.ToInt32(textBox1.TextLength <= 0 ? "0" : textBox1.Text); // 阳光大小
            WriteProcessMemory(hProcess, edi + int.Parse("5560", NumberStyles.HexNumber), ref lightSize, 4, 0); // 修改阳光
            CloseHandle(hProcess); // 关闭内核对象
            checkBox2.Checked = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                ppid = int.MinValue;
                hwnd = FindWindow(null, "植物大战僵尸中文版");
                GetWindowThreadProcessId(hwnd, ref ppid); // 取与窗口关联的进程标识
                int hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, ppid); // 进程句柄
                int a = 0x1477;
                WriteProcessMemory(hProcess, 0x00487296, ref a, 2, 0);
            }
        }
    }
}
