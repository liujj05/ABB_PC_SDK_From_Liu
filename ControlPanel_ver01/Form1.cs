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
    }
}
