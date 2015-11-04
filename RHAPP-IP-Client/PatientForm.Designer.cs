namespace RHAPP_IP_Client
{
    partial class PatientForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.actualBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timeBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bpmChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.rpmChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.leeftijdBox1 = new System.Windows.Forms.TextBox();
            this.gewichtBox2 = new System.Windows.Forms.TextBox();
            this.geslachtComboBox2 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bpmChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpmChart)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(398, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(313, 11);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(75, 21);
            this.comboBox1.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(257, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Comport";
            // 
            // actualBox
            // 
            this.actualBox.Location = new System.Drawing.Point(172, 11);
            this.actualBox.Name = "actualBox";
            this.actualBox.ReadOnly = true;
            this.actualBox.Size = new System.Drawing.Size(75, 20);
            this.actualBox.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(132, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Watt";
            // 
            // timeBox
            // 
            this.timeBox.Location = new System.Drawing.Point(47, 11);
            this.timeBox.Name = "timeBox";
            this.timeBox.ReadOnly = true;
            this.timeBox.Size = new System.Drawing.Size(75, 20);
            this.timeBox.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Tijd";
            // 
            // bpmChart
            // 
            chartArea1.Name = "ChartArea1";
            this.bpmChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.bpmChart.Legends.Add(legend1);
            this.bpmChart.Location = new System.Drawing.Point(322, 38);
            this.bpmChart.Name = "bpmChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Hartslag";
            this.bpmChart.Series.Add(series1);
            this.bpmChart.Size = new System.Drawing.Size(300, 300);
            this.bpmChart.TabIndex = 12;
            this.bpmChart.Text = "chart2";
            // 
            // rpmChart
            // 
            chartArea2.Name = "ChartArea1";
            this.rpmChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.rpmChart.Legends.Add(legend2);
            this.rpmChart.Location = new System.Drawing.Point(16, 38);
            this.rpmChart.Name = "rpmChart";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "RPM";
            this.rpmChart.Series.Add(series2);
            this.rpmChart.Size = new System.Drawing.Size(300, 300);
            this.rpmChart.TabIndex = 8;
            this.rpmChart.Text = "chart1";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(499, 13);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(70, 17);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Ask Data";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.requestData_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(654, 229);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 21;
            this.button2.Text = "start test";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(651, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Zelf Invullen";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(651, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Leeftijd";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(651, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Gewicht";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(651, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Geslacht";
            // 
            // leeftijdBox1
            // 
            this.leeftijdBox1.Location = new System.Drawing.Point(707, 45);
            this.leeftijdBox1.Name = "leeftijdBox1";
            this.leeftijdBox1.Size = new System.Drawing.Size(100, 20);
            this.leeftijdBox1.TabIndex = 26;
            // 
            // gewichtBox2
            // 
            this.gewichtBox2.Location = new System.Drawing.Point(707, 78);
            this.gewichtBox2.Name = "gewichtBox2";
            this.gewichtBox2.Size = new System.Drawing.Size(100, 20);
            this.gewichtBox2.TabIndex = 27;
            // 
            // geslachtComboBox2
            // 
            this.geslachtComboBox2.FormattingEnabled = true;
            this.geslachtComboBox2.Items.AddRange(new object[] {
            "Man",
            "Vrouw"});
            this.geslachtComboBox2.Location = new System.Drawing.Point(707, 111);
            this.geslachtComboBox2.Name = "geslachtComboBox2";
            this.geslachtComboBox2.Size = new System.Drawing.Size(100, 21);
            this.geslachtComboBox2.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(657, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Hou 60 RPM aan";
            // 
            // PatientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 344);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.geslachtComboBox2);
            this.Controls.Add(this.gewichtBox2);
            this.Controls.Add(this.leeftijdBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.actualBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.timeBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bpmChart);
            this.Controls.Add(this.rpmChart);
            this.Name = "PatientForm";
            this.Text = "PatientForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PatientForm_FormClosing);
            this.Load += new System.EventHandler(this.PatientForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bpmChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpmChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox timeBox;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.DataVisualization.Charting.Chart bpmChart;
        public System.Windows.Forms.DataVisualization.Charting.Chart rpmChart;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox actualBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox leeftijdBox1;
        public System.Windows.Forms.TextBox gewichtBox2;
        public System.Windows.Forms.ComboBox geslachtComboBox2;
        private System.Windows.Forms.Label label8;
    }
}