namespace Studiu_Individual_1
{
    partial class Administrator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Administrator));
            this.loginpicture = new System.Windows.Forms.PictureBox();
            this.LOGIN = new System.Windows.Forms.Label();
            this.USERNAME_a = new System.Windows.Forms.Label();
            this.PASSWORD_A = new System.Windows.Forms.Label();
            this.username_tb = new System.Windows.Forms.TextBox();
            this.password_tb = new System.Windows.Forms.TextBox();
            this.Conectare_bt = new System.Windows.Forms.Button();
            this.showPassword_btn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.hidePassword_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.loginpicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // loginpicture
            // 
            this.loginpicture.Image = ((System.Drawing.Image)(resources.GetObject("loginpicture.Image")));
            this.loginpicture.Location = new System.Drawing.Point(12, 65);
            this.loginpicture.Name = "loginpicture";
            this.loginpicture.Size = new System.Drawing.Size(264, 311);
            this.loginpicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.loginpicture.TabIndex = 0;
            this.loginpicture.TabStop = false;
            // 
            // LOGIN
            // 
            this.LOGIN.AutoSize = true;
            this.LOGIN.Font = new System.Drawing.Font("Modern No. 20", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LOGIN.Location = new System.Drawing.Point(150, 22);
            this.LOGIN.Name = "LOGIN";
            this.LOGIN.Size = new System.Drawing.Size(356, 30);
            this.LOGIN.TabIndex = 1;
            this.LOGIN.Text = "ADMINISTRATOR LOGIN";
            // 
            // USERNAME_a
            // 
            this.USERNAME_a.AutoSize = true;
            this.USERNAME_a.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.USERNAME_a.Location = new System.Drawing.Point(317, 96);
            this.USERNAME_a.Name = "USERNAME_a";
            this.USERNAME_a.Size = new System.Drawing.Size(153, 22);
            this.USERNAME_a.TabIndex = 2;
            this.USERNAME_a.Text = "Nume de utilizator";
            // 
            // PASSWORD_A
            // 
            this.PASSWORD_A.AutoSize = true;
            this.PASSWORD_A.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.PASSWORD_A.Location = new System.Drawing.Point(317, 196);
            this.PASSWORD_A.Name = "PASSWORD_A";
            this.PASSWORD_A.Size = new System.Drawing.Size(62, 22);
            this.PASSWORD_A.TabIndex = 3;
            this.PASSWORD_A.Text = "Parola";
            // 
            // username_tb
            // 
            this.username_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username_tb.Location = new System.Drawing.Point(349, 135);
            this.username_tb.Name = "username_tb";
            this.username_tb.Size = new System.Drawing.Size(241, 27);
            this.username_tb.TabIndex = 4;
            // 
            // password_tb
            // 
            this.password_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password_tb.Location = new System.Drawing.Point(349, 231);
            this.password_tb.Name = "password_tb";
            this.password_tb.Size = new System.Drawing.Size(241, 27);
            this.password_tb.TabIndex = 5;
            this.password_tb.UseSystemPasswordChar = true;
            // 
            // Conectare_bt
            // 
            this.Conectare_bt.BackColor = System.Drawing.Color.LightSeaGreen;
            this.Conectare_bt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Conectare_bt.ForeColor = System.Drawing.Color.White;
            this.Conectare_bt.Location = new System.Drawing.Point(385, 294);
            this.Conectare_bt.Name = "Conectare_bt";
            this.Conectare_bt.Size = new System.Drawing.Size(151, 43);
            this.Conectare_bt.TabIndex = 6;
            this.Conectare_bt.Text = "Conectare";
            this.Conectare_bt.UseVisualStyleBackColor = false;
            this.Conectare_bt.Click += new System.EventHandler(this.Conectare_bt_Click);
            // 
            // showPassword_btn
            // 
            this.showPassword_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("showPassword_btn.BackgroundImage")));
            this.showPassword_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.showPassword_btn.Location = new System.Drawing.Point(554, 230);
            this.showPassword_btn.Name = "showPassword_btn";
            this.showPassword_btn.Size = new System.Drawing.Size(36, 30);
            this.showPassword_btn.TabIndex = 7;
            this.showPassword_btn.UseVisualStyleBackColor = true;
            this.showPassword_btn.Click += new System.EventHandler(this.showPassword_btn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(321, 135);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(321, 231);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 27);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // hidePassword_btn
            // 
            this.hidePassword_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("hidePassword_btn.BackgroundImage")));
            this.hidePassword_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.hidePassword_btn.Location = new System.Drawing.Point(554, 231);
            this.hidePassword_btn.Name = "hidePassword_btn";
            this.hidePassword_btn.Size = new System.Drawing.Size(36, 29);
            this.hidePassword_btn.TabIndex = 10;
            this.hidePassword_btn.UseVisualStyleBackColor = true;
            this.hidePassword_btn.Visible = false;
            this.hidePassword_btn.Click += new System.EventHandler(this.hidePassword_btn_Click);
            // 
            // Administrator
            // 
            this.AcceptButton = this.Conectare_bt;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(629, 404);
            this.Controls.Add(this.hidePassword_btn);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.showPassword_btn);
            this.Controls.Add(this.Conectare_bt);
            this.Controls.Add(this.password_tb);
            this.Controls.Add(this.username_tb);
            this.Controls.Add(this.PASSWORD_A);
            this.Controls.Add(this.USERNAME_a);
            this.Controls.Add(this.LOGIN);
            this.Controls.Add(this.loginpicture);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Administrator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administrator";
            ((System.ComponentModel.ISupportInitialize)(this.loginpicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox loginpicture;
        private System.Windows.Forms.Label LOGIN;
        private System.Windows.Forms.Label USERNAME_a;
        private System.Windows.Forms.Label PASSWORD_A;
        private System.Windows.Forms.TextBox username_tb;
        private System.Windows.Forms.TextBox password_tb;
        private System.Windows.Forms.Button Conectare_bt;
        private System.Windows.Forms.Button showPassword_btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button hidePassword_btn;
    }
}