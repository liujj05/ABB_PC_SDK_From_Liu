//#define ROBOT_EXIST

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;



namespace ControlPanel_ver01
{

    public partial class Form_ReadABB_Vars : Form
    {

        Form1 main_form = new Form1();
        private RapidData RD_rt;
        private RapidData RD_time;
        private Num ABB_Num_time;
        private RobTarget ABB_RT_rt;
        private ABB.Robotics.Controllers.RapidDomain.Task tRob1;

        
        public Form_ReadABB_Vars(Form1 f1)
        {
            InitializeComponent();
            main_form = f1;
        }

        private void button_Read_Click(object sender, EventArgs e)
        {
#if ROBOT_EXIST
            ControllerInfo controllerInfo = main_form.controllers[0]; // 我们很自信，就一个控制器，以后有多个的话，这里肯定需要修改
            Controller ctrl = ControllerFactory.CreateFrom(controllerInfo);
            ctrl.Logon(UserInfo.DefaultUser);
            tRob1 = ctrl.Rapid.GetTask("T_ROB1");
#endif
            string module_name = comboBox_Module.SelectedItem.ToString();
            string var_type = comboBox_Var.SelectedItem.ToString();
            int var_type_no = comboBox_Var.SelectedIndex;
            string var_name = textBox_varName.Text;
#if ROBOT_EXIST
            string str_read_res = "";
            switch (var_type)
            {
                case "Num":
                    RD_time = tRob1.GetRapidData(module_name, var_name);
                    ABB_Num_time = (Num)RD_time.Value;
                    str_read_res = ABB_Num_time.ToString();
                    break;
                case "RobTarget":
                    RD_rt = tRob1.GetRapidData(module_name, var_name);
                    ABB_RT_rt = (RobTarget)RD_rt.Value;
                    str_read_res = ABB_RT_rt.ToString();
                    break;
                default:
                    break;
            }
            textBox1_Result.Text = str_read_res;
            textBox1_Result.Update();
            ctrl.Logoff();
            ctrl.Dispose();
#else
            switch (var_type)
            {
                case "Num":
                    textBox1_Result.Text = "Num";
                    break;
                case "RobTarget":
                    textBox1_Result.Text = "RobTarget";
                    break;
                default:
                    break;
            }
            textBox1_Result.Update();
#endif
        }
    }
}
