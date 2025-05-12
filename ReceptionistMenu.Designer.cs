namespace Studiu_Individual_1
{
    partial class ReceptionistMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceptionistMenu));
            this.menuPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.back = new System.Windows.Forms.Button();
            this.cauta2 = new System.Windows.Forms.Button();
            this.cauta1 = new System.Windows.Forms.Button();
            this.programare = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuPanel
            // 
            this.menuPanel.BackColor = System.Drawing.Color.Transparent;
            this.menuPanel.Controls.Add(this.label1);
            this.menuPanel.Controls.Add(this.back);
            this.menuPanel.Controls.Add(this.cauta2);
            this.menuPanel.Controls.Add(this.cauta1);
            this.menuPanel.Controls.Add(this.programare);
            this.menuPanel.Location = new System.Drawing.Point(-6, 1);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Size = new System.Drawing.Size(377, 1060);
            this.menuPanel.TabIndex = 0;
            this.menuPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.menuPanel_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(18, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "MENIUL PERSONAL 🏢";
            // 
            // back
            // 
            this.back.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.back.ForeColor = System.Drawing.Color.White;
            this.back.Location = new System.Drawing.Point(44, 708);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(276, 87);
            this.back.TabIndex = 4;
            this.back.Text = "⬅ Înapoi";
            this.back.UseVisualStyleBackColor = true;
            this.back.Click += new System.EventHandler(this.back_Click);
            // 
            // cauta2
            // 
            this.cauta2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cauta2.ForeColor = System.Drawing.Color.White;
            this.cauta2.Location = new System.Drawing.Point(44, 512);
            this.cauta2.Name = "cauta2";
            this.cauta2.Size = new System.Drawing.Size(276, 154);
            this.cauta2.TabIndex = 2;
            this.cauta2.Text = "Vezi programările unui medic";
            this.cauta2.UseVisualStyleBackColor = true;
            this.cauta2.Click += new System.EventHandler(this.cauta2_Click);
            // 
            // cauta1
            // 
            this.cauta1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cauta1.ForeColor = System.Drawing.Color.White;
            this.cauta1.Location = new System.Drawing.Point(44, 364);
            this.cauta1.Name = "cauta1";
            this.cauta1.Size = new System.Drawing.Size(276, 106);
            this.cauta1.TabIndex = 1;
            this.cauta1.Text = "Vezi programările unui pacient";
            this.cauta1.UseVisualStyleBackColor = true;
            this.cauta1.Click += new System.EventHandler(this.cauta1_Click);
            // 
            // programare
            // 
            this.programare.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.programare.ForeColor = System.Drawing.Color.White;
            this.programare.Location = new System.Drawing.Point(44, 232);
            this.programare.Name = "programare";
            this.programare.Size = new System.Drawing.Size(276, 84);
            this.programare.TabIndex = 0;
            this.programare.Text = "Programări";
            this.programare.UseVisualStyleBackColor = true;
            this.programare.Click += new System.EventHandler(this.programare_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(555, -48);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(528, 321);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // ReceptionistMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1482, 1055);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReceptionistMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReceptionistMenu";
            this.menuPanel.ResumeLayout(false);
            this.menuPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel menuPanel;
        private System.Windows.Forms.Button cauta2;
        private System.Windows.Forms.Button cauta1;
        private System.Windows.Forms.Button programare;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button back;
        private System.Windows.Forms.Label label1;
    }
}