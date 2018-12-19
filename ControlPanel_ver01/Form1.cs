using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

// ABB 通信
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;

// 与C++通信
using System.Runtime.InteropServices;

// Gocator 通信
using Lmi3d.GoSdk;
using Lmi3d.Zen;
using Lmi3d.Zen.Io;
using Lmi3d.GoSdk.Messages;

// 文件读取
using System.IO;

// 内存映射
using System.IO.MemoryMappedFiles;

// 针对对话框，之前不能放别的类

namespace ControlPanel_ver01
{
    public partial class Form1 : Form
    {
        // =========变量定义=========
        // 接收ABB信号需要定义的变量
        private RapidData RD_rt;
        private RapidData RD_time;
        private Num ABB_Num_time;
        private RobTarget ABB_RT_rt;
        private ABB.Robotics.Controllers.RapidDomain.Task tRob1;

        public ControllerInfoCollection controllers;
        private bool ABB_Connect_OK = false;

        // EndOf-接收ABB信号需要定义的变量

        // Gocator 通信
        private const string SENSOR_IP = "192.168.1.10";
        public GoSystem system;
        public GoSensor sensor;

        public static void onData(KObject data)
        {
            GoDataSet dataSet = (GoDataSet)data;
            for (UInt32 i = 0; i < dataSet.Count; i++)
            {
                GoDataMsg dataObj = (GoDataMsg)dataSet.Get(i);
                switch (dataObj.MessageType)
                {
                    case GoDataMessageType.Stamp:
                        {
                            GoStampMsg stampMsg = (GoStampMsg)dataObj;
                            for (UInt32 j = 0; j < stampMsg.Count; j++)
                            {
                                GoStamp stamp = stampMsg.Get(j);
                                Console.WriteLine("Frame Index = {0}", stamp.FrameIndex);
                                Console.WriteLine("Time Stamp = {0}", stamp.Timestamp);
                                Console.WriteLine("Encoder Value = {0}", stamp.Encoder);
                            }
                        }
                        break;
                }
            }
            // Refer to example ReceiveRange, ReceiveProfile, ReceiveMeasurement and ReceiveWholePart on how to receive data
            Console.WriteLine(Environment.NewLine);
        }
        // Endof-Gocator 通信

        // =C++通信=
        public string strDlgTitle = "CoreAlgorithm"; // 对方窗口
        IntPtr hwndRecvWindow = IntPtr.Zero; // 接收窗口指针
        IntPtr hwndSendWindow = IntPtr.Zero; // 发送窗口指针
        double[] db_zs;
        IntPtr db_ptr;
        bool If_Init_WM_COPY = false;

