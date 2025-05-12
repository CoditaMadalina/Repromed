namespace Studiu_Individual_1
{
    partial class MedicMenu
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MedicMenu));
            this.panelMeniu = new System.Windows.Forms.Panel();
            this.back = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.istoric = new System.Windows.Forms.Button();
            this.programari = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.medicBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panelMeniu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.medicBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMeniu
            // 
            this.panelMeniu.BackColor = System.Drawing.Color.Transparent;
            this.panelMeniu.Controls.Add(this.back);
            this.panelMeniu.Controls.Add(this.label1);
            this.panelMeniu.Controls.Add(this.istoric);
            this.panelMeniu.Controls.Add(this.programari);
            this.panelMeniu.Location = new System.Drawing.Point(-2, -3);
            this.panelMeniu.Name = "panelMeniu";
            this.panelMeniu.Size = new System.Drawing.Size(394, 1066);
            this.panelMeniu.TabIndex = 0;
            this.panelMeniu.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMeniu_Paint);
            // 
            // back
            // 
            this.back.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.back.ForeColor = System.Drawing.Color.White;
            this.back.Location = new System.Drawing.Point(14, 559);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(302, 87);
            this.back.TabIndex = 3;
            this.back.Text = "⬅ Înapoi";
            this.back.UseVisualStyleBackColor = true;
            this.back.Click += new System.EventHandler(this.back_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(28, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "MENIUL PERSONAL 🩺";
            // 
            // istoric
            // 
            this.istoric.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.istoric.ForeColor = System.Drawing.Color.White;
            this.istoric.Location = new System.Drawing.Point(14, 390);
            this.istoric.Name = "istoric";
            this.istoric.Size = new System.Drawing.Size(302, 123);
            this.istoric.TabIndex = 1;
            this.istoric.Text = "📜 Istoricul medical al pacientului";
            this.istoric.UseVisualStyleBackColor = true;
            this.istoric.Click += new System.EventHandler(this.istoric_Click);
            // 
            // programari
            // 
            this.programari.BackColor = System.Drawing.Color.White;
            this.programari.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.programari.ForeColor = System.Drawing.Color.White;
            this.programari.Location = new System.Drawing.Point(14, 278);
            this.programari.Name = "programari";
            this.programari.Size = new System.Drawing.Size(302, 87);
            this.programari.TabIndex = 1;
            this.programari.Text = "📅 Gestionarea programărilor";
            this.programari.UseVisualStyleBackColor = false;
            this.programari.Click += new System.EventHandler(this.programari_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(565, -97);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(489, 385);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // medicBindingSource
            // 
            this.medicBindingSource.DataMember = "Medic";
//            this.medicBindingSource.DataSource = this.spitalRepromedDataSet;
            // 
            // MedicMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1482, 1055);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panelMeniu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MedicMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MedicMenu";
//            this.Load += new System.EventHandler(this.MedicMenu_Load);
            this.panelMeniu.ResumeLayout(false);
            this.panelMeniu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.medicBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMeniu;
        private System.Windows.Forms.Button istoric;
        private System.Windows.Forms.Button programari;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button back;
    //    private SpitalRepromedDataSet spitalRepromedDataSet;
        private System.Windows.Forms.BindingSource medicBindingSource;
    //    private SpitalRepromedDataSetTableAdapters.MedicTableAdapter medicTableAdapter;
    }
}