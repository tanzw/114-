namespace QueryPlatform.Frm
{
    partial class fmQuickQuery
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataListView1 = new BrightIdeasSoftware.DataListView();
            ((System.ComponentModel.ISupportInitialize)(this.dataListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(554, 29);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(554, 15);
            this.panel1.TabIndex = 1;
            // 
            // dataListView1
            // 
            this.dataListView1.CellEditUseWholeCell = false;
            this.dataListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataListView1.DataSource = null;
            this.dataListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataListView1.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.dataListView1.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.dataListView1.Location = new System.Drawing.Point(0, 44);
            this.dataListView1.Name = "dataListView1";
            this.dataListView1.Size = new System.Drawing.Size(554, 211);
            this.dataListView1.TabIndex = 2;
            this.dataListView1.UseCompatibleStateImageBehavior = false;
            this.dataListView1.UseFilterIndicator = true;
            this.dataListView1.UseFiltering = true;
            this.dataListView1.View = System.Windows.Forms.View.Details;
            this.dataListView1.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.dataListView1_FormatRow);
            this.dataListView1.DoubleClick += new System.EventHandler(this.dataListView1_DoubleClick);
            this.dataListView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataListView1_KeyDown);
            // 
            // fmQuickQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 255);
            this.Controls.Add(this.dataListView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "fmQuickQuery";
            this.Text = "快速查询";
            this.Activated += new System.EventHandler(this.fmQuickQuery_Activated);
            this.Deactivate += new System.EventHandler(this.fmQuickQuery_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmQuickQuery_FormClosing);
            this.Load += new System.EventHandler(this.fmQuickQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataListView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private BrightIdeasSoftware.DataListView dataListView1;
    }
}