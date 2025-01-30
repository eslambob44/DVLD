namespace DVLD_PresentationLayer.Test_Appointments
{
    partial class frmManageTestAppointments
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManageTestAppointments));
            this.lblAppointmentTitle = new System.Windows.Forms.Label();
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.lblRecords = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddTestAppointment = new System.Windows.Forms.Button();
            this.pbAppointment = new System.Windows.Forms.PictureBox();
            this.ctrlShowLocalDrivingLicenseApplicationInfo1 = new DVLD_PresentationLayer.Applications.Local_License_Application.ctrlShowLocalDrivingLicenseApplicationInfo();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAppointment)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAppointmentTitle
            // 
            this.lblAppointmentTitle.AutoSize = true;
            this.lblAppointmentTitle.Font = new System.Drawing.Font("Modern No. 20", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppointmentTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(7)))), ((int)(((byte)(13)))));
            this.lblAppointmentTitle.Location = new System.Drawing.Point(242, 116);
            this.lblAppointmentTitle.Name = "lblAppointmentTitle";
            this.lblAppointmentTitle.Size = new System.Drawing.Size(370, 36);
            this.lblAppointmentTitle.TabIndex = 9;
            this.lblAppointmentTitle.Text = "Vision Test Appointment";
            // 
            // dgvAppointments
            // 
            this.dgvAppointments.AllowUserToAddRows = false;
            this.dgvAppointments.AllowUserToDeleteRows = false;
            this.dgvAppointments.AllowUserToResizeColumns = false;
            this.dgvAppointments.AllowUserToResizeRows = false;
            this.dgvAppointments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAppointments.BackgroundColor = System.Drawing.Color.White;
            this.dgvAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAppointments.Location = new System.Drawing.Point(13, 590);
            this.dgvAppointments.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvAppointments.MultiSelect = false;
            this.dgvAppointments.Name = "dgvAppointments";
            this.dgvAppointments.ReadOnly = true;
            this.dgvAppointments.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvAppointments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAppointments.Size = new System.Drawing.Size(829, 240);
            this.dgvAppointments.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 556);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "Appointments:";
            // 
            // lblRecords
            // 
            this.lblRecords.AutoSize = true;
            this.lblRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecords.Location = new System.Drawing.Point(25, 842);
            this.lblRecords.Name = "lblRecords";
            this.lblRecords.Size = new System.Drawing.Size(91, 20);
            this.lblRecords.TabIndex = 15;
            this.lblRecords.Text = "#Records:";
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnClose.Location = new System.Drawing.Point(700, 834);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(141, 46);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddTestAppointment
            // 
            this.btnAddTestAppointment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTestAppointment.Image = ((System.Drawing.Image)(resources.GetObject("btnAddTestAppointment.Image")));
            this.btnAddTestAppointment.Location = new System.Drawing.Point(794, 548);
            this.btnAddTestAppointment.Name = "btnAddTestAppointment";
            this.btnAddTestAppointment.Size = new System.Drawing.Size(47, 37);
            this.btnAddTestAppointment.TabIndex = 14;
            this.btnAddTestAppointment.UseVisualStyleBackColor = true;
            this.btnAddTestAppointment.Click += new System.EventHandler(this.btnAddTestAppointment_Click);
            // 
            // pbAppointment
            // 
            this.pbAppointment.Image = global::DVLD_PresentationLayer.Properties.Resources.driving_test_512;
            this.pbAppointment.Location = new System.Drawing.Point(357, 12);
            this.pbAppointment.Name = "pbAppointment";
            this.pbAppointment.Size = new System.Drawing.Size(140, 101);
            this.pbAppointment.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbAppointment.TabIndex = 8;
            this.pbAppointment.TabStop = false;
            // 
            // ctrlShowLocalDrivingLicenseApplicationInfo1
            // 
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.BackColor = System.Drawing.Color.White;
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.Location = new System.Drawing.Point(13, 160);
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.Name = "ctrlShowLocalDrivingLicenseApplicationInfo1";
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.Size = new System.Drawing.Size(828, 380);
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.TabIndex = 18;
            // 
            // frmManageTestAppointments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(854, 895);
            this.Controls.Add(this.ctrlShowLocalDrivingLicenseApplicationInfo1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblRecords);
            this.Controls.Add(this.btnAddTestAppointment);
            this.Controls.Add(this.dgvAppointments);
            this.Controls.Add(this.lblAppointmentTitle);
            this.Controls.Add(this.pbAppointment);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmManageTestAppointments";
            this.Text = "frmManageTestAppointments";
            this.Load += new System.EventHandler(this.frmManageTestAppointments_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAppointment)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAppointmentTitle;
        private System.Windows.Forms.PictureBox pbAppointment;
        private System.Windows.Forms.Button btnAddTestAppointment;
        private System.Windows.Forms.DataGridView dgvAppointments;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRecords;
        private System.Windows.Forms.Button btnClose;
        private Applications.Local_License_Application.ctrlShowLocalDrivingLicenseApplicationInfo ctrlShowLocalDrivingLicenseApplicationInfo1;
    }
}