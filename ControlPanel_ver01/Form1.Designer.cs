namespace ControlPanel_ver01
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Image_res = new System.Windows.Forms.PictureBox();
            this.Btn_Test = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listView_Log = new System.Windows.Forms.ListView();
            this.From = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.To = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Brief = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_Gocator = new System.Windows.Forms.Button();
            this.timer_MonitorGocator = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Image_res)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Image_res
            // 
            this.Image_res.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Image_res.Location = new System.Drawing.Point(3, 3);
            this.Image_res.Name = "Image_res";
            this.Image_res.Size = new System.Drawing.Size(758, 494);
            this.Image_res.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Image_res.TabIndex = 0;
            this.Image_res.TabStop = false;
            // 
            // Btn_Test
            // 
            this.Btn_Test.Location = new System.Drawing.Point(867, 529);
            this.Btn_Test.Name = "Btn_Test";
            this.Btn_Test.Size = new System.Drawing.Size(94, 44);
            this.Btn_Test.TabIndex = 1;
            this.Btn_Test.Text = "Test";
            this.Btn_Test.UseVisualStyleBackColor = true;
            this.Btn_Test.Click += new System.EventHandler(this.Btn_Test_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Image_res, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Btn_Test, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.listView_Log, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.button_Gocator, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1064, 577);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // listView_Log
            // 
            this.listView_Log.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.From,
            this.To,
            this.Brief});
            this.tableLayoutPanel1.SetColumnSpan(this.listView_Log, 3);
            this.listView_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Log.Location = new System.Drawing.Point(767, 3);
            this.listView_Log.Name = "listView_Log";
            this.tableLayoutPanel1.SetRowSpan(this.listView_Log, 2);
            this.listView_Log.Size = new System.Drawing.Size(294, 520);
            this.listView_Log.TabIndex = 2;
            this.listView_Log.UseCompatibleStateImageBehavior = false;
            this.listView_Log.View = System.Windows.Forms.View.Details;
            // 
            // From
            // 
            this.From.Text = "From";
            // 
            // To
            // 
            this.To.Text = "To";
            // 
            // Brief
            // 
            this.Brief.Text = "Brief";
            // 
            // button_Gocator
            // 
            this.button_Gocator.Location = new System.Drawing.Point(767, 529);
            this.button_Gocator.Name = "button_Gocator";
            this.button_Gocator.Size = new System.Drawing.Size(94, 45);
            this.button_Gocator.TabIndex = 3;
            this.button_Gocator.Text = "Gocator";
            this.button_Gocator.UseVisualStyleBackColor = true;
            this.button_Gocator.Click += new System.EventHandler(this.button_Gocator_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 577);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Image_res)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Image_res;
        private System.Windows.Forms.Button Btn_Test;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView listView_Log;
        private System.Windows.Forms.ColumnHeader From;
        private System.Windows.Forms.ColumnHeader To;
        private System.Windows.Forms.ColumnHeader Brief;
        private System.Windows.Forms.Button button_Gocator;
        private System.Windows.Forms.Timer timer_MonitorGocator;
    }
}

