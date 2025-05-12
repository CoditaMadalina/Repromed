using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Studiu_Individual_1
{
    public partial class Administrator : Form
    {
        // Credențiale hardcodate pentru conexiunea la baza de date
        private const string DB_SERVER = ".\\SQLEXPRESS";
        private const string DB_NAME = "SpitalRepromed";
        private const string DB_USER = "admin_hotineanu";
        private const string DB_PASSWORD = "HotineanuA123!";

        // Credențiale hardcodate pentru autentificare în aplicație
        private const string APP_ADMIN_USER = "admin_hotineanu";
        private const string APP_ADMIN_PASS = "HotineanuA123!";

        public Administrator()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Configurare inițială a controalelor
            password_tb.UseSystemPasswordChar = true;
            hidePassword_btn.Visible = false;

            // Evenimente pentru navigare cu săgeți
            username_tb.KeyDown += TextBox_KeyDown;
            password_tb.KeyDown += TextBox_KeyDown;
            Conectare_bt.KeyDown += Button_KeyDown;

            // Focus inițial pe câmpul de username
            username_tb.Focus();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (sender == username_tb)
                        password_tb.Focus();
                    else if (sender == password_tb)
                        Authenticate();
                    break;

                case Keys.Down:
                    if (sender == username_tb)
                        password_tb.Focus();
                    else if (sender == password_tb)
                        Conectare_bt.Focus();
                    break;

                case Keys.Up:
                    if (sender == password_tb)
                        username_tb.Focus();
                    else if (sender == Conectare_bt)
                        password_tb.Focus();
                    break;
            }
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Authenticate();
            }
        }

        private void Conectare_bt_Click(object sender, EventArgs e)
        {
            Authenticate();
        }

        private void Authenticate()
        {
            string username = username_tb.Text.Trim();
            string password = password_tb.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Introduceți username-ul și parola!");
                return;
            }

            if (username == APP_ADMIN_USER && password == APP_ADMIN_PASS)
            {
                if (TestDatabaseConnection())
                {
                    this.Hide();
                    AdminMenu adminForm = new AdminMenu();
                    adminForm.FormClosed += (s, args) => this.Close();
                    adminForm.Show();
                }
                else
                {
                    ShowError("Conexiune la baza de date eșuată!");
                }
            }
            else
            {
                password_tb.Clear();
                ShowError("Credențiale invalide!");
            }
        }

        private bool TestDatabaseConnection()
        {
            try
            {
                using (var connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare conexiune: {ex.Message}");
                return false;
            }
        }

        private string GetConnectionString()
        {
            return $"Server={DB_SERVER};Database={DB_NAME};User Id={DB_USER};Password={DB_PASSWORD};";
        }

        private void showPassword_btn_Click(object sender, EventArgs e)
        {
            password_tb.UseSystemPasswordChar = false;
            showPassword_btn.Visible = false;
            hidePassword_btn.Visible = true;
        }

        private void hidePassword_btn_Click(object sender, EventArgs e)
        {
            password_tb.UseSystemPasswordChar = true;
            hidePassword_btn.Visible = false;
            showPassword_btn.Visible = true;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}