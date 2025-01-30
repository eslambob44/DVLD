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
            this.ctrlShowRetakeTestApplicationInfo1 = new DVLD_PresentationLayer.Applications.Retake_Test_Application.ctrlShowRetakeTestApplicationInfo();
            this.SuspendLayout();
            // 
            // ctrlShowRetakeTestApplicationInfo1
            // 
            this.ctrlShowRetakeTestApplicationInfo1.BackColor = System.Drawing.Color.White;
            this.ctrlShowRetakeTestApplicationInfo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlShowRetakeTestApplicationInfo1.Location = new System.Drawing.Point(236, 167);
            this.ctrlShowRetakeTestApplicationInfo1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctrlShowRetakeTestApplicationInfo1.Name = "ctrlShowRetakeTestApplicationInfo1";
            this.ctrlShowRetakeTestApplicationInfo1.Size = new System.Drawing.Size(556, 123);
            this.ctrlShowRetakeTestApplicationInfo1.TabIndex = 0;
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 620);
            this.Controls.Add(this.ctrlShowRetakeTestApplicationInfo1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmTest";
            this.Text = "clsTest";
            this.Load += new System.EventHandler(this.frmTest_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Applications.Retake_Test_Application.ctrlShowRetakeTestApplicationInfo ctrlShowRetakeTestApplicationInfo1;
    }
}