using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Studiu_Individual_1
{

    public partial class LoginOption : Form
    {
        private Dictionary<Control, Rectangle> initialSizes = new Dictionary<Control, Rectangle>();
        private float initialFormWidth, initialFormHeight;

        private void SaveInitialSizes()
        {
            initialFormWidth = this.Width;
            initialFormHeight = this.Height;

            foreach (Control ctrl in this.Controls)
            {
                initialSizes[ctrl] = new Rectangle(ctrl.Location, ctrl.Size);
            }
        }

        private void ResizeControls()
        {
            float scaleX = this.Width / initialFormWidth;
            float scaleY = this.Height / initialFormHeight;

            foreach (Control ctrl in this.Controls)
            {
                if (initialSizes.ContainsKey(ctrl))
                {
                    Rectangle rect = initialSizes[ctrl];
                    ctrl.Location = new Point((int)(rect.X * scaleX), (int)(rect.Y * scaleY));
                    ctrl.Size = new Size((int)(rect.Width * scaleX), (int)(rect.Height * scaleY));
                }
            }
        }
        public LoginOption()
        {

            InitializeComponent();
            SaveInitialSizes();
            this.Resize += (s, e) => ResizeControls();

            admin.MouseEnter += PictureBox_MouseEnter;
            admin.MouseLeave += PictureBox_MouseLeave;

            doctor.MouseEnter += PictureBox_MouseEnter;
            doctor.MouseLeave += PictureBox_MouseLeave;

            receptionist.MouseEnter += PictureBox_MouseEnter;
            receptionist.MouseLeave += PictureBox_MouseLeave;
        }

        private void Administrator_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Administrator adminForm = new Administrator();
            adminForm.Show();
        }

        private void admin_Click(object sender, EventArgs e)
        {
            Administrator adminForm = new Administrator();
            adminForm.Show();
        }

        private void Medic_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Medic medicForm = new Medic();
            medicForm.Show();
        }

        private void doctor_Click(object sender, EventArgs e)
        {
            Medic medicForm = new Medic();
            medicForm.Show();
        }

        private void Recepționist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Receptionist recForm = new Receptionist();
            recForm.Show();
        }

        private void receptionist_Click(object sender, EventArgs e)
        {
            Receptionist recForm = new Receptionist();
            recForm.Show();
        }

        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                pictureBox.Size = new Size((int)(pictureBox.Width * 1.2), (int)(pictureBox.Height * 1.2));
            }
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                pictureBox.Size = new Size((int)(pictureBox.Width / 1.2), (int)(pictureBox.Height / 1.2));
            }
        }

    }
}
