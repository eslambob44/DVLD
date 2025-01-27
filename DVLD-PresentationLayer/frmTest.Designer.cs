namespace DVLD_PresentationLayer
{
    partial class frmTest
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
            this.ctrlShowLocalDrivingLicenseApplicationInfo1 = new DVLD_PresentationLayer.Applications.Local_License_Application.ctrlShowLocalDrivingLicenseApplicationInfo();
            this.SuspendLayout();
            // 
            // ctrlShowLocalDrivingLicenseApplicationInfo1
            // 
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.BackColor = System.Drawing.Color.White;
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.Location = new System.Drawing.Point(65, 41);
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.Name = "ctrlShowLocalDrivingLicenseApplicationInfo1";
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.Size = new System.Drawing.Size(828, 454);
            this.ctrlShowLocalDrivingLicenseApplicationInfo1.TabIndex = 0;
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 620);
            this.Controls.Add(this.ctrlShowLocalDrivingLicenseApplicationInfo1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmTest";
            this.Text = "clsTest";
            this.Load += new System.EventHandler(this.frmTest_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Applications.Local_License_Application.ctrlShowLocalDrivingLicenseApplicationInfo ctrlShowLocalDrivingLicenseApplicationInfo1;
    }
}