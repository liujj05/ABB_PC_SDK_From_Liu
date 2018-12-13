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


namespace ControlPanel_ver01
{

    public partial class Form1 : Form
    {

        // 接收ABB信号需要定义的变量
        private RapidData RD_rt;
        private RapidData RD_time;
        private Num ABB_Num_time;
        private RobTarget ABB_RT_rt;
        private ABB.Robotics.Controllers.RapidDomain.Task tRob1;

        public ControllerInfoCollection controllers;
        private bool ABB_Connect_OK = false;
        // EndOf-接收ABB信号需要定义的变量

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
        }
    }
}
