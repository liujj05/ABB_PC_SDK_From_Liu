// ABB 通信
namespace ControlPanel_ver01
{
    partial class Form_ReadABB_Vars
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox_Var = new System.Windows.Forms.ComboBox();
            this.comboBox_Module = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1_Result = new System.Windows.Forms.TextBox();
            this.button_Read = new System.Windows.Forms.Button();
            this.textBox_varName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox_Var
            // 
            this.comboBox_Var.FormattingEnabled = true;
            this.comboBox_Var.Items.AddRange(new object[] {
            "Num",
            "RobTarget"});
            this.comboBox_Var.Location = new System.Drawing.Point(136, 75);
            this.comboBox_Var.Name = "comboBox_Var";
            this.comboBox_Var.Size = new System.Drawing.Size(121, 26);
            this.comboBox_Var.TabIndex = 0;
            // 
            // comboBox_Module
            // 
            this.comboBox_Module.FormattingEnabled = true;
            this.comboBox_Module.Items.AddRange(new object[] {
            "MainModule"});
            this.comboBox_Module.Location = new System.Drawing.Point(136, 23);
            this.comboBox_Module.Name = "comboBox_Module";
            this.comboBox_Module.Size = new System.Drawing.Size(121, 26);
            this.comboBox_Module.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Module Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Var Type";
            // 
            // textBox1_Result
            // 
            this.textBox1_Result.Location = new System.Drawing.Point(263, 23);
            this.textBox1_Result.Multiline = true;
            this.textBox1_Result.Name = "textBox1_Result";
            this.textBox1_Result.Size = new System.Drawing.Size(525, 266);
            this.textBox1_Result.TabIndex = 4;
            // 
            // button_Read
            // 
            this.button_Read.Location = new System.Drawing.Point(26, 244);
            this.button_Read.Name = "button_Read";
            this.button_Read.Size = new System.Drawing.Size(122, 45);
            this.button_Read.TabIndex = 5;
            this.button_Read.Text = "Read";
            this.button_Read.UseVisualStyleBackColor = true;
            this.button_Read.Click += new System.EventHandler(this.button_Read_Click);
            // 
            // textBox_varName
            // 
            this.textBox_varName.Location = new System.Drawing.Point(136, 130);
            this.textBox_varName.Name = "textBox_varName";
            this.textBox_varName.Size = new System.Drawing.Size(121, 28);
            this.textBox_varName.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Var Name";
            // 
            // Form_ReadABB_Vars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_varName);
            this.Controls.Add(this.button_Read);
            this.Controls.Add(this.textBox1_Result);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_Module);
            this.Controls.Add(this.comboBox_Var);
            this.Name = "Form_ReadABB_Vars";
            this.Text = "Form_ReadABB_Vars";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_Var;
        private System.Windows.Forms.ComboBox comboBox_Module;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1_Result;
        private System.Windows.Forms.Button button_Read;
        private System.Windows.Forms.TextBox textBox_varName;
        private System.Windows.Forms.Label label3;
    }
}