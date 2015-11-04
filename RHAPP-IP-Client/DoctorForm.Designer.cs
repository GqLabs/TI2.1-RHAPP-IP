namespace RHAPP_IP_Client
{
    partial class DoctorForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.crtRPM = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblWatt = new System.Windows.Forms.Label();
            this.crtPower = new System.Windows.Forms.NumericUpDown();
            this.btnSetPower = new System.Windows.Forms.Button();
            this.crtPulse = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblTijd = new System.Windows.Forms.Label();
            this.tbTime = new System.Windows.Forms.TextBox();
            this.btnStartTest = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnTestButton = new System.Windows.Forms.Button();
            this.lblUser = new System.Windows.Forms.Label();
            this.cmbOnlinePatients = new System.Windows.Forms.ComboBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.crtRPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.crtPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.crtPulse)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // crtRPM
            // 
            chartArea1.Name = "ChartArea1";
            this.crtRPM.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.crtRPM.Legends.Add(legend1);
            this.crtRPM.Location = new System.Drawing.Point(248, 6);
            this.crtRPM.Name = "crtRPM";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "RPM";
            this.crtRPM.Series.Add(series1);
            this.crtRPM.Size = new System.Drawing.Size(300, 300);
            this.crtRPM.TabIndex = 0;
            this.crtRPM.Text = "chart1";
            // 
            // lblWatt
            // 
            this.lblWatt.AutoSize = true;
            this.lblWatt.Location = new System.Drawing.Point(6, 36);
            this.lblWatt.Name = "lblWatt";
            this.lblWatt.Size = new System.Drawing.Size(30, 13);
            this.lblWatt.TabIndex = 1;
            this.lblWatt.Text = "Watt";
            // 
            // crtPower
            // 
            this.crtPower.Increment = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.crtPower.Location = new System.Drawing.Point(52, 34);
            this.crtPower.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.crtPower.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.crtPower.Name = "crtPower";
            this.crtPower.Size = new System.Drawing.Size(103, 20);
            this.crtPower.TabIndex = 2;
            this.crtPower.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // btnSetPower
            // 
            this.btnSetPower.Location = new System.Drawing.Point(160, 31);
            this.btnSetPower.Name = "btnSetPower";
            this.btnSetPower.Size = new System.Drawing.Size(57, 23);
            this.btnSetPower.TabIndex = 3;
            this.btnSetPower.Text = "Set";
            this.btnSetPower.UseVisualStyleBackColor = true;
            this.btnSetPower.Click += new System.EventHandler(this.btnSetPower_Click);
            // 
            // crtPulse
            // 
            chartArea2.Name = "ChartArea1";
            this.crtPulse.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.crtPulse.Legends.Add(legend2);
            this.crtPulse.Location = new System.Drawing.Point(554, 7);
            this.crtPulse.Name = "crtPulse";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Hartslag";
            this.crtPulse.Series.Add(series2);
            this.crtPulse.Size = new System.Drawing.Size(300, 300);
            this.crtPulse.TabIndex = 4;
            this.crtPulse.Text = "chart2";
            // 
            // lblTijd
            // 
            this.lblTijd.AutoSize = true;
            this.lblTijd.Location = new System.Drawing.Point(6, 63);
            this.lblTijd.Name = "lblTijd";
            this.lblTijd.Size = new System.Drawing.Size(24, 13);
            this.lblTijd.TabIndex = 5;
            this.lblTijd.Text = "Tijd";
            // 
            // tbTime
            // 
            this.tbTime.Location = new System.Drawing.Point(52, 60);
            this.tbTime.Name = "tbTime";
            this.tbTime.ReadOnly = true;
            this.tbTime.Size = new System.Drawing.Size(103, 20);
            this.tbTime.TabIndex = 6;
            // 
            // btnStartTest
            // 
            this.btnStartTest.Location = new System.Drawing.Point(52, 86);
            this.btnStartTest.Name = "btnStartTest";
            this.btnStartTest.Size = new System.Drawing.Size(57, 23);
            this.btnStartTest.TabIndex = 7;
            this.btnStartTest.Text = "Start test";
            this.btnStartTest.UseVisualStyleBackColor = true;
            this.btnStartTest.Click += new System.EventHandler(this.btnStartTest_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(0, 0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(200, 100);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "tabPage2";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(857, 331);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnTestButton);
            this.tabPage1.Controls.Add(this.lblUser);
            this.tabPage1.Controls.Add(this.cmbOnlinePatients);
            this.tabPage1.Controls.Add(this.crtRPM);
            this.tabPage1.Controls.Add(this.btnStartTest);
            this.tabPage1.Controls.Add(this.lblWatt);
            this.tabPage1.Controls.Add(this.tbTime);
            this.tabPage1.Controls.Add(this.crtPower);
            this.tabPage1.Controls.Add(this.lblTijd);
            this.tabPage1.Controls.Add(this.btnSetPower);
            this.tabPage1.Controls.Add(this.crtPulse);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(849, 305);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Test";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // btnTestButton
            // 
            this.btnTestButton.Location = new System.Drawing.Point(52, 125);
            this.btnTestButton.Name = "btnTestButton";
            this.btnTestButton.Size = new System.Drawing.Size(75, 23);
            this.btnTestButton.TabIndex = 10;
            this.btnTestButton.Text = "Test button";
            this.btnTestButton.UseVisualStyleBackColor = true;
            this.btnTestButton.Click += new System.EventHandler(this.btnTestButton_Click);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(6, 10);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(40, 13);
            this.lblUser.TabIndex = 9;
            this.lblUser.Text = "Patient";
            // 
            // cmbOnlinePatients
            // 
            this.cmbOnlinePatients.FormattingEnabled = true;
            this.cmbOnlinePatients.Location = new System.Drawing.Point(52, 7);
            this.cmbOnlinePatients.Name = "cmbOnlinePatients";
            this.cmbOnlinePatients.Size = new System.Drawing.Size(103, 21);
            this.cmbOnlinePatients.TabIndex = 8;
            this.cmbOnlinePatients.SelectionChangeCommitted += new System.EventHandler(this.cmbOnlinePatients_SelectionChangeCommitted);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.comboBox1);
            this.tabPage3.Controls.Add(this.textBox5);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.textBox3);
            this.tabPage3.Controls.Add(this.textBox2);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(849, 305);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Historie";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(59, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 8;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(59, 85);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "VO2-Max";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(59, 33);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(59, 59);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Naam";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Gewicht";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Leeftijd";
            // 
            // DoctorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 331);
            this.Controls.Add(this.tabControl1);
            this.Name = "DoctorForm";
            this.Text = "Doctor Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DoctorForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.crtRPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.crtPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.crtPulse)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart crtRPM;
        private System.Windows.Forms.Label lblWatt;
        private System.Windows.Forms.NumericUpDown crtPower;
        private System.Windows.Forms.Button btnSetPower;
        private System.Windows.Forms.DataVisualization.Charting.Chart crtPulse;
        private System.Windows.Forms.Label lblTijd;
        private System.Windows.Forms.TextBox tbTime;
        private System.Windows.Forms.Button btnStartTest;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.ComboBox cmbOnlinePatients;
        private System.Windows.Forms.Button btnTestButton;
    }
}

