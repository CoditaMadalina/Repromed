using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Studiu_Individual_1
{
    public partial class MedicMenu : Form
    {
        private Dictionary<Control, Rectangle> initialSizes = new Dictionary<Control, Rectangle>();
        private float initialFormWidth, initialFormHeight;
        private string connString;
        private string medicUsername;
        private void SaveInitialSizes()
        {
            initialFormWidth = this.Width;
            initialFormHeight = this.Height;
            this.ResizeEnd += (s, ev) => {
                if (dgvProgramari != null)
                {
                    AdjustDataGridViewColumns();
                }
            };

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

        public MedicMenu(string connString, string medicUsername)
        {
            InitializeComponent();
            SaveInitialSizes();
            this.connString = connString;
            this.medicUsername = medicUsername;  
            this.Resize += (s, e) => ResizeControls();


            panelMeniu.BackColor = Color.FromArgb(30, 30, 45);
            programari.BackColor = Color.FromArgb(70, 70, 100);
            istoric.BackColor = Color.FromArgb(70, 70, 100);
            back.BackColor = Color.FromArgb(70, 70, 100);

            programari.FlatStyle = FlatStyle.Flat;
            istoric.FlatStyle = FlatStyle.Flat;
            back.FlatStyle = FlatStyle.Flat;

            istoric.FlatAppearance.BorderSize = 0;
            istoric.MouseEnter += (s, e) => istoric.BackColor = Color.FromArgb(70, 70, 100);
            istoric.MouseLeave += (s, e) => istoric.BackColor = Color.FromArgb(50, 50, 70);

            programari.FlatAppearance.BorderSize = 0;
            programari.MouseEnter += (s, e) => programari.BackColor = Color.FromArgb(70, 70, 100);
            programari.MouseLeave += (s, e) => programari.BackColor = Color.FromArgb(50, 50, 70);

            back.FlatAppearance.BorderSize = 0;
            back.MouseEnter += (s, e) => back.BackColor = Color.FromArgb(70, 70, 100);
            back.MouseLeave += (s, e) => back.BackColor = Color.FromArgb(50, 50, 70);
        }


        private void SeteazaStilButoane(Button btn)
        {
            btn.BackColor = Color.FromArgb(30, 70, 130);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.UseVisualStyleBackColor = false;
            btn.MouseEnter += (s, ev) => btn.BackColor = Color.FromArgb(80, 140, 200);
            btn.MouseLeave += (s, ev) => btn.BackColor = Color.FromArgb(50, 110, 180);
        }

        private void AscundePanouri()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Panel panel && panel.Name != "panelMeniu")
                {
                    panel.Visible = false;
                }
            }
        }

        private Panel panelAfisare;
        private DataGridView dgvProgramari;

        private void IncarcaProgramari(string criteriu, string valoare)
        {
            if (panelAfisare != null)
            {
                this.Controls.Remove(panelAfisare);
                panelAfisare.Dispose();
            }

            int oneThirdHeight = this.ClientSize.Height / 3;

            panelAfisare = new Panel
            {
                Location = new Point(panelMeniu.Right, oneThirdHeight),
                Size = new Size(this.ClientSize.Width - panelMeniu.Width, this.ClientSize.Height - oneThirdHeight),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(panelAfisare);
            panelAfisare.BringToFront();

            dgvProgramari = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                ScrollBars = ScrollBars.Both,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false, 
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false 
            };
            panelAfisare.Controls.Add(dgvProgramari);

            // Obține IdMedic
            string[] parts = medicUsername.Split('_');
            if (parts.Length != 2)
            {
                MessageBox.Show("Format invalid al username-ului.");
                return;
            }

            string nume = parts[0];
            string prenume = parts[1];
            string idMedic = "";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand getIdCmd = new SqlCommand("SELECT IdMedic FROM Medic WHERE Nume = @Nume AND Prenume = @Prenume", conn);
                getIdCmd.Parameters.AddWithValue("@Nume", nume);
                getIdCmd.Parameters.AddWithValue("@Prenume", prenume);
                object result = getIdCmd.ExecuteScalar();
                if (result == null)
                {
                    MessageBox.Show("Medicul nu a fost găsit.");
                    return;
                }
                idMedic = result.ToString();
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = @"
            SELECT 
                P.IdProgramare, 
                P.IdPacient, 
                C.Nume, 
                C.Prenume, 
                P.DataProgramare, 
                P.OraProgramare, 
                P.StatusProgramare,
                C.Gen, 
                C.Adresa, 
                C.NrTelefon, 
                C.Email
            FROM Programari P 
            JOIN Pacient C ON P.IdPacient = C.IdPacient
            WHERE P.IdMedic = @IdMedic";

                if (criteriu == "data")
                {
                    query += " AND CONVERT(date, P.DataProgramare) = @Data";
                }
                else if (criteriu == "idnp")
                {
                    query += " AND C.IdPacient = @IDNP";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdMedic", idMedic);

                if (criteriu == "data")
                    cmd.Parameters.AddWithValue("@Data", DateTime.Parse(valoare));
                else
                    cmd.Parameters.AddWithValue("@IDNP", valoare);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvProgramari.DataSource = dt;

                // Personalizare coloane
                if (dgvProgramari.Columns.Count > 0)
                {
                    // Setează titlurile coloanelor
                    dgvProgramari.Columns["IdProgramare"].HeaderText = "ID Programare";
                    dgvProgramari.Columns["IdPacient"].HeaderText = "ID Pacient";
                    dgvProgramari.Columns["Nume"].HeaderText = "Nume";
                    dgvProgramari.Columns["Prenume"].HeaderText = "Prenume";
                    dgvProgramari.Columns["DataProgramare"].HeaderText = "Data Programare";
                    dgvProgramari.Columns["OraProgramare"].HeaderText = "Ora Programare";
                    dgvProgramari.Columns["StatusProgramare"].HeaderText = "Status";
                    dgvProgramari.Columns["Gen"].HeaderText = "Gen";
                    dgvProgramari.Columns["Adresa"].HeaderText = "Adresă";
                    dgvProgramari.Columns["NrTelefon"].HeaderText = "Telefon";
                    dgvProgramari.Columns["Email"].HeaderText = "Email";

                    // Formatează coloana de dată
                    dgvProgramari.Columns["DataProgramare"].DefaultCellStyle.Format = "dd.MM.yyyy";

                    // Ajustează lățimea coloanelor
                    dgvProgramari.Columns["IdProgramare"].Width = 80;
                    dgvProgramari.Columns["IdPacient"].Width = 80;
                    dgvProgramari.Columns["Nume"].Width = 120;
                    dgvProgramari.Columns["Prenume"].Width = 120;
                    dgvProgramari.Columns["DataProgramare"].Width = 100;
                    dgvProgramari.Columns["OraProgramare"].Width = 80;
                    dgvProgramari.Columns["StatusProgramare"].Width = 80;

                    // Face coloanele să se extindă pentru a umple spațiul disponibil
                    dgvProgramari.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
        }

        private void programari_Click_1(object sender, EventArgs e)
        {
            AscundePanouri();

            if (panelAfisare != null)
            {
                this.Controls.Remove(panelAfisare);
                panelAfisare.Dispose();
            }

            // Calculează 1/3 din înălțimea formularului
            int oneThirdHeight = this.ClientSize.Height / 3;

            panelAfisare = new Panel
            {
                Location = new Point(panelMeniu.Right, oneThirdHeight), // Începe de la 1/3 din înălțime
                Size = new Size(this.ClientSize.Width - panelMeniu.Width, this.ClientSize.Height - oneThirdHeight),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(panelAfisare);
            panelAfisare.BringToFront();

            // PANEL PENTRU RADIOBUTOANE (acum va fi în partea de sus a ecranului)
            Panel panelOptiuni = new Panel
            {
                Location = new Point(panelMeniu.Right, 0), // Plasează deasupra DataGridView
                Size = new Size(this.ClientSize.Width - panelMeniu.Width, oneThirdHeight),
                BackColor = Color.LightGray
            };
            this.Controls.Add(panelOptiuni);
            panelOptiuni.BringToFront();


            Label lblTitlu = new Label
            {
                Text = "Căutare Programări",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(20, 10)
            };
            panelOptiuni.Controls.Add(lblTitlu);

            RadioButton rbData = new RadioButton
            {
                Text = "Caută după dată",
                Location = new Point(20, 45),
                Width = 180
            };
            panelOptiuni.Controls.Add(rbData);

            RadioButton rbIDNP = new RadioButton
            {
                Text = "Caută după IDNP",
                Location = new Point(220, 45),
                Width = 180
            };
            panelOptiuni.Controls.Add(rbIDNP);

            DateTimePicker datePicker = new DateTimePicker
            {
                Location = new Point(20, 80),
                Width = 250,
                Visible = false
            };
            panelOptiuni.Controls.Add(datePicker);

            TextBox txtIDNP = new TextBox
            {
                Location = new Point(20, 80),
                Width = 250,
                Visible = false
            };
            panelOptiuni.Controls.Add(txtIDNP);

            Button btnCauta = new Button
            {
                Text = "🔍 Caută",
                Location = new Point(300, 75),
                Width = 170,
                Height = 35
            };
            SeteazaStilButoane(btnCauta);
            panelOptiuni.Controls.Add(btnCauta);

            rbData.CheckedChanged += (s, ev) => { datePicker.Visible = rbData.Checked; txtIDNP.Visible = false; };
            rbIDNP.CheckedChanged += (s, ev) => { txtIDNP.Visible = rbIDNP.Checked; datePicker.Visible = false; };

            // DATAGRIDVIEW DEDESUBT
            dgvProgramari = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, // Schimbat din AllCells în Fill
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                ScrollBars = ScrollBars.Both,
                ReadOnly = true,
                AllowUserToResizeColumns = true, // Permite utilizatorului să redimensioneze coloanele
                AllowUserToResizeRows = false,   // Opțional - pentru a preveni redimensionarea rândurilor
                ColumnHeadersHeight = 35,        // Înălțime mai mare pentru header-e
            };
            panelAfisare.Controls.Add(dgvProgramari);

            // BUTON CĂUTARE
            btnCauta.Click += (s, ev) =>
            {
                if (rbData.Checked)
                {
                    IncarcaProgramari("data", datePicker.Value.ToString("yyyy-MM-dd"));
                }
                else if (rbIDNP.Checked && !string.IsNullOrWhiteSpace(txtIDNP.Text))
                {
                    IncarcaProgramari("idnp", txtIDNP.Text);
                }
                else
                {
                    MessageBox.Show("Introduceți un criteriu valid de căutare!");
                }
            };
        }


        private void AdjustDataGridViewColumns()
        {
            if (dgvProgramari != null && dgvProgramari.Columns.Count > 0)
            {
                // Setăm lățimea coloanelor proporțional cu conținutul
                foreach (DataGridViewColumn column in dgvProgramari.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    int width = column.Width;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    column.Width = width + 20; // Adăugăm un padding de 20 de pixeli
                }

                // Forțăm DataGridView să ocupe tot spațiul disponibil
                dgvProgramari.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }


        private void istoric_Click(object sender, EventArgs e)
        {
            AscundePanouri();

            if (panelAfisare != null)
            {
                this.Controls.Remove(panelAfisare);
                panelAfisare.Dispose();
            }

            // Calculează 1/3 din înălțimea formularului
            int oneThirdHeight = this.ClientSize.Height / 3;

            // Panel pentru afișarea istoricului (joasă)
            panelAfisare = new Panel
            {
                Location = new Point(panelMeniu.Right, oneThirdHeight), // Începe de la dreapta meniului
                Size = new Size(this.ClientSize.Width - panelMeniu.Width, this.ClientSize.Height - oneThirdHeight), // Pătrunde până la fundul ferestrei
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(panelAfisare);
            panelAfisare.BringToFront();

            // Panel pentru căutare (sus)
            Panel panelCautare = new Panel
            {
                Location = new Point(panelMeniu.Right, 0), // Începe din partea de sus, la dreapta meniului
                Size = new Size(this.ClientSize.Width - panelMeniu.Width, oneThirdHeight), // Ocupă o treime din înălțimea formularului
                BackColor = Color.LightGray,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right // Se întinde pe toată lățimea disponibilă
            };
            this.Controls.Add(panelCautare);
            panelCautare.BringToFront();

            // Elementele UI pentru căutare
            Label lblTitlu = new Label
            {
                Text = "Căutare Istoric Pacient",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            panelCautare.Controls.Add(lblTitlu);

            Label lblIDNP = new Label
            {
                Text = "IDNP Pacient:",
                Font = new Font("Arial", 10),
                Location = new Point(20, 60),
                AutoSize = true
            };
            panelCautare.Controls.Add(lblIDNP);

            TextBox txtIDNP = new TextBox
            {
                Location = new Point(120, 60),
                Width = 200,
                Font = new Font("Arial", 10)
            };
            panelCautare.Controls.Add(txtIDNP);

            Button btnCauta = new Button
            {
                Text = "🔍 Caută Istoric",
                Location = new Point(350, 55),
                Width = 180,
                Height = 35,
                Font = new Font("Arial", 10)
            };
            SeteazaStilButoane(btnCauta);
            panelCautare.Controls.Add(btnCauta);

            // DataGridView pentru afișarea istoricului
            DataGridView dgvIstoric = new DataGridView
            {
                Dock = DockStyle.Fill, // Setează DataGridView să ocupe tot spațiul disponibil în panelAfisare
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                ScrollBars = ScrollBars.Both,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            panelAfisare.Controls.Add(dgvIstoric);

            // Stilizare DataGridView
            dgvIstoric.EnableHeadersVisualStyles = false;
            dgvIstoric.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            dgvIstoric.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dgvIstoric.RowsDefaultCellStyle.Font = new Font("Arial", 9);
            dgvIstoric.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;

            // Funcționalitatea butonului de căutare
            btnCauta.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtIDNP.Text) || txtIDNP.Text.Length != 13)
                {
                    MessageBox.Show("Introduceți un IDNP valid (13 caractere)!");
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        string query = @"
        SELECT 
            P.IdProgramare,
            P.DataProgramare,
            P.OraProgramare,
            M.Nume + ' ' + M.Prenume AS Medic,
            P.StatusProgramare,
            Pa.Nume + ' ' + Pa.Prenume AS Pacient,
            Pa.Gen,
            Pa.NrTelefon
        FROM Programari P
        JOIN Medic M ON P.IdMedic = M.IdMedic
        JOIN Pacient Pa ON P.IdPacient = Pa.IdPacient
        WHERE Pa.IdPacient = @IDNP
        ORDER BY P.DataProgramare DESC, P.OraProgramare DESC";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@IDNP", txtIDNP.Text);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Nu s-au găsit înregistrări pentru acest pacient.");
                            return;
                        }

                        dgvIstoric.DataSource = dt;

                        // Formatare coloane
                        dgvIstoric.Columns["IdProgramare"].HeaderText = "ID Programare";
                        dgvIstoric.Columns["DataProgramare"].HeaderText = "Data";
                        dgvIstoric.Columns["OraProgramare"].HeaderText = "Ora";
                        dgvIstoric.Columns["Medic"].HeaderText = "Medic";
                        dgvIstoric.Columns["StatusProgramare"].HeaderText = "Status";
                        dgvIstoric.Columns["Pacient"].HeaderText = "Pacient";
                        dgvIstoric.Columns["Gen"].HeaderText = "Gen";
                        dgvIstoric.Columns["NrTelefon"].HeaderText = "Telefon";

                        // Ajustare automată a lățimii coloanelor
                        foreach (DataGridViewColumn column in dgvIstoric.Columns)
                        {
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            int width = column.Width;
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            column.Width = width + 20;
                        }
                        dgvIstoric.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la încărcarea istoricului: {ex.Message}");
                }
            };
        }


        private void panelMeniu_Paint(object sender, PaintEventArgs e)
        {
            SaveInitialSizes();
        }

        private void back_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
