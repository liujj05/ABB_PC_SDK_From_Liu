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
        private Task tRob1;
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
            ControllerInfoCollection controllers = networkScanner.Controllers;
            foreach (ControllerInfo info in controllers)
            {
                ListViewItem item = new ListViewItem("PC");
                item.SubItems.Add("ABB");
                item.SubItems.Add("Detected");
                item.Tag = info;
                listView_Log.Items.Add(item);
            }
        }

        private void Btn_Test_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            string pathname = string.Empty;
            if (file.FileName != string.Empty)
            {
                try
                {
                    pathname = file.FileName;
                    Image_res.Load(pathname);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
