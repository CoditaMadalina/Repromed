using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Studiu_Individual_1
{
    public partial class Receptionist : Form
    {
        public Receptionist()
        {
            InitializeComponent();
            this.AcceptButton = Conectare_bt2;
        }

        private void showPassword_btn_Click_1(object sender, EventArgs e)
        {
            password_tb.UseSystemPasswordChar = false;
            showPassword_btn.Visible = false;
            hidePassword_btn.Visible = true;
            password_tb.Focus();
        }

        private void hidePassword_btn_Click(object sender, EventArgs e)
        {
            password_tb.UseSystemPasswordChar = true;
            hidePassword_btn.Visible = false;
            showPassword_btn.Visible = true;
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
            string receptionistUsername = username_tb.Text.Trim();
            string receptionistPassword = password_tb.Text;

            // Verifică dacă username-ul și parola sunt introduse
            if (string.IsNullOrWhiteSpace(receptionistUsername) || string.IsNullOrWhiteSpace(receptionistPassword))
            {
                AfiseazaEroare("Introduceți username-ul și parola!");
                return;
            }

            try
            {
                // Conexiune la baza de date folosind SQL Server Authentication
                connString = $"Data Source=DESKTOP-MFVSBQK\\SQLEXPRESS;Initial Catalog=SpitalRepromed;Persist Security Info=True;User ID={receptionistUsername};Password={receptionistPassword};Encrypt=False;TrustServerCertificate=True;";

                // Conectează-te la baza de date
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    MessageBox.Show("Autentificare reușită prin SQL Server!");

                    // După autentificare, ascunde formularul de login și afișează fereastra principală pentru Receptionist
                    this.Hide();
                    ReceptionistMenu recepMenu = new ReceptionistMenu(connString, receptionistUsername);
                    recepMenu.ShowDialog();
                    this.Show();
                }
            }
            catch (Exception ex)
            {
                password_tb.Clear();
                AfiseazaEroare($"Autentificare eșuată: {ex.Message}\nConexiune: {receptionistUsername}");
            }
        }

        // Metodă pentru afișarea mesajelor de eroare
        private void AfiseazaEroare(string mesaj)
        {
            MessageBox.Show(mesaj, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