        // 接收函数
        bool If_received = false; // 流程控制用，接到C++反馈信息后由接收函数写为true
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case MyMsg.WM_USR_RECEIVED:
                    If_received = true;
                    break;
            }
            base.WndProc(ref m);
        }
        // =Endof C++通信=

        // 内存映射
        const string memo_file_name = "data_gc";    // 映射文件名
        const long memo_capacity = 0x800000;        // 映射段大小
        MemoryMappedFile mmf;
        MemoryMappedViewAccessor accessor;
        bool If_Init_mmf = false;
        // Endof-内存映射



        // =======END of 变量定义=======
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 主窗口大小、位置
            int Desk_width = Screen.PrimaryScreen.WorkingArea.Width;
            int Desk_height = Screen.PrimaryScreen.WorkingArea.Height;
            Width = Desk_width;
            Height = Desk_height;
            Location = new Point(0, 0);

            // 尝试连接ABB
            NetworkScanner networkScanner = new NetworkScanner();
            networkScanner.Scan();
            controllers = networkScanner.Controllers;
            foreach (ControllerInfo info in controllers)
            {
                ListViewItem item = new ListViewItem("PC");
                item.SubItems.Add("ABB");
                item.SubItems.Add("Detected");
                item.Tag = info;
                listView_Log.Items.Add(item);
                ABB_Connect_OK = true;
            }

            

            // 初始化内存映射
            if (If_Init_mmf == false)
            {
                try
                {
                    mmf = MemoryMappedFile.CreateNew(memo_file_name, memo_capacity);
                }
                catch (Exception Memo_e)
                {
                    Console.WriteLine("{0} Exception caught.", Memo_e);
                    return;
                }

                try
                {
                    accessor = mmf.CreateViewAccessor();
                }
                catch (Exception Memo_e)
                {
                    Console.WriteLine("{0} Exception caught.", Memo_e);
                    return;
                }
                If_Init_mmf = true;
            }
            // Endof-初始化内存映射


            

            

            


        }// - Endof - Form1Load

        private void Btn_Test_Click(object sender, EventArgs e)
        {
            // 初始化消息发送
            if (If_Init_WM_COPY == false)
            {
                hwndRecvWindow = ImportFromDLL.FindWindow(null, strDlgTitle);
                if (hwndRecvWindow == IntPtr.Zero)
                {
                    Console.WriteLine("请先启动接收消息程序"); // 这个接口可能要修改，程序需要自动启动
                    return;
                }
                hwndSendWindow = ImportFromDLL.GetForegroundWindow();
                if (hwndSendWindow == IntPtr.Zero)
                {
                    Console.WriteLine("获取自己的窗口句柄失败，请重试");
                    return;
                }
                If_Init_WM_COPY = true;
            }
            // Enfof-初始化消息发送

            // 1. z方向轮廓数据
            string file_path
                = @"C:\Users\jiajun\SynologyDrive\005-LeiMu_GangJin\p01-C++\Gocater——data\New_Data\Profile_Z";
            List<double> db_list_z = new List<double>();
            if (File.Exists(file_path))
            {
                using (StreamReader sr = File.OpenText(file_path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        db_list_z.Add(double.Parse(s));
                    }
                }
            }
            else
                return;
            // 尝试向共享内存中写入数据
            double[] db_array_z = db_list_z.ToArray();
            accessor.WriteArray<double>(0, db_array_z, 0, db_array_z.Length);
            // 向C++发送消息，通知读取
            If_received = false;

            ImportFromDLL.COPYDATASTRUCT copydata;
            copydata.dwData = (IntPtr)1;            // 表明是z
            copydata.cbData = db_array_z.Length;
            copydata.lpData = IntPtr.Zero;
            ImportFromDLL.SendMessage(hwndRecvWindow, ImportFromDLL.WM_COPYDATA, hwndSendWindow, ref copydata);

            // 等待C++返回消息，读取完成

            while (If_received == false)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Waiting for received...");
            }

            db_array_z = null;

            // 2. x 方向轮廓数据
            file_path
                = @"C:\Users\jiajun\SynologyDrive\005-LeiMu_GangJin\p01-C++\Gocater——data\New_Data\Profile_X";
            List<double> db_list_x = new List<double>();
            if (File.Exists(file_path))
            {
                using (StreamReader sr = File.OpenText(file_path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        db_list_x.Add(double.Parse(s));
                    }
                }
            }
            else
                return;

            double[] db_array_x = db_list_x.ToArray();
            accessor.WriteArray<double>(0, db_array_x, 0, db_array_x.Length);
            If_received = false;

            copydata.dwData = (IntPtr)2;            // 表明是x
            copydata.cbData = db_list_x.Count;
            copydata.lpData = IntPtr.Zero;
            ImportFromDLL.SendMessage(hwndRecvWindow, ImportFromDLL.WM_COPYDATA, hwndSendWindow, ref copydata);

            while (If_received == false)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Waiting for received...");
            }

            db_array_x = null;

            // 3. 时间戳
            file_path
                = @"C:\Users\jiajun\SynologyDrive\005-LeiMu_GangJin\p01-C++\Gocater——data\New_Data\TimeStamp";

            List<ulong> ulTimeStamp = new List<ulong>();
            if (File.Exists(file_path))
            {
                using (StreamReader sr = File.OpenText(file_path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        //ulTimeStamp.Add(ulong.Parse(s));
                        ulTimeStamp.Add(ulong.Parse(s));
                    }
                }
            }
            else
                return;
            ulong[] ulong_timestamps = ulTimeStamp.ToArray();
            accessor.WriteArray<ulong>(0, ulong_timestamps, 0, ulong_timestamps.Length);
            If_received = false;


            copydata.dwData = (IntPtr)3;            // 表明是 time stamp
            copydata.cbData = ulTimeStamp.Count;
            copydata.lpData = IntPtr.Zero;
            ImportFromDLL.SendMessage(hwndRecvWindow, ImportFromDLL.WM_COPYDATA, hwndSendWindow, ref copydata);
            while (If_received == false)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Waiting for received...");
            }

            copydata.dwData = (IntPtr)4;            // 表明是 要求退出
            copydata.cbData = 0;
            copydata.lpData = IntPtr.Zero;
            ImportFromDLL.SendMessage(hwndRecvWindow, ImportFromDLL.WM_COPYDATA, hwndSendWindow, ref copydata);

            return;
        }

        private void button_Gocator_Click(object sender, EventArgs e)
        {
            // 进行Gocator初始化的相关工作
            KApiLib.Construct();
            GoSdkLib.Construct();
            system = new GoSystem();
            KIpAddress ipAddress = KIpAddress.Parse(SENSOR_IP);
            sensor = system.FindSensorByIpAddress(ipAddress);
            GoSetup sensor_setup = sensor.Setup;
            sensor.Connect();
            system.EnableData(true);
            system.SetDataHandler(onData);
            // 以下代码需要Debug
            sensor_setup.TriggerGateEnabled = true; // 设定值
            bool Trigger_gate_bool_flag1 = false;
            bool Trigger_gate_bool_flag2 = false;
            Trigger_gate_bool_flag1 = sensor_setup.TriggerGateEnabledSystemValue; // 读取值1
            Trigger_gate_bool_flag2 = sensor_setup.TriggerGateEnabledUsed; // 读取值2
            // 以上代码需要Debug
            system.Start(); // Start() 之后，在对话框情况下其实我们可以启动一个定时器去读Gocator的
            
            // 启动定时器 - 以下两个方法等效
            // timer_MonitorGocator.Enabled = true;
            timer_MonitorGocator.Start();
        }

        private void MonitorGocator_Tick(object sender, EventArgs e)
        {
            // 该函数由定时器配置，周期性执行
            // 主要功能是查询Gocator是否仍有数据
        }

        public class ImportFromDLL
        {
            public const int WM_COPYDATA = 0x004A;
            public const int WM_QUIT = 0x0012;
            //启用非托管代码   
            [StructLayout(LayoutKind.Sequential)]
            public struct COPYDATASTRUCT
            {
                public IntPtr dwData;    //not used   
                public int cbData;    //长度   
                // [MarshalAs(UnmanagedType.LPStr)] // 例程用
                // public string lpData; // 例程用
                public IntPtr lpData; // 本程序用
            }

            [DllImport("User32.dll")]
            public static extern int SendMessage(
                IntPtr hWnd,     // handle to destination window    
                int Msg,         // message   
                IntPtr wParam,    // first message parameter    
                ref COPYDATASTRUCT pcd // second message parameter    
            );

            [DllImport("User32.dll", EntryPoint = "FindWindow")]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("Kernel32.dll", EntryPoint = "GetConsoleWindow")]
            public static extern IntPtr GetConsoleWindow();

            [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
            public static extern IntPtr GetForegroundWindow();
        }

        public class MyMsg
        {
            public const int WM_USR_RECEIVED = 0x400 + 2;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 释放资源用
            accessor.Dispose();
            mmf.Dispose();
        }
    }
}
