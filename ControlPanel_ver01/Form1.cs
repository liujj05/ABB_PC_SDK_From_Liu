using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        // =Endof C++通信=

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

            // 初始化C++接口
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

        }

        private void Btn_Test_Click(object sender, EventArgs e)
        {
            // ver02 读取ABB中特定形式的变量-这时需要另外一个窗体
            foreach (Form fm in Application.OpenForms)
            {
                if (fm.Name == "Form_ReadABB_Vars")
                {
                    fm.WindowState = FormWindowState.Normal;
                    fm.Activate();
                    return;
                }
            }
            Form fm_ABB = new Form_ReadABB_Vars(this);
            fm_ABB.Show();

            // ver2.5 读取出的RobTarget的格式是：
            // 一个大中括号套着四个小中括号，每个小中括号中又有不同的文字
            // 我们需要依次提取所有的数字。
            string str_robTarget = 
                "[[555,-180.01,659.99],[0.719836,-0.00122566,0.694142,0.00106312],[-1,-1,0,1],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]]";
            string[] str_rt_split = str_robTarget.Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries );
            // 单机器人只要第一、第二段数据就够了
            string str_pos = str_rt_split[0].Substring(2); // 前两个字符"[["不要
            string str_rot = str_rt_split[1];
            string[] str_pos_xyz = str_pos.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] str_rot_qua = str_rot.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            double[] db_pos = new double[3];
            for (int i=0; i<3; i++)
            {
                db_pos[i] = double.Parse(str_pos_xyz[i]);
            }
            double[] db_qua = new double[4];
            for (int i = 0; i < 4; i++)
            {
                db_qua[i] = double.Parse(str_rot_qua[i]);
            }
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
                public int dwData;    //not used   
                public int cbData;    //长度   
                // [MarshalAs(UnmanagedType.LPStr)] // 例程用
                // public string lpData; // 例程用
                public int lpData; // 本程序用
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
    }
}
