namespace Studiu_Individual_1
{
    partial class Receptionist
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Receptionist));
            this.LOGIN = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.noCont = new System.Windows.Forms.LinkLabel();
            this.hidePassword_btn = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.showPassword_btn = new System.Windows.Forms.Button();
            this.Conectare_bt2 = new System.Windows.Forms.Button();
            this.password_tb = new System.Windows.Forms.TextBox();
            this.username_tb = new System.Windows.Forms.TextBox();
            this.PASSWORD_A = new System.Windows.Forms.Label();
            this.USERNAME_a = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // LOGIN
            // 
            this.LOGIN.AutoSize = true;
            this.LOGIN.Font = new System.Drawing.Font("Modern No. 20", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LOGIN.Location = new System.Drawing.Point(178, 21);
            this.LOGIN.Name = "LOGIN";
            this.LOGIN.Size = new System.Drawing.Size(324, 30);
            this.LOGIN.TabIndex = 2;
            this.LOGIN.Text = "RECEPȚIONIST LOGIN";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 65);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(262, 299);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // noCont
            // 
            this.noCont.AutoSize = true;
            this.noCont.Location = new System.Drawing.Point(318, 348);
            this.noCont.Name = "noCont";
            this.noCont.Size = new System.Drawing.Size(263, 16);
            this.noCont.TabIndex = 14;
            this.noCont.TabStop = true;
            this.noCont.Text = "Nu aveți un cont ? Contactați administratorul";
            // 
            // hidePassword_btn
            // 
            this.hidePassword_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("hidePassword_btn.BackgroundImage")));
            this.hidePassword_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.hidePassword_btn.Location = new System.Drawing.Point(545, 214);
            this.hidePassword_btn.Name = "hidePassword_btn";
            this.hidePassword_btn.Size = new System.Drawing.Size(36, 29);
            this.hidePassword_btn.TabIndex = 23;
            this.hidePassword_btn.UseVisualStyleBackColor = true;
            this.hidePassword_btn.Visible = false;
            this.hidePassword_btn.Click += new System.EventHandler(this.hidePassword_btn_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(312, 214);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 27);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 22;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.White;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(312, 118);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(32, 27);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 21;
            this.pictureBox3.TabStop = false;
            // 
            // showPassword_btn
            // 
            this.showPassword_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("showPassword_btn.BackgroundImage")));
            this.showPassword_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.showPassword_btn.Location = new System.Drawing.Point(545, 213);
            this.showPassword_btn.Name = "showPassword_btn";
            this.showPassword_btn.Size = new System.Drawing.Size(36, 30);
            this.showPassword_btn.TabIndex = 20;
            this.showPassword_btn.UseVisualStyleBackColor = true;
            this.showPassword_btn.Click += new System.EventHandler(this.showPassword_btn_Click_1);
            // 
            // Conectare_bt2
            // 
            this.Conectare_bt2.BackColor = System.Drawing.Color.LightSeaGreen;
            this.Conectare_bt2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Conectare_bt2.ForeColor = System.Drawing.Color.White;
            this.Conectare_bt2.Location = new System.Drawing.Point(376, 277);
            this.Conectare_bt2.Name = "Conectare_bt2";
            this.Conectare_bt2.Size = new System.Drawing.Size(151, 43);
            this.Conectare_bt2.TabIndex = 19;
            this.Conectare_bt2.Text = "Conectare";
            this.Conectare_bt2.UseVisualStyleBackColor = false;
            this.Conectare_bt2.Click += new System.EventHandler(this.button1_Click);
            // 
            // password_tb
            // 
            this.password_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password_tb.Location = new System.Drawing.Point(340, 214);
            this.password_tb.Name = "password_tb";
            this.password_tb.Size = new System.Drawing.Size(241, 27);
            this.password_tb.TabIndex = 18;
            this.password_tb.UseSystemPasswordChar = true;
            // 
            // username_tb
            // 
            this.username_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username_tb.Location = new System.Drawing.Point(340, 118);
            this.username_tb.Name = "username_tb";
            this.username_tb.Size = new System.Drawing.Size(241, 27);
            this.username_tb.TabIndex = 17;
            this.username_tb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.username_tb_KeyDown);
            // 
            // PASSWORD_A
            // 
            this.PASSWORD_A.AutoSize = true;
            this.PASSWORD_A.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.PASSWORD_A.Location = new System.Drawing.Point(308, 179);
            this.PASSWORD_A.Name = "PASSWORD_A";
            this.PASSWORD_A.Size = new System.Drawing.Size(62, 22);
            this.PASSWORD_A.TabIndex = 16;
            this.PASSWORD_A.Text = "Parola";
            // 
            // USERNAME_a
            // 
            this.USERNAME_a.AutoSize = true;
            this.USERNAME_a.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.USERNAME_a.Location = new System.Drawing.Point(308, 79);
            this.USERNAME_a.Name = "USERNAME_a";
            this.USERNAME_a.Size = new System.Drawing.Size(153, 22);
            this.USERNAME_a.TabIndex = 15;
            this.USERNAME_a.Text = "Nume de utilizator";
            // 
            // Receptionist
            // 
            this.AcceptButton = this.Conectare_bt2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(629, 404);
            this.Controls.Add(this.hidePassword_btn);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.showPassword_btn);
            this.Controls.Add(this.Conectare_bt2);
            this.Controls.Add(this.password_tb);
            this.Controls.Add(this.username_tb);
            this.Controls.Add(this.PASSWORD_A);
            this.Controls.Add(this.USERNAME_a);
            this.Controls.Add(this.noCont);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LOGIN);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Receptionist";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Receptionist";
            this.Click += new System.EventHandler(this.button1_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LOGIN;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel noCont;
        private System.Windows.Forms.Button hidePassword_btn;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button showPassword_btn;
        private System.Windows.Forms.Button Conectare_bt2;
        private System.Windows.Forms.TextBox password_tb;
        private System.Windows.Forms.TextBox username_tb;
        private System.Windows.Forms.Label PASSWORD_A;
        private System.Windows.Forms.Label USERNAME_a;
    }
}