using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Studiu_Individual_1
{
    public partial class ReceptionistMenu : Form
    {
        private Dictionary<Control, Rectangle> initialSizes = new Dictionary<Control, Rectangle>();
        private float initialFormWidth, initialFormHeight;
        private string connString;
        string receptionistUsername;

        private void SaveInitialSizes()
        {
            initialFormWidth = this.Width;
            initialFormHeight = this.Height;
            initialSizes.Clear();

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

        public ReceptionistMenu(string connString, string receptionistUsername)
        {
            this.connString = connString;
            this.receptionistUsername = receptionistUsername;
            InitializeComponent();
            SaveInitialSizes();
            this.Resize += (s, e) => ResizeControls();
            menuPanel.BackColor = Color.FromArgb(30, 30, 45);
            programare.BackColor = Color.FromArgb(70, 70, 100);
            cauta1.BackColor = Color.FromArgb(70, 70, 100);
            cauta2.BackColor = Color.FromArgb(70, 70, 100);
            back.BackColor = Color.FromArgb(70, 70, 100);
            programare.FlatStyle = FlatStyle.Flat;
            cauta1.FlatStyle = FlatStyle.Flat;
            cauta2.FlatStyle = FlatStyle.Flat;
            back.FlatStyle = FlatStyle.Flat;
            back.FlatAppearance.BorderSize = 0;
            cauta1.FlatAppearance.BorderSize = 0;
            cauta2.FlatAppearance.BorderSize = 0;
            cauta1.MouseEnter += (s, e) => cauta1.BackColor = Color.FromArgb(70, 70, 100);
            cauta1.MouseLeave += (s, e) => cauta1.BackColor = Color.FromArgb(50, 50, 70);
            cauta2.FlatAppearance.BorderSize = 0;
            cauta2.MouseEnter += (s, e) => cauta2.BackColor = Color.FromArgb(70, 70, 100);
            cauta2.MouseLeave += (s, e) => cauta2.BackColor = Color.FromArgb(50, 50, 70);
            programare.FlatAppearance.BorderSize = 0;
            programare.MouseEnter += (s, e) => programare.BackColor = Color.FromArgb(70, 70, 100);
            programare.MouseLeave += (s, e) => programare.BackColor = Color.FromArgb(50, 50, 70);
            back.FlatAppearance.BorderSize = 0;
            back.MouseEnter += (s, e) => back.BackColor = Color.FromArgb(70, 70, 100);
            back.MouseLeave += (s, e) => back.BackColor = Color.FromArgb(50, 50, 70);
        }

        private void StergePanouri()
        {
            // List of panels to keep (only menuPanel)
            var excludedPanels = new List<string> { "menuPanel" };

            // Create a list of panels to remove (to avoid modification during iteration)
            var panelsToRemove = new List<Panel>();
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Panel panel && !excludedPanels.Contains(panel.Name))
                {
                    panelsToRemove.Add(panel);
                }
            }

            // Remove the panels
            foreach (var panel in panelsToRemove)
            {
                this.Controls.Remove(panel);
                panel.Dispose();
            }

            // Reset panelDetalii since we've disposed of it
            panelDetalii = null;
        }

        private Panel panelDetalii;

        private void programare_Click(object sender, EventArgs e)
        {
            StergePanouri(); // This will now properly clear all panels except menuPanel
            SaveInitialSizes();

            // Create main panel
            Panel panelProgramare = new Panel
            {
                Name = "panelProgramare",
                Width = 700,
                Height = 600,
                BackColor = Color.LightGray,
                Location = new Point((this.ClientSize.Width - 700) / 2 + 150, (this.ClientSize.Height - 600) / 2),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelProgramare);
            panelProgramare.BringToFront();

            Font fontMare = new Font("Arial", 11, FontStyle.Bold);

            // Radio buttons panel
            Panel panelRadio = new Panel
            {
                Name = "panelRadio",
                Width = 650,
                Height = 40,
                Location = new Point(25, 20),
                BackColor = Color.Transparent
            };
            panelProgramare.Controls.Add(panelRadio);

            // Create radio buttons
            RadioButton rbAdauga = new RadioButton
            {
                Name = "rbAdauga",
                Text = "Adăugare programare",
                Location = new Point(20, 10),
                AutoSize = true,
                Font = fontMare,
                Tag = "Adăugare"
            };

            RadioButton rbModifica = new RadioButton
            {
                Name = "rbModifica",
                Text = "Modificare programare",
                Location = new Point(250, 10),
                AutoSize = true,
                Font = fontMare,
                Tag = "Modificare"
            };

            RadioButton rbSterge = new RadioButton
            {
                Name = "rbSterge",
                Text = "Ștergere programare",
                Location = new Point(480, 10),
                AutoSize = true,
                Font = fontMare,
                Tag = "Ștergere"
            };

            panelRadio.Controls.Add(rbAdauga);
            panelRadio.Controls.Add(rbModifica);
            panelRadio.Controls.Add(rbSterge);

            // Create details panel
            panelDetalii = new Panel
            {
                Name = "panelDetalii",
                Location = new Point(25, 80),
                Width = 650,
                Height = 450,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };
            panelProgramare.Controls.Add(panelDetalii);

            // Assign event handlers
            rbAdauga.CheckedChanged += RadioButton_CheckedChanged;
            rbModifica.CheckedChanged += RadioButton_CheckedChanged;
            rbSterge.CheckedChanged += RadioButton_CheckedChanged;
        }

        private void AfiseazaCampuri(string tipActiune)
        {
            if (panelDetalii == null) return;

            panelDetalii.Controls.Clear();
            SaveInitialSizes();

            Font fontMare = new Font("Arial", 11, FontStyle.Bold);

            // Common controls for all actions
            TextBox txtIDNP = new TextBox
            {
                Location = new Point(200, 40),
                Width = 250,
                MaxLength = 13,
                Tag = "txtIDNP"
            };
            Label lblIDNP = new Label
            {
                Text = "IDNP Pacient:",
                Location = new Point(10, 40),
                AutoSize = true
            };
            panelDetalii.Controls.Add(lblIDNP);
            panelDetalii.Controls.Add(txtIDNP);

            Label lblInfo = new Label
            {
                Text = "Completați detaliile:",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = fontMare
            };
            panelDetalii.Controls.Add(lblInfo);

            // Patient details panel (shown only when adding new patient)
            Panel panelPacient = new Panel
            {
                Location = new Point(10, 70),
                Width = 630,
                Height = 200,
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            // Initialize common patient controls (used in multiple actions)
            TextBox txtNume = new TextBox { Location = new Point(150, 20), Width = 250, MaxLength = 15 };
            TextBox txtPrenume = new TextBox { Location = new Point(150, 50), Width = 250, MaxLength = 15 };
            ComboBox cbGen = new ComboBox { Location = new Point(150, 80), Width = 50, DropDownStyle = ComboBoxStyle.DropDownList };
            TextBox txtAdresa = new TextBox { Location = new Point(150, 110), Width = 250, MaxLength = 30 };
            TextBox txtTelefon = new TextBox { Location = new Point(150, 140), Width = 150, MaxLength = 9 };
            TextBox txtEmail = new TextBox { Location = new Point(150, 170), Width = 250, MaxLength = 20 };

            // Add patient controls to patient panel
            panelPacient.Controls.AddRange(new Control[]
            {
        new Label { Text = "Nume:", Location = new Point(10, 20), AutoSize = true },
        txtNume,
        new Label { Text = "Prenume:", Location = new Point(10, 50), AutoSize = true },
        txtPrenume,
        new Label { Text = "Gen:", Location = new Point(10, 80), AutoSize = true },
        cbGen,
        new Label { Text = "Adresa:", Location = new Point(10, 110), AutoSize = true },
        txtAdresa,
        new Label { Text = "Telefon:", Location = new Point(10, 140), AutoSize = true },
        txtTelefon,
        new Label { Text = "Email:", Location = new Point(10, 170), AutoSize = true },
        txtEmail
            });
            cbGen.Items.AddRange(new string[] { "M", "F" });
            panelDetalii.Controls.Add(panelPacient);

            // Action-specific UI
            if (tipActiune == "Adăugare")
            {
                SetupAdaugareUI();
            }
            else if (tipActiune == "Modificare")
            {
                SetupModificareUI();
            }
            else if (tipActiune == "Ștergere")
            {
                SetupStergereUI();
            }

            // Helper methods for each action type
            void SetupAdaugareUI()
            {
                Button btnCheckIDNP = new Button
                {
                    Text = "Verifică IDNP",
                    Location = new Point(460, 40),
                    Width = 100,
                    Height = 25
                };
                panelDetalii.Controls.Add(btnCheckIDNP);

                btnCheckIDNP.Click += (s, e) => CheckIDNP(txtIDNP.Text.Trim());

                // Add confirm patient button (initially hidden)
                Button btnConfirmaPacient = new Button
                {
                    Text = "Confirmă Pacient",
                    Location = new Point(460, 70),
                    Width = 100,
                    Height = 25,
                    Visible = false,
                    Name = "btnConfirmaPacient"
                };
                btnConfirmaPacient.Click += (s, e) => SavePatientData();
                panelDetalii.Controls.Add(btnConfirmaPacient);

                int yPos = 280;
                var (cbMedici, cbFiliala, datePicker, cbOra) = CreateAppointmentControls(yPos);
                panelDetalii.Controls.AddRange(new Control[] { cbMedici, cbFiliala, datePicker, cbOra });

                Button btnAdauga = new Button
                {
                    Text = "Adaugă Programare",
                    Location = new Point(225, 450),
                    Width = 200,
                    Height = 40,
                    Font = fontMare,
                    BackColor = Color.Gray,
                    ForeColor = Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    Name = "btnAdauga"
                };
                btnAdauga.Click += (s, e) => ProcessAppointmentAction("Adăugare");
                panelDetalii.Controls.Add(btnAdauga);
                panelDetalii.Height = btnAdauga.Bottom + 20;
            }
            void SavePatientData()
            {
                string idnp = txtIDNP.Text.Trim();
                string nume = txtNume.Text.Trim();
                string prenume = txtPrenume.Text.Trim();
                string gen = cbGen.SelectedItem?.ToString();
                string adresa = txtAdresa.Text.Trim();
                string telefon = txtTelefon.Text.Trim();
                string email = txtEmail.Text.Trim();

                if (string.IsNullOrWhiteSpace(nume) || string.IsNullOrWhiteSpace(prenume) ||
                    string.IsNullOrWhiteSpace(gen) || string.IsNullOrWhiteSpace(adresa) ||
                    string.IsNullOrWhiteSpace(telefon))
                {
                    MessageBox.Show("Completați toate câmpurile obligatorii pentru pacient.");
                    return;
                }

                if (telefon.Length != 9)
                {
                    MessageBox.Show("Numărul de telefon trebuie să aibă exact 9 cifre.");
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        SqlCommand insertPacient = new SqlCommand(
                            "INSERT INTO Pacient (IdPacient, Nume, Prenume, Gen, Adresa, NrTelefon, Email) " +
                            "VALUES (@idnp, @nume, @prenume, @gen, @adresa, @telefon, @email)", conn);

                        insertPacient.Parameters.AddWithValue("@idnp", idnp);
                        insertPacient.Parameters.AddWithValue("@nume", nume);
                        insertPacient.Parameters.AddWithValue("@prenume", prenume);
                        insertPacient.Parameters.AddWithValue("@gen", gen);
                        insertPacient.Parameters.AddWithValue("@adresa", adresa);
                        insertPacient.Parameters.AddWithValue("@telefon", telefon);
                        insertPacient.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? DBNull.Value : (object)email);

                        insertPacient.ExecuteNonQuery();
                        MessageBox.Show("Pacientul a fost adăugat cu succes.");

                        // Hide patient panel and confirm button
                        panelPacient.Visible = false;
                        var btnConfirma = panelDetalii.Controls.Find("btnConfirmaPacient", true).FirstOrDefault();
                        if (btnConfirma != null) btnConfirma.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la salvarea pacientului: " + ex.Message);
                }
            }
            void SetupModificareUI()
            {
                Panel panelCautare = CreateSearchPanel();
                Button btnCauta = new Button
                {
                    Text = "Caută Programare",
                    Location = new Point(300, 30),
                    Width = 150,
                    Height = 30
                };
                panelCautare.Controls.Add(btnCauta);

                btnCauta.Click += (s, e) => SearchAppointmentForModification();
                panelDetalii.Controls.Add(panelCautare);
            }

            void SetupStergereUI()
            {
                Panel panelCautare = CreateSearchPanel();
                Button btnCauta = new Button
                {
                    Text = "Caută Programare",
                    Location = new Point(300, 30),
                    Width = 150,
                    Height = 30
                };
                panelCautare.Controls.Add(btnCauta);

                btnCauta.Click += (s, e) => SearchAppointmentForDeletion();
                panelDetalii.Controls.Add(panelCautare);
            }

            (ComboBox, ComboBox, DateTimePicker, ComboBox) CreateAppointmentControls(int yPos)
            {
                // Medic ComboBox
                Label lblMedic = new Label { Text = "Medic:", Location = new Point(10, yPos), AutoSize = true };
                ComboBox cbMedici = new ComboBox
                {
                    Location = new Point(200, yPos),
                    Width = 250,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Name = "cbMedici" // Add name
                };
                panelDetalii.Controls.Add(lblMedic);
                yPos += 40;

                // Filiala ComboBox
                Label lblFiliala = new Label { Text = "Filiala:", Location = new Point(10, yPos), AutoSize = true };
                ComboBox cbFiliala = new ComboBox
                {
                    Location = new Point(200, yPos),
                    Width = 250,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Name = "cbFiliala" // Add name
                };
                panelDetalii.Controls.Add(lblFiliala);
                yPos += 40;

                // Date Picker
                Label lblData = new Label { Text = "Data Programării:", Location = new Point(10, yPos), AutoSize = true };
                DateTimePicker datePicker = new DateTimePicker
                {
                    Location = new Point(200, yPos),
                    Width = 150,
                    Format = DateTimePickerFormat.Short,
                    MinDate = DateTime.Today,
                    Name = "datePicker" // Add name
                };
                panelDetalii.Controls.Add(lblData);
                yPos += 40;

                // Time ComboBox
                Label lblOra = new Label { Text = "Ora Programării:", Location = new Point(10, yPos), AutoSize = true };
                ComboBox cbOra = new ComboBox
                {
                    Location = new Point(200, yPos),
                    Width = 100,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Name = "cbOra" // Add name
                };

                // Populate time slots
                for (int hour = 8; hour <= 18; hour++)
                {
                    cbOra.Items.Add($"{hour:00}:00");
                    if (hour != 18) cbOra.Items.Add($"{hour:00}:30");
                }
                cbOra.SelectedIndex = 2;
                panelDetalii.Controls.Add(lblOra);
                panelDetalii.Controls.Add(cbOra);

                // Load data for comboboxes
                LoadComboBoxData(cbMedici, "SELECT IdMedic, Nume + ' ' + Prenume AS NumeComplet FROM Medic");
                LoadComboBoxData(cbFiliala, "SELECT IdFiliala, Adresa FROM Filiala");

                return (cbMedici, cbFiliala, datePicker, cbOra);
            }

            Panel CreateSearchPanel()
            {
                Panel panelCautare = new Panel
                {
                    Name = "panelCautare", // Adaugă această linie
                    Location = new Point(10, 70),
                    Width = 630,
                    Height = 100,
                    BackColor = Color.WhiteSmoke,
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Date control
                Label lblData = new Label { Text = "Data Programării:", Location = new Point(10, 20), AutoSize = true };
                DateTimePicker datePicker = new DateTimePicker
                {
                    Location = new Point(150, 20),
                    Width = 150,
                    Format = DateTimePickerFormat.Short
                };
                panelCautare.Controls.Add(lblData);
                panelCautare.Controls.Add(datePicker);

                // Time control
                Label lblOra = new Label { Text = "Ora Programării:", Location = new Point(10, 50), AutoSize = true };
                ComboBox cbOra = new ComboBox
                {
                    Location = new Point(150, 50),
                    Width = 100,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                // Populate time slots
                for (int hour = 8; hour <= 18; hour++)
                {
                    cbOra.Items.Add($"{hour:00}:00");
                    if (hour != 18) cbOra.Items.Add($"{hour:00}:30");
                }
                panelCautare.Controls.Add(lblOra);
                panelCautare.Controls.Add(cbOra);

                return panelCautare;
            }

            // Database and business logic methods
            void CheckIDNP(string idnp)
            {
                if (idnp.Length != 13)
                {
                    MessageBox.Show("IDNP trebuie să aibă exact 13 caractere.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Pacient WHERE IdPacient = @idnp", conn);
                        cmd.Parameters.AddWithValue("@idnp", idnp);
                        int count = (int)cmd.ExecuteScalar();

                        panelPacient.Visible = (count == 0);

                        // Show/hide confirm button based on patient existence
                        var btnConfirma = panelDetalii.Controls.Find("btnConfirmaPacient", true).FirstOrDefault();
                        if (btnConfirma != null) btnConfirma.Visible = (count == 0);

                        if (count > 0)
                        {
                            MessageBox.Show("Pacientul există deja în baza de date.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Eroare la verificare IDNP: " + ex.Message);
                    }
                }
            }

            void LoadComboBoxData(ComboBox comboBox, string query)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        comboBox.DataSource = dt;
                        comboBox.DisplayMember = dt.Columns[1].ColumnName;
                        comboBox.ValueMember = dt.Columns[0].ColumnName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Eroare la încărcarea datelor: {ex.Message}");
                    }
                }
            }

            void ProcessAppointmentAction(string actionType)
            {
                string idnp = txtIDNP.Text.Trim();
                string nume = txtNume.Text.Trim();
                string prenume = txtPrenume.Text.Trim();
                string gen = cbGen.SelectedItem?.ToString();
                string adresa = txtAdresa.Text.Trim();
                string telefon = txtTelefon.Text.Trim();
                string email = txtEmail.Text.Trim();

                if (string.IsNullOrWhiteSpace(idnp) || idnp.Length != 13)
                {
                    MessageBox.Show("Introduceți un IDNP valid (13 caractere).");
                    return;
                }

                if (panelPacient.Visible)
                {
                    if (string.IsNullOrWhiteSpace(nume) || string.IsNullOrWhiteSpace(prenume) ||
                        string.IsNullOrWhiteSpace(gen) || string.IsNullOrWhiteSpace(adresa) ||
                        string.IsNullOrWhiteSpace(telefon))
                    {
                        MessageBox.Show("Completați toate câmpurile obligatorii pentru pacient.");
                        return;
                    }

                    if (telefon.Length != 9)
                    {
                        MessageBox.Show("Numărul de telefon trebuie să aibă exact 9 cifre.");
                        return;
                    }
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();

                        // Check if patient exists
                        SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Pacient WHERE IdPacient = @idnp", conn);
                        checkCmd.Parameters.AddWithValue("@idnp", idnp);
                        bool patientExists = (int)checkCmd.ExecuteScalar() > 0;

                        if (!patientExists && panelPacient.Visible)
                        {
                            // Insert new patient
                            SqlCommand insertPacient = new SqlCommand(
                                "INSERT INTO Pacient (IdPacient, Nume, Prenume, Gen, Adresa, NrTelefon, Email) " +
                                "VALUES (@idnp, @nume, @prenume, @gen, @adresa, @telefon, @email)", conn);

                            insertPacient.Parameters.AddWithValue("@idnp", idnp);
                            insertPacient.Parameters.AddWithValue("@nume", nume);
                            insertPacient.Parameters.AddWithValue("@prenume", prenume);
                            insertPacient.Parameters.AddWithValue("@gen", gen);
                            insertPacient.Parameters.AddWithValue("@adresa", adresa);
                            insertPacient.Parameters.AddWithValue("@telefon", telefon);
                            insertPacient.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? DBNull.Value : (object)email);

                            insertPacient.ExecuteNonQuery();
                        }
                        else if (!patientExists)
                        {
                            MessageBox.Show("Pacientul nu există în baza de date. Completați detaliile pacientului.");
                            return;
                        }

                        if (actionType == "Adăugare")
                        {
                            // Get appointment controls from panel
                            var controls = panelDetalii.Controls;
                            var cbMedici = (ComboBox)controls.Find("cbMedici", true).FirstOrDefault();
                            var cbFiliala = (ComboBox)controls.Find("cbFiliala", true).FirstOrDefault();
                            var datePicker = (DateTimePicker)controls.Find("datePicker", true).FirstOrDefault();
                            var cbOra = (ComboBox)controls.Find("cbOra", true).FirstOrDefault();

                            if (cbMedici == null || cbFiliala == null || datePicker == null || cbOra == null)
                            {
                                MessageBox.Show("Nu s-au găsit toate controalele necesare.");
                                return;
                            }

                            string idMedic = cbMedici.SelectedValue?.ToString();
                            string idFiliala = cbFiliala.SelectedValue?.ToString();
                            DateTime dataProgramare = datePicker.Value.Date;
                            string oraProgramare = cbOra.SelectedItem?.ToString();

                            if (string.IsNullOrEmpty(idMedic) || string.IsNullOrEmpty(idFiliala) || string.IsNullOrEmpty(oraProgramare))
                            {
                                MessageBox.Show("Completați toate câmpurile pentru programare.");
                                return;
                            }

                            // Check for existing appointment
                            SqlCommand checkAppointment = new SqlCommand(
                                "SELECT COUNT(*) FROM Programari WHERE " +
                                "IdMedic = @idMedic AND DataProgramare = @data AND OraProgramare = @ora",
                                conn);
                            checkAppointment.Parameters.AddWithValue("@idMedic", idMedic);
                            checkAppointment.Parameters.AddWithValue("@data", dataProgramare);
                            checkAppointment.Parameters.AddWithValue("@ora", oraProgramare);

                            if ((int)checkAppointment.ExecuteScalar() > 0)
                            {
                                MessageBox.Show("Medicul are deja o programare la această dată și oră.");
                                return;
                            }

                            // Insert new appointment
                            SqlCommand insertProgramare = new SqlCommand(
                                "INSERT INTO Programari (IdPacient, IdMedic, IdFiliala, DataProgramare, OraProgramare, StatusProgramare) " +
                                "VALUES (@idPacient, @idMedic, @idFiliala, @data, @ora, @status)",
                                conn);

                            insertProgramare.Parameters.AddWithValue("@idPacient", idnp);
                            insertProgramare.Parameters.AddWithValue("@idMedic", idMedic);
                            insertProgramare.Parameters.AddWithValue("@idFiliala", idFiliala);
                            insertProgramare.Parameters.AddWithValue("@data", dataProgramare);
                            insertProgramare.Parameters.AddWithValue("@ora", oraProgramare);
                            insertProgramare.Parameters.AddWithValue("@status", "Neconfirmată");

                            insertProgramare.ExecuteNonQuery();
                            MessageBox.Show("Programare adăugată cu succes.");
                            ResetControls();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Eroare: " + ex.Message);
                    }
                }
            }

            void SearchAppointmentForModification()
            {
                var panelCautare = panelDetalii.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "panelCautare");
                if (panelCautare == null) return;

                var textIDNP = (TextBox)panelDetalii.Controls.Find("txtIDNP", true).FirstOrDefault();
                var datePicker = panelCautare.Controls.OfType<DateTimePicker>().FirstOrDefault();
                var cbOra = panelCautare.Controls.OfType<ComboBox>().FirstOrDefault();

                if (txtIDNP == null || datePicker == null || cbOra == null)
                {
                    MessageBox.Show("Nu s-au găsit toate controalele necesare.");
                    return;
                }

                string idnp = txtIDNP.Text.Trim();
                DateTime data = datePicker.Value.Date;
                string ora = cbOra.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(idnp) || idnp.Length != 13)
                {
                    MessageBox.Show("IDNP invalid. Trebuie să conțină exact 13 caractere.");
                    return;
                }

                if (string.IsNullOrEmpty(ora))
                {
                    MessageBox.Show("Selectați ora programării.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();

                        // Verify patient exists
                        SqlCommand checkPacient = new SqlCommand(
                            "SELECT COUNT(*) FROM Pacient WHERE IdPacient = @idnp", conn);
                        checkPacient.Parameters.AddWithValue("@idnp", idnp);

                        if ((int)checkPacient.ExecuteScalar() == 0)
                        {
                            MessageBox.Show("Pacientul nu există în baza de date.");
                            return;
                        }

                        // Get appointment details
                        SqlCommand cmd = new SqlCommand(
                            "SELECT p.IdMedic, p.IdFiliala, p.StatusProgramare, " +
                            "m.Nume + ' ' + m.Prenume AS NumeMedic, f.Adresa AS AdresaFiliala " +
                            "FROM Programari p " +
                            "JOIN Medic m ON p.IdMedic = m.IdMedic " +
                            "JOIN Filiala f ON p.IdFiliala = f.IdFiliala " +
                            "WHERE p.IdPacient = @idnp AND p.DataProgramare = @data AND p.OraProgramare = @ora", conn);

                        cmd.Parameters.AddWithValue("@idnp", idnp);
                        cmd.Parameters.AddWithValue("@data", data);
                        cmd.Parameters.AddWithValue("@ora", ora);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                panelDetalii.Controls.Remove(panelCautare);

                                // Display original appointment info
                                int yPos = 70;
                                Label lblOriginal = new Label
                                {
                                    Text = $"Programare originală: {data.ToShortDateString()} {ora}",
                                    Location = new Point(10, yPos),
                                    AutoSize = true,
                                    Font = new Font("Arial", 10, FontStyle.Bold),
                                    Name = "lblOriginal"
                                };
                                panelDetalii.Controls.Add(lblOriginal);
                                yPos += 30;

                                // Display current doctor
                                Label lblMedic = new Label
                                {
                                    Text = "Medic: " + reader["NumeMedic"].ToString(),
                                    Location = new Point(10, yPos),
                                    AutoSize = true,
                                    Name = "lblMedic"
                                };
                                panelDetalii.Controls.Add(lblMedic);
                                yPos += 30;

                                // Display current branch
                                Label lblFiliala = new Label
                                {
                                    Text = "Filială: " + reader["AdresaFiliala"].ToString(),
                                    Location = new Point(10, yPos),
                                    AutoSize = true,
                                    Name = "lblFiliala"
                                };
                                panelDetalii.Controls.Add(lblFiliala);
                                yPos += 40;

                                // Add modification controls
                                Label lblModificare = new Label
                                {
                                    Text = "Modifică programarea:",
                                    Location = new Point(10, yPos),
                                    AutoSize = true,
                                    Font = fontMare,
                                    Name = "lblModificare"
                                };
                                panelDetalii.Controls.Add(lblModificare);
                                yPos += 30;

                                // New doctor combobox
                                Label lblMedicNou = new Label
                                {
                                    Text = "Medic nou:",
                                    Location = new Point(10, yPos),
                                    AutoSize = true,
                                    Name = "lblMedicNou"
                                };
                                ComboBox cbMediciModif = new ComboBox
                                {
                                    Location = new Point(150, yPos),
                                    Width = 300,
                                    DropDownStyle = ComboBoxStyle.DropDownList,
                                    Name = "cbMediciModif"
                                };
                                LoadComboBoxData(cbMediciModif, "SELECT IdMedic, Nume + ' ' + Prenume AS NumeComplet FROM Medic");
                                cbMediciModif.SelectedValue = reader["IdMedic"];
                                panelDetalii.Controls.Add(lblMedicNou);
                                panelDetalii.Controls.Add(cbMediciModif);
                                yPos += 40;

                                // New branch combobox
                                Label lblFilialaNoua = new Label
                                {
                                    Text = "Filială nouă:",
                                    Location = new Point(10, yPos),
                                    AutoSize = true,
                                    Name = "lblFilialaNoua"
                                };
                                ComboBox cbFilialaModif = new ComboBox
                                {
                                    Location = new Point(150, yPos),
                                    Width = 300,
                                    DropDownStyle = ComboBoxStyle.DropDownList,
                                    Name = "cbFilialaModif"
                                };
                                LoadComboBoxData(cbFilialaModif, "SELECT IdFiliala, Adresa FROM Filiala");
                                cbFilialaModif.SelectedValue = reader["IdFiliala"];
                                panelDetalii.Controls.Add(lblFilialaNoua);
                                panelDetalii.Controls.Add(cbFilialaModif);
                                yPos += 40;

                                // New date
                                Label lblDataNoua = new Label
                                {
                                    Text = "Data nouă:",
                                    Location = new Point(10, yPos),
                                    AutoSize = true,
                                    Name = "lblDataNoua"
                                };
                                DateTimePicker datePickerNou = new DateTimePicker
                                {
                                    Location = new Point(150, yPos),
                                    Width = 150,
                                    Format = DateTimePickerFormat.Short,
                                    MinDate = DateTime.Today,
                                    Value = data,
                                    Name = "datePickerNou"
                                };
                                panelDetalii.Controls.Add(lblDataNoua);
                                panelDetalii.Controls.Add(datePickerNou);
                                yPos += 40;

                                // New time
                                Label lblOraNoua = new Label
                                {
                                    Text = "Ora nouă:",
                                    Location = new Point(10, yPos),
                                    AutoSize = true,
                                    Name = "lblOraNoua"
                                };
                                ComboBox cbOraNoua = new ComboBox
                                {
                                    Location = new Point(150, yPos),
                                    Width = 100,
                                    DropDownStyle = ComboBoxStyle.DropDownList,
                                    Name = "cbOraNoua"
                                };
                                for (int hour = 8; hour <= 18; hour++)
                                {
                                    cbOraNoua.Items.Add($"{hour:00}:00");
                                    if (hour != 18) cbOraNoua.Items.Add($"{hour:00}:30");
                                }
                                cbOraNoua.SelectedItem = ora;
                                panelDetalii.Controls.Add(lblOraNoua);
                                panelDetalii.Controls.Add(cbOraNoua);
                                yPos += 40;

                                // Status combobox
                                Label lblStatus = new Label
                                {
                                    Text = "Status:",
                                    Location = new Point(10, yPos),
                                    AutoSize = true,
                                    Name = "lblStatus"
                                };
                                ComboBox cbStatusModif = new ComboBox
                                {
                                    Location = new Point(150, yPos),
                                    Width = 150,
                                    DropDownStyle = ComboBoxStyle.DropDownList,
                                    Name = "cbStatusModif"
                                };
                                cbStatusModif.Items.AddRange(new string[] { "Neconfirmată", "Confirmată", "Anulată" });
                                cbStatusModif.SelectedItem = reader["StatusProgramare"].ToString();
                                panelDetalii.Controls.Add(lblStatus);
                                panelDetalii.Controls.Add(cbStatusModif);
                                yPos += 50;

                                // Save button
                                Button btnSalveaza = new Button
                                {
                                    Text = "Salvează Modificările",
                                    Location = new Point(150, yPos),
                                    Width = 200,
                                    Height = 40,
                                    BackColor = Color.LightGreen,
                                    Name = "btnSalveaza"
                                };
                                btnSalveaza.Click += (s2, e2) => SaveAppointmentModification(idnp, data, ora);
                                panelDetalii.Controls.Add(btnSalveaza);
                            }
                            else
                            {
                                MessageBox.Show("Nu s-a găsit programarea pentru acest pacient la data și ora specificate.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Eroare la căutarea programării: " + ex.Message);
                    }
                }
            }

            void SaveAppointmentModification(string originalIdnp, DateTime originalDate, string originalTime)
            {
                var controls = panelDetalii.Controls;
                var cbMediciModif = (ComboBox)controls.Find("cbMediciModif", true).FirstOrDefault();
                var cbFilialaModif = (ComboBox)controls.Find("cbFilialaModif", true).FirstOrDefault();
                var datePickerNou = (DateTimePicker)controls.Find("datePickerNou", true).FirstOrDefault();
                var cbOraNoua = (ComboBox)controls.Find("cbOraNoua", true).FirstOrDefault();
                var cbStatusModif = (ComboBox)controls.Find("cbStatusModif", true).FirstOrDefault();

                if (cbMediciModif == null || cbFilialaModif == null || datePickerNou == null ||
                    cbOraNoua == null || cbStatusModif == null)
                {
                    MessageBox.Show("Nu s-au găsit toate controalele necesare.");
                    return;
                }

                string idMedicNou = cbMediciModif.SelectedValue?.ToString();
                string idFilialaNoua = cbFilialaModif.SelectedValue?.ToString();
                DateTime dataNoua = datePickerNou.Value.Date;
                string oraNoua = cbOraNoua.SelectedItem?.ToString();
                string statusNou = cbStatusModif.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(idMedicNou) || string.IsNullOrEmpty(idFilialaNoua) ||
                    string.IsNullOrEmpty(oraNoua) || string.IsNullOrEmpty(statusNou))
                {
                    MessageBox.Show("Completați toate câmpurile.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();


                        if (dataNoua != originalDate || oraNoua != originalTime)
                        {
                            SqlCommand checkConflict = new SqlCommand(
                                "SELECT COUNT(*) FROM Programari WHERE " +
                                "IdMedic = @idMedic AND DataProgramare = @data AND OraProgramare = @ora " +
                                "AND NOT (DataProgramare = @oldData AND OraProgramare = @oldOra)",
                                conn);
                            checkConflict.Parameters.AddWithValue("@idMedic", idMedicNou);
                            checkConflict.Parameters.AddWithValue("@data", dataNoua);
                            checkConflict.Parameters.AddWithValue("@ora", oraNoua);
                            checkConflict.Parameters.AddWithValue("@oldData", originalDate);
                            checkConflict.Parameters.AddWithValue("@oldOra", originalTime);

                            if ((int)checkConflict.ExecuteScalar() > 0)
                            {
                                MessageBox.Show("Medicul are deja o programare la noua dată și oră selectată.");
                                return;
                            }
                        }

                        // Actualizarea programarii
                        SqlCommand update = new SqlCommand(
                            "UPDATE Programari SET " +
                            "IdMedic = @idMedic, IdFiliala = @idFiliala, " +
                            "DataProgramare = @data, OraProgramare = @ora, " +
                            "StatusProgramare = @status " +
                            "WHERE IdPacient = @idPacient AND DataProgramare = @oldData AND OraProgramare = @oldOra",
                            conn);

                        update.Parameters.AddWithValue("@idMedic", idMedicNou);
                        update.Parameters.AddWithValue("@idFiliala", idFilialaNoua);
                        update.Parameters.AddWithValue("@data", dataNoua);
                        update.Parameters.AddWithValue("@ora", oraNoua);
                        update.Parameters.AddWithValue("@status", statusNou);
                        update.Parameters.AddWithValue("@idPacient", originalIdnp);
                        update.Parameters.AddWithValue("@oldData", originalDate);
                        update.Parameters.AddWithValue("@oldOra", originalTime);

                        int rowsAffected = update.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Programarea a fost actualizată cu succes.");
                            ResetControls();
                        }
                        else
                        {
                            MessageBox.Show("Nu s-a putut actualiza programarea.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Eroare la actualizare: " + ex.Message);
                    }
                }
            }

            void SearchAppointmentForDeletion()
            {
                var panelCautare = panelDetalii.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "panelCautare");
                if (panelCautare == null) return;

                var textIDNP = (TextBox)panelDetalii.Controls.Find("txtIDNP", true).FirstOrDefault();
                var datePicker = (DateTimePicker)panelCautare.Controls.OfType<DateTimePicker>().FirstOrDefault();
                var cbOra = (ComboBox)panelCautare.Controls.OfType<ComboBox>().FirstOrDefault();

                if (txtIDNP == null || datePicker == null || cbOra == null)
                {
                    MessageBox.Show("Nu s-au găsit toate controalele necesare.");
                    return;
                }

                string idnp = txtIDNP.Text.Trim();
                DateTime data = datePicker.Value.Date;
                string ora = cbOra.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(idnp) || idnp.Length != 13)
                {
                    MessageBox.Show("IDNP invalid. Trebuie să conțină exact 13 caractere.");
                    return;
                }

                if (string.IsNullOrEmpty(ora))
                {
                    MessageBox.Show("Selectați ora programării.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();

                        // Se verifica existenta unui pacient
                        SqlCommand checkPacient = new SqlCommand(
                            "SELECT COUNT(*) FROM Pacient WHERE IdPacient = @idnp", conn);
                        checkPacient.Parameters.AddWithValue("@idnp", idnp);

                        if ((int)checkPacient.ExecuteScalar() == 0)
                        {
                            MessageBox.Show("Pacientul nu există în baza de date.");
                            return;
                        }

                        // Returneaza detaliile programarii
                        SqlCommand cmd = new SqlCommand(
                            "SELECT p.IdMedic, p.IdFiliala, p.StatusProgramare, " +
                            "m.Nume + ' ' + m.Prenume AS NumeMedic, f.Adresa AS AdresaFiliala " +
                            "FROM Programari p " +
                            "JOIN Medic m ON p.IdMedic = m.IdMedic " +
                            "JOIN Filiala f ON p.IdFiliala = f.IdFiliala " +
                            "WHERE p.IdPacient = @idnp AND p.DataProgramare = @data AND p.OraProgramare = @ora", conn);

                        cmd.Parameters.AddWithValue("@idnp", idnp);
                        cmd.Parameters.AddWithValue("@data", data);
                        cmd.Parameters.AddWithValue("@ora", ora);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                panelDetalii.Controls.Remove(panelCautare);

                                // Display appointment details
                                int yPos = 70;
                                Label lblDetalii = new Label
                                {
                                    Text = "Detalii programare:",
                                    Location = new Point(10, yPos),
                                    AutoSize = true,
                                    Font = new Font("Arial", 10, FontStyle.Bold)
                                };
                                panelDetalii.Controls.Add(lblDetalii);
                                yPos += 30;

                                // Display IDNP
                                Label lblIDNPInfo = new Label
                                {
                                    Text = $"IDNP: {idnp}",
                                    Location = new Point(10, yPos),
                                    AutoSize = true
                                };
                                panelDetalii.Controls.Add(lblIDNPInfo);
                                yPos += 30;

                                // Display date and time
                                Label lblDataOra = new Label
                                {
                                    Text = $"Data și ora: {data.ToShortDateString()} {ora}",
                                    Location = new Point(10, yPos),
                                    AutoSize = true
                                };
                                panelDetalii.Controls.Add(lblDataOra);
                                yPos += 30;

                                // Display doctor
                                Label lblMedic = new Label
                                {
                                    Text = $"Medic: {reader["NumeMedic"]}",
                                    Location = new Point(10, yPos),
                                    AutoSize = true
                                };
                                panelDetalii.Controls.Add(lblMedic);
                                yPos += 30;

                                // Display branch
                                Label lblFiliala = new Label
                                {
                                    Text = $"Filială: {reader["AdresaFiliala"]}",
                                    Location = new Point(10, yPos),
                                    AutoSize = true
                                };
                                panelDetalii.Controls.Add(lblFiliala);
                                yPos += 30;

                                // Display status
                                Label lblStatus = new Label
                                {
                                    Text = $"Status: {reader["StatusProgramare"]}",
                                    Location = new Point(10, yPos),
                                    AutoSize = true
                                };
                                panelDetalii.Controls.Add(lblStatus);
                                yPos += 40;

                                // Delete button
                                Button btnSterge = new Button
                                {
                                    Text = "Șterge Programarea",
                                    Location = new Point(150, yPos),
                                    Width = 200,
                                    Height = 40,
                                    BackColor = Color.LightCoral,
                                    Font = new Font("Arial", 10, FontStyle.Bold)
                                };
                                btnSterge.Click += (s, e) => DeleteAppointment(idnp, data, ora);
                                panelDetalii.Controls.Add(btnSterge);
                            }
                            else
                            {
                                MessageBox.Show("Nu s-a găsit programarea pentru acest pacient la data și ora specificate.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Eroare la căutarea programării: " + ex.Message);
                    }
                }
            }

            void DeleteAppointment(string idnp, DateTime data, string ora)
            {
                DialogResult result = MessageBox.Show(
                    "Sigur doriți să ștergeți această programare?",
                    "Confirmare ștergere",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand deleteCmd = new SqlCommand(
                                "DELETE FROM Programari " +
                                "WHERE IdPacient = @idnp AND DataProgramare = @data AND OraProgramare = @ora",
                                conn);

                            deleteCmd.Parameters.AddWithValue("@idnp", idnp);
                            deleteCmd.Parameters.AddWithValue("@data", data);
                            deleteCmd.Parameters.AddWithValue("@ora", ora);

                            int rowsAffected = deleteCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Programarea a fost ștearsă cu succes.");
                                ResetControls();
                            }
                            else
                            {
                                MessageBox.Show("Nu s-a găsit programarea pentru ștergere.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Eroare la ștergere: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void ResetControls()
        {
            if (panelDetalii == null) return;

            // Șterge toate controalele existente
            panelDetalii.Controls.Clear();

            // Reinițializează interfața cu acțiunea implicită sau o valoare prestabilită
            AfiseazaCampuri("Adăugare"); // Sau orice altă acțiune implicită dorită
        }
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rb && rb.Checked && panelDetalii != null)
            {
                // Don't call StergePanouri() here - it would remove our container
                panelDetalii.Controls.Clear();

                if (rb.Text == "Adăugare programare")
                    AfiseazaCampuri("Adăugare");
                else if (rb.Text == "Modificare programare")
                    AfiseazaCampuri("Modificare");
                else if (rb.Text == "Ștergere programare")
                    AfiseazaCampuri("Ștergere");

                panelDetalii.Visible = true;
            }
        }


        private void cauta1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            StergePanouri();
            SaveInitialSizes();

            int menuWidth = menuPanel.Width;

            // === Panel principal care va contine tot ===
            Panel pnlContainer = new Panel
            {
                Name = "pnlContainerProgramari",
                Location = new Point(menuPanel.Width, 0),
                Size = new Size(this.ClientSize.Width - menuPanel.Width, this.ClientSize.Height),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(pnlContainer);

            // === Panel pentru bara de căutare ===
            Panel pnlSearch = new Panel
            {
                Name = "pnlSearch",
                Height = 50,
                Width = pnlContainer.Width - 20,
                Location = new Point(10, 20),
                BackColor = Color.Transparent
            };
            pnlContainer.Controls.Add(pnlSearch);

            int offsetX = 80;

            Label lblIDNP = new Label
            {
                Text = "Introduceți IDNP:",
                Location = new Point(offsetX, 15),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            pnlSearch.Controls.Add(lblIDNP);

            TextBox txtIDNP = new TextBox
            {
                Name = "txtIDNP",
                Location = new Point(lblIDNP.Right + 10, 12),
                Width = 200,
                MaxLength = 13,
                Font = new Font("Arial", 10)
            };
            pnlSearch.Controls.Add(txtIDNP);

            Button btnCauta = new Button
            {
                Name = "btnCauta",
                Text = "Caută Programări",
                Location = new Point(txtIDNP.Right + 10, 10),
                Size = new Size(150, 28),
                Font = new Font("Arial", 10),
                BackColor = Color.LightSteelBlue
            };
            pnlSearch.Controls.Add(btnCauta);

            // === Panel pentru DataGridView ===
            Panel pnlGrid = new Panel
            {
                Name = "pnlGrid",
                Location = new Point(10, pnlSearch.Bottom + 10),
                Size = new Size(pnlContainer.Width - 20, pnlContainer.Height - pnlSearch.Height - 40),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlContainer.Controls.Add(pnlGrid);

            // === DataGridView în panelul dedicat ===
            DataGridView dgv = new DataGridView
            {
                Name = "dgvProgramari",
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White
            };
            pnlGrid.Controls.Add(dgv);

            // === Handler pt căutare ===
            btnCauta.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtIDNP.Text) || txtIDNP.Text.Length != 13)
                {
                    MessageBox.Show("Introduceți un IDNP valid (13 caractere)!");
                    dgv.DataSource = null;
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        string query = @"
                    SELECT 
                        M.Nume + ' ' + M.Prenume AS Medic,
                        F.Adresa AS Filiala,
                        CONVERT(varchar, P.DataProgramare, 104) AS [Data],
                        P.OraProgramare AS [Ora],
                        P.StatusProgramare AS [Status]
                    FROM Programari P
                    JOIN Medic M ON P.IdMedic = M.IdMedic
                    JOIN Filiala F ON P.IdFiliala = F.IdFiliala
                    WHERE P.IdPacient = @idnp
                    ORDER BY P.DataProgramare, P.OraProgramare";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@idnp", txtIDNP.Text.Trim());

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show("Nu există programări pentru acest IDNP.");
                                dgv.DataSource = null;
                                return;
                            }

                            dgv.DataSource = dt;
                            dgv.Columns["Ora"].DefaultCellStyle.Format = "hh\\:mm";
                            dgv.Columns["Data"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dgv.Columns["Ora"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                            // Autoajustare înălțime rânduri
                            dgv.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare: " + ex.Message);
                    dgv.DataSource = null;
                }
            };
        }
        private void back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuPanel_Paint(object sender, PaintEventArgs e)
        {
            SaveInitialSizes();
        }

        private void cauta2_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            StergePanouri();
            SaveInitialSizes();

            int menuWidth = menuPanel.Width;

            Panel pnlContainer = new Panel
            {
                Name = "pnlContainerProgramariMedic",
                Location = new Point(menuWidth, 0),
                Size = new Size(this.ClientSize.Width - menuWidth, this.ClientSize.Height),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(pnlContainer);

            Panel pnlSearch = new Panel
            {
                Name = "pnlSearch",
                Height = 100,
                Width = pnlContainer.Width - 20,
                Location = new Point(10, 20),
                BackColor = Color.Transparent
            };
            pnlContainer.Controls.Add(pnlSearch);

            int offsetX = 50;

            Label lblMedic = new Label
            {
                Text = "Selectați medicul:",
                Location = new Point(offsetX, 15),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            pnlSearch.Controls.Add(lblMedic);

            ComboBox cmbMedici = new ComboBox
            {
                Name = "cmbMedici",
                Location = new Point(lblMedic.Right + 10, 12),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 10)
            };
            pnlSearch.Controls.Add(cmbMedici);

            CheckBox chkData = new CheckBox
            {
                Text = "Filtrare după dată",
                Location = new Point(offsetX, 50),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            pnlSearch.Controls.Add(chkData);

            DateTimePicker dtpData = new DateTimePicker
            {
                Name = "dtpData",
                Location = new Point(chkData.Right + 10, 47),
                Width = 150,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Arial", 10),
                Visible = false // inițial ascuns
            };
            pnlSearch.Controls.Add(dtpData);

            chkData.CheckedChanged += (s, ev) =>
            {
                dtpData.Visible = chkData.Checked;
            };

            Button btnCauta = new Button
            {
                Name = "btnCauta",
                Text = "Caută Programări",
                Location = new Point(cmbMedici.Right + 20, 25),
                Size = new Size(150, 28),
                Font = new Font("Arial", 10),
                BackColor = Color.LightSteelBlue
            };
            pnlSearch.Controls.Add(btnCauta);

            Panel pnlGrid = new Panel
            {
                Name = "pnlGrid",
                Location = new Point(10, pnlSearch.Bottom + 10),
                Size = new Size(pnlContainer.Width - 20, pnlContainer.Height - pnlSearch.Height - 40),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlContainer.Controls.Add(pnlGrid);

            DataGridView dgv = new DataGridView
            {
                Name = "dgvProgramariMedic",
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White
            };
            pnlGrid.Controls.Add(dgv);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT IdMedic, Nume + ' ' + Prenume AS NumeComplet FROM Medic", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbMedici.DataSource = dt;
                    cmbMedici.DisplayMember = "NumeComplet";
                    cmbMedici.ValueMember = "IdMedic";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la încărcarea medicilor: " + ex.Message);
                }
            }

            btnCauta.Click += (s, ev) =>
            {
                if (cmbMedici.SelectedIndex < 0)
                {
                    MessageBox.Show("Selectați un medic valid!");
                    dgv.DataSource = null;
                    return;
                }

                try
                {
                    string idMedic = cmbMedici.SelectedValue.ToString();
                    bool filtrareData = chkData.Checked;
                    DateTime selectedDate = dtpData.Value.Date;

                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        string query = @"
                SELECT 
                    Pa.Nume + ' ' + Pa.Prenume AS Pacient,
                    Pa.IdPacient,
                    F.Adresa AS Filiala,
                    CONVERT(varchar, P.DataProgramare, 104) AS [Data],
                    P.OraProgramare AS [Ora],
                    P.StatusProgramare AS [Status]
                FROM Programari P
                JOIN Pacient Pa ON P.IdPacient = Pa.IdPacient
                JOIN Filiala F ON P.IdFiliala = F.IdFiliala
                WHERE P.IdMedic = @idMedic";

                        if (filtrareData)
                            query += " AND CONVERT(date, P.DataProgramare) = @dataProgramare";

                        query += " ORDER BY P.DataProgramare, P.OraProgramare";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@idMedic", idMedic);
                            if (filtrareData)
                                cmd.Parameters.AddWithValue("@dataProgramare", selectedDate);

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count == 0)
                            {
                                string mesaj = filtrareData
                                    ? $"Nu există programări pentru medicul selectat în data de {selectedDate:dd.MM.yyyy}."
                                    : "Nu există programări pentru medicul selectat.";
                                MessageBox.Show(mesaj);
                                dgv.DataSource = null;
                                return;
                            }

                            dgv.DataSource = dt;
                            dgv.Columns["Ora"].DefaultCellStyle.Format = "hh\\:mm";
                            dgv.Columns["Data"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dgv.Columns["Ora"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dgv.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare: " + ex.Message);
                    dgv.DataSource = null;
                }
            };
        }


    }
}