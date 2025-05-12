namespace Studiu_Individual_1
{
    partial class LoginOption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginOption));
            this.Conectare = new System.Windows.Forms.Label();
            this.Administrator = new System.Windows.Forms.LinkLabel();
            this.Medic = new System.Windows.Forms.LinkLabel();
            this.Recepționist = new System.Windows.Forms.LinkLabel();
            this.admin = new System.Windows.Forms.PictureBox();
            this.doctor = new System.Windows.Forms.PictureBox();
            this.receptionist = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.admin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.doctor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.receptionist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Conectare
            // 
            this.Conectare.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Conectare.AutoSize = true;
            this.Conectare.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Conectare.Location = new System.Drawing.Point(449, 202);
            this.Conectare.Name = "Conectare";
            this.Conectare.Size = new System.Drawing.Size(591, 69);
            this.Conectare.TabIndex = 0;
            this.Conectare.Text = "CONECTAȚI-VĂ CA";
            // 
            // Administrator
            // 
            this.Administrator.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Administrator.AutoSize = true;
            this.Administrator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Administrator.DisabledLinkColor = System.Drawing.Color.Red;
            this.Administrator.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Administrator.LinkColor = System.Drawing.Color.Black;
            this.Administrator.Location = new System.Drawing.Point(266, 603);
            this.Administrator.Name = "Administrator";
            this.Administrator.Size = new System.Drawing.Size(126, 25);
            this.Administrator.TabIndex = 1;
            this.Administrator.TabStop = true;
            this.Administrator.Text = "Administrator";
            this.Administrator.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Administrator_LinkClicked);
            // 
            // Medic
            // 
            this.Medic.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Medic.AutoSize = true;
            this.Medic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Medic.DisabledLinkColor = System.Drawing.Color.Red;
            this.Medic.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Medic.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.Medic.LinkColor = System.Drawing.Color.Black;
            this.Medic.Location = new System.Drawing.Point(701, 603);
            this.Medic.Name = "Medic";
            this.Medic.Size = new System.Drawing.Size(65, 25);
            this.Medic.TabIndex = 2;
            this.Medic.TabStop = true;
            this.Medic.Text = "Medic";
            this.Medic.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Medic_LinkClicked);
            // 
            // Recepționist
            // 
            this.Recepționist.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Recepționist.AutoSize = true;
            this.Recepționist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Recepționist.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Recepționist.LinkColor = System.Drawing.Color.Black;
            this.Recepționist.Location = new System.Drawing.Point(1089, 603);
            this.Recepționist.Name = "Recepționist";
            this.Recepționist.Size = new System.Drawing.Size(118, 25);
            this.Recepționist.TabIndex = 3;
            this.Recepționist.TabStop = true;
            this.Recepționist.Text = "Recepționist";
            this.Recepționist.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Recepționist_LinkClicked);
            // 
            // admin
            // 
            this.admin.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.admin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.admin.Image = ((System.Drawing.Image)(resources.GetObject("admin.Image")));
            this.admin.Location = new System.Drawing.Point(239, 382);
            this.admin.Name = "admin";
            this.admin.Size = new System.Drawing.Size(166, 163);
            this.admin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.admin.TabIndex = 4;
            this.admin.TabStop = false;
            this.admin.Click += new System.EventHandler(this.admin_Click);
            // 
            // doctor
            // 
            this.doctor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.doctor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.doctor.Image = ((System.Drawing.Image)(resources.GetObject("doctor.Image")));
            this.doctor.Location = new System.Drawing.Point(644, 382);
            this.doctor.Name = "doctor";
            this.doctor.Size = new System.Drawing.Size(166, 163);
            this.doctor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.doctor.TabIndex = 5;
            this.doctor.TabStop = false;
            this.doctor.Click += new System.EventHandler(this.doctor_Click);
            // 
            // receptionist
            // 
            this.receptionist.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.receptionist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.receptionist.Image = ((System.Drawing.Image)(resources.GetObject("receptionist.Image")));
            this.receptionist.Location = new System.Drawing.Point(1054, 382);
            this.receptionist.Name = "receptionist";
            this.receptionist.Size = new System.Drawing.Size(164, 163);
            this.receptionist.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.receptionist.TabIndex = 6;
            this.receptionist.TabStop = false;
            this.receptionist.Click += new System.EventHandler(this.receptionist_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(461, -69);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(565, 326);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // LoginOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1482, 1055);
            this.Controls.Add(this.Conectare);
            this.Controls.Add(this.receptionist);
            this.Controls.Add(this.doctor);
            this.Controls.Add(this.admin);
            this.Controls.Add(this.Recepționist);
            this.Controls.Add(this.Medic);
            this.Controls.Add(this.Administrator);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginOption";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginOption";
            ((System.ComponentModel.ISupportInitialize)(this.admin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.doctor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.receptionist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Conectare;
        private System.Windows.Forms.LinkLabel Administrator;
        private System.Windows.Forms.LinkLabel Medic;
        private System.Windows.Forms.LinkLabel Recepționist;
        private System.Windows.Forms.PictureBox admin;
        private System.Windows.Forms.PictureBox doctor;
        private System.Windows.Forms.PictureBox receptionist;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

