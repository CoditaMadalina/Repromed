using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Studiu_Individual_1
{
    public partial class Medic : Form
    {
        public Medic()
        {
            InitializeComponent();
            this.AcceptButton = Conectare_bt1;
        }

        private void show_btn_Click(object sender, EventArgs e)
        {
            password_tb.UseSystemPasswordChar = false;
            show.Visible = false;
            hide.Visible = true;
            password_tb.Focus();
        }

        private void hide_btn_Click(object sender, EventArgs e)
        {
            password_tb.UseSystemPasswordChar = true;
            hide.Visible = false;
            show.Visible = true;
            password_tb.Focus();
        }

        private void username_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                password_tb.Focus();
            }
        }

        private string connString;

        private void button1_Click(object sender, EventArgs e)
        {
            string medicUsername = username_tb.Text.Trim();
            string medicPassword = password_tb.Text;

            if (string.IsNullOrWhiteSpace(medicUsername) || string.IsNullOrWhiteSpace(medicPassword))
            {
                AfiseazaEroare("Introduceți username-ul și parola!");
                return;
            }

            try
            {
                connString = $"Data Source=DESKTOP-MFVSBQK\\SQLEXPRESS;Initial Catalog=SpitalRepromed;Persist Security Info=True;User ID={medicUsername};Password={medicPassword};Encrypt=False;TrustServerCertificate=True;";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    MessageBox.Show("Autentificare reușită prin SQL Server!");
                    this.Hide();
                    MedicMenu medicMenu = new MedicMenu(connString, medicUsername);
                    medicMenu.ShowDialog();
                    this.Show();
                }
            }
            catch (Exception ex)
            {
                password_tb.Clear();
                AfiseazaEroare($"Autentificare eșuată: {ex.Message}\nConexiune: {medicUsername}");
            }
        }

        private void AfiseazaEroare(string mesaj)
        {
            MessageBox.Show(mesaj, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
