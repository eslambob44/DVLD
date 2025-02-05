namespace DVLD_PresentationLayer.Licenses
{
    partial class frmManageLicenses
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManageLicenses));
            this.gbDriverLicense = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpLocalLicenses = new System.Windows.Forms.TabPage();
            this.tpInternationalLicenses = new System.Windows.Forms.TabPage();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvLocalLicensesHistory = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.lblLocalRecords = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ctrlFilterPerson1 = new DVLD_PresentationLayer.People.ctrlFilterPerson();
            this.gbDriverLicense.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpLocalLicenses.SuspendLayout();
            this.tpInternationalLicenses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocalLicensesHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbDriverLicense
            // 
            this.gbDriverLicense.Controls.Add(this.tabControl1);
            this.gbDriverLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDriverLicense.Location = new System.Drawing.Point(5, 501);
            this.gbDriverLicense.Name = "gbDriverLicense";
            this.gbDriverLicense.Size = new System.Drawing.Size(1183, 313);
            this.gbDriverLicense.TabIndex = 1;
            this.gbDriverLicense.TabStop = false;
            this.gbDriverLicense.Text = "Driver License";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpLocalLicenses);
            this.tabControl1.Controls.Add(this.tpInternationalLicenses);
            this.tabControl1.Location = new System.Drawing.Point(3, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1174, 274);
            this.tabControl1.TabIndex = 0;
            // 
            // tpLocalLicenses
            // 
            this.tpLocalLicenses.BackColor = System.Drawing.Color.White;
            this.tpLocalLicenses.Controls.Add(this.lblLocalRecords);
            this.tpLocalLicenses.Controls.Add(this.label2);
            this.tpLocalLicenses.Controls.Add(this.dgvLocalLicensesHistory);
            this.tpLocalLicenses.Location = new System.Drawing.Point(4, 29);
            this.tpLocalLicenses.Name = "tpLocalLicenses";
            this.tpLocalLicenses.Padding = new System.Windows.Forms.Padding(3);
            this.tpLocalLicenses.Size = new System.Drawing.Size(1166, 241);
            this.tpLocalLicenses.TabIndex = 0;
            this.tpLocalLicenses.Text = "Local";
            // 
            // tpInternationalLicenses
            // 
            this.tpInternationalLicenses.BackColor = System.Drawing.Color.White;
            this.tpInternationalLicenses.Controls.Add(this.label1);
            this.tpInternationalLicenses.Location = new System.Drawing.Point(4, 29);
            this.tpInternationalLicenses.Name = "tpInternationalLicenses";
            this.tpInternationalLicenses.Padding = new System.Windows.Forms.Padding(3);
            this.tpInternationalLicenses.Size = new System.Drawing.Size(1166, 241);
            this.tpInternationalLicenses.TabIndex = 1;
            this.tpInternationalLicenses.Text = "International";
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnClose.Location = new System.Drawing.Point(1060, 834);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(127, 43);
            this.btnClose.TabIndex = 41;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvLocalLicensesHistory
            // 
            this.dgvLocalLicensesHistory.AllowUserToAddRows = false;
            this.dgvLocalLicensesHistory.AllowUserToDeleteRows = false;
            this.dgvLocalLicensesHistory.AllowUserToResizeColumns = false;
            this.dgvLocalLicensesHistory.AllowUserToResizeRows = false;
            this.dgvLocalLicensesHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvLocalLicensesHistory.BackgroundColor = System.Drawing.Color.White;
            this.dgvLocalLicensesHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLocalLicensesHistory.Location = new System.Drawing.Point(9, 40);
            this.dgvLocalLicensesHistory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvLocalLicensesHistory.MultiSelect = false;
            this.dgvLocalLicensesHistory.Name = "dgvLocalLicensesHistory";
            this.dgvLocalLicensesHistory.ReadOnly = true;
            this.dgvLocalLicensesHistory.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvLocalLicensesHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLocalLicensesHistory.Size = new System.Drawing.Size(1145, 174);
            this.dgvLocalLicensesHistory.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(9, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 20);
            this.label2.TabIndex = 34;
            this.label2.Text = "Local Licenses History:";
            // 
            // lblLocalRecords
            // 
            this.lblLocalRecords.AutoSize = true;
            this.lblLocalRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocalRecords.Location = new System.Drawing.Point(8, 216);
            this.lblLocalRecords.Name = "lblLocalRecords";
            this.lblLocalRecords.Size = new System.Drawing.Size(91, 20);
            this.lblLocalRecords.TabIndex = 35;
            this.lblLocalRecords.Text = "#Records:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(488, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 20);
            this.label1.TabIndex = 35;
            this.label1.Text = "Coming Soon";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 158);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(263, 242);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 43;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Modern No. 20", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(7)))), ((int)(((byte)(13)))));
            this.label3.Location = new System.Drawing.Point(477, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(239, 36);
            this.label3.TabIndex = 42;
            this.label3.Text = "Manage Drivers";
            // 
            // ctrlFilterPerson1
            // 
            this.ctrlFilterPerson1.BackColor = System.Drawing.Color.White;
            this.ctrlFilterPerson1.FilterEnabled = true;
            this.ctrlFilterPerson1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlFilterPerson1.Location = new System.Drawing.Point(305, 53);
            this.ctrlFilterPerson1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctrlFilterPerson1.Name = "ctrlFilterPerson1";
            this.ctrlFilterPerson1.PersonID = -1;
            this.ctrlFilterPerson1.Size = new System.Drawing.Size(882, 452);
            this.ctrlFilterPerson1.TabIndex = 0;
            // 
            // frmManageLicenses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 897);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbDriverLicense);
            this.Controls.Add(this.ctrlFilterPerson1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmManageLicenses";
            this.Text = "frmManageLicenses";
            this.Load += new System.EventHandler(this.frmManageLicenses_Load);
            this.gbDriverLicense.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpLocalLicenses.ResumeLayout(false);
            this.tpLocalLicenses.PerformLayout();
            this.tpInternationalLicenses.ResumeLayout(false);
            this.tpInternationalLicenses.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocalLicensesHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private People.ctrlFilterPerson ctrlFilterPerson1;
        private System.Windows.Forms.GroupBox gbDriverLicense;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpLocalLicenses;
        private System.Windows.Forms.TabPage tpInternationalLicenses;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvLocalLicensesHistory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblLocalRecords;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
    }
}