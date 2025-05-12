using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data;
using System.IO;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Reporting.WinForms;



namespace Studiu_Individual_1
{
    public partial class AdminMenu : Form
    {
        private Dictionary<Control, Rectangle> initialSizes = new Dictionary<Control, Rectangle>();
        private Dictionary<Button, EventHandler> buttonHandlers = new Dictionary<Button, EventHandler>();
        private float initialFormWidth, initialFormHeight;
        private Panel sideMenuPanel;
        private Panel mainMenuPanel;
        private Panel currentActionPanel;
        string connectionString = "Data Source=DESKTOP-MFVSBQK\\SQLEXPRESS;Initial Catalog=SpitalRepromed;Integrated Security=True;";

        public AdminMenu()
        {
            InitializeComponent();
            InitializeComponentCustom();
        }

        private void InitializeComponentCustom()
        {
            InitializeMainMenu();
            SaveInitialSizes();
            this.Resize += (s, e) => ResizeControls();
        }

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

        private void InitializeMainMenu()
        {
            mainMenuPanel = new Panel
            {
                Name = "pnlMainMenu",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 240, 245),
                Padding = new Padding(20)
            };
            this.Controls.Add(mainMenuPanel);

            Label lblTitle = new Label
            {
                Text = "ADMINISTRARE CLINICĂ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 80),
                AutoSize = true,
                Dock = DockStyle.Top,
                Height = 80,
                TextAlign = ContentAlignment.MiddleCenter
            };
            mainMenuPanel.Controls.Add(lblTitle);

            FlowLayoutPanel flowPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0, 20, 0, 0),
                Anchor = AnchorStyles.None
            };

            List<Button> mainButtons = new List<Button>
    {
        CreateMenuButton("👥 UTILIZATORI", Color.FromArgb(70, 130, 180)),
        CreateMenuButton("🏥 FILIALE", Color.FromArgb(100, 149, 237)),
        CreateMenuButton("🔐 DREPTURI", Color.FromArgb(30, 144, 255)),
        CreateMenuButton("💾 BACKUP", Color.FromArgb(0, 191, 255)),
        CreateMenuButton("📊 RAPOARTE", Color.FromArgb(75, 0, 130))
    };

            foreach (var btn in mainButtons)
            {
                flowPanel.Controls.Add(btn);
            }

            mainMenuPanel.Controls.Add(flowPanel);
            CenterPanel(flowPanel, mainMenuPanel);

            // Adaugă handler-ele pentru butoane
            mainButtons[0].Click += (s, e) => ShowUserManagement();
            mainButtons[1].Click += (s, e) => ShowBranchManagement();
            mainButtons[2].Click += (s, e) => ShowRightsManagement();
            mainButtons[3].Click += (s, e) => ShowBackupRestore();
            mainButtons[4].Click += (s, e) => ShowReportsPanel();
        }

        private Panel reportsPanel;

        private void ShowReportsPanel()
        {
            mainMenuPanel.Visible = false;

            reportsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            this.Controls.Add(reportsPanel);
            reportsPanel.BringToFront();

            Label lblReportsTitle = new Label
            {
                Text = "RAPOARTE PROGRAMĂRI",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };
            reportsPanel.Controls.Add(lblReportsTitle);

            FlowLayoutPanel btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10),
                AutoSize = true
            };

            Button btnRapDepartament = new Button
            {
                Text = "Programări per departament",
                Width = 220,
                Height = 40,
                BackColor = Color.LightSteelBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnRapDepartament.Click += (s, e) => ShowReport("departament");

            Button btnRapStatus = new Button
            {
                Text = "Programări anulate/confirmate",
                Width = 240,
                Height = 40,
                BackColor = Color.LightSteelBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnRapStatus.Click += (s, e) => ShowReport("status");

            btnPanel.Controls.Add(btnRapDepartament);
            btnPanel.Controls.Add(btnRapStatus);
            reportsPanel.Controls.Add(btnPanel);
        }

        private void ShowReport(string reportType)
        {
            try
            {
                // Clear previous reports
                var oldViewers = reportsPanel.Controls.OfType<ReportViewer>().ToList();
                foreach (var viewer in oldViewers)
                {
                    reportsPanel.Controls.Remove(viewer);
                    viewer.Dispose();
                }

                // Create new viewer with proper settings
                ReportViewer reportViewer = new ReportViewer
                {
                    Dock = DockStyle.Fill,
                    ProcessingMode = ProcessingMode.Local,
                    Visible = true,
                    ZoomMode = ZoomMode.Percent,
                    ZoomPercent = 100
                };

                reportsPanel.Controls.Add(reportViewer);

                switch (reportType.ToLower())
                {
                    case "departament":
                        LoadBranchReport(reportViewer);
                        break;

                    case "status":
                        LoadStatusReport(reportViewer);
                        break;
                }

                reportViewer.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading report: {ex.Message}\n\n{ex.StackTrace}",
                              "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBranchReport(ReportViewer viewer)
        {
            try
            {
                // 1. Reset and prepare viewer
                viewer.Reset();
                viewer.LocalReport.ReportEmbeddedResource = "Studiu_Individual_1.Reports.AppointmentsByBranch.rdlc";

                // 2. Get data with error handling
                DataTable data = GetAppointmentsByBranch();

                // 3. Verify data
                if (data == null || data.Rows.Count == 0)
                {
                    data = CreateEmptyBranchDataTable();
                    data.Rows.Add("No data available", "", 0, DateTime.Now.ToString("dd.MM.yyyy"), "");
                }

                // 4. Bind data
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(new ReportDataSource("App2", data));

                // 5. Refresh with proper settings
                viewer.SetDisplayMode(DisplayMode.PrintLayout);
                viewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Branch Report Error: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStatusReport(ReportViewer viewer)
        {
            try
            {
                // 1. Reset and prepare viewer
                viewer.Reset();
                viewer.LocalReport.ReportEmbeddedResource = "Studiu_Individual_1.Reports.AppointmentStatus.rdlc";

                // 2. Get data - first try with test data to verify
                DataTable data = GetAppointmentsByStatus();

                // 4. Bind data
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));

                // 5. Force proper refresh
                viewer.SetDisplayMode(DisplayMode.PrintLayout);
                viewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Pie Chart Error: {ex.Message}\n\n{ex.StackTrace}",
                              "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetAppointmentsByBranch()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    f.Adresa AS BranchAddress,
                    m.Specializare AS Specialization,
                    COUNT(p.IdProgramare) AS AppointmentCount,
                    CONVERT(VARCHAR, p.DataProgramare, 104) AS AppointmentDate,
                    p.StatusProgramare AS Status
                FROM Programari p
                INNER JOIN Filiala f ON p.IdFiliala = f.IdFiliala
                INNER JOIN Medic m ON p.IdMedic = m.IdMedic
                GROUP BY f.Adresa, m.Specializare, p.DataProgramare, p.StatusProgramare
                ORDER BY p.DataProgramare, f.Adresa";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        dt.Load(cmd.ExecuteReader());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading branch data: {ex.Message}",
                              "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

        private DataTable GetAppointmentsByStatus()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
SELECT
                    COUNT(*) IdProgramare 
                FROM Programari
                GROUP BY StatusProgramare";


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        dt.Load(cmd.ExecuteReader());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading status data: {ex.Message}",
                              "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

        private DataTable CreateEmptyBranchDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BranchAddress", typeof(string));
            dt.Columns.Add("Specialization", typeof(string));
            dt.Columns.Add("AppointmentCount", typeof(int));
            dt.Columns.Add("AppointmentDate", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            return dt;
        }

        private Button CreateMenuButton(string text, Color baseColor)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = baseColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(250, 60),
                Margin = new Padding(0, 0, 0, 20),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) =>
            {
                btn.Font = new Font(btn.Font, FontStyle.Bold | FontStyle.Underline);
                btn.Size = new Size(260, 65);
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.Font = new Font(btn.Font, FontStyle.Bold);
                btn.Size = new Size(250, 60);
            };

            return btn;
        }

        private void CenterPanel(Control panelToCenter, Control parentPanel)
        {
            panelToCenter.Location = new Point(
                (parentPanel.Width - panelToCenter.Width) / 2,
                (parentPanel.Height - panelToCenter.Height) / 2);
        }

        private void ShowUserManagement()
        {
            mainMenuPanel.Visible = false;
            CreateSideMenu("UTILIZATORI", new List<string> { "➕ Adăugare Utilizator", "✏️ Modificare Utilizator", "❌ Ștergere Utilizator" },
                new List<Action> { ShowAddUserPanel, ShowEditUserPanel, ShowDeleteUserPanel });
        }

        private void ShowBranchManagement()
        {
            mainMenuPanel.Visible = false;
            CreateSideMenu("FILIALE", new List<string> { "➕ Adăugare Filială", "✏️ Modificare Filială", "❌ Ștergere Filială" },
                new List<Action> { ShowAddBranchPanel, ShowEditBranchPanel, ShowDeleteBranchPanel });
        }

        private void ShowRightsManagement()
        {
            mainMenuPanel.Visible = false;
            CreateSideMenu("DREPTURI", new List<string> { "✏️ Acordare/Revocare Drepturi" },
                new List<Action> { ShowManageRightsPanel });
        }

        private void CreateSideMenu(string title, List<string> buttons, List<Action> actions)
        {
            if (sideMenuPanel != null)
            {
                this.Controls.Remove(sideMenuPanel);
                sideMenuPanel.Dispose();
            }

            sideMenuPanel = new Panel
            {
                Width = 280,
                Height = this.ClientSize.Height,
                BackColor = Color.FromArgb(30, 30, 45),
                Dock = DockStyle.Left,
                Padding = new Padding(10)
            };
            this.Controls.Add(sideMenuPanel);

            AddMenuButton("⬅ Înapoi", 330, ReturnToMainMenu);
            AddMenuLabel(title, 80);

            int yPos = 130;
            for (int i = 0; i < buttons.Count; i++)
            {
                int index = i;
                AddMenuButton(buttons[i], yPos, () =>
                {
                    RemoveActionPanel();
                    actions[index]();
                });
                yPos += 65;
            }

            sideMenuPanel.Show();
        }

        private void AddMenuButton(string text, int y, Action onClick)
        {
            Button button = new Button
            {
                Text = text,
                Height = 55,
                Width = 240,
                Location = new Point(15, y),
                BackColor = Color.FromArgb(50, 50, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };

            button.FlatAppearance.BorderSize = 0;
            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(70, 70, 100);
            button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(50, 50, 70);
            button.Click += (s, e) => onClick();

            sideMenuPanel.Controls.Add(button);
        }

        private void AddMenuLabel(string text, int y)
        {
            Label label = new Label
            {
                Text = text,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.WhiteSmoke,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = sideMenuPanel.Width - 20,
                Location = new Point(10, y)
            };
            sideMenuPanel.Controls.Add(label);
        }

        private void ReturnToMainMenu()
        {
            if (sideMenuPanel != null)
            {
                this.Controls.Remove(sideMenuPanel);
                sideMenuPanel.Dispose();
                sideMenuPanel = null;
            }

            if (currentActionPanel != null)
            {
                this.Controls.Remove(currentActionPanel);
                currentActionPanel.Dispose();
                currentActionPanel = null;
            }

            mainMenuPanel.Visible = true;
        }

        private void RemoveActionPanel()
        {
            if (currentActionPanel != null)
            {
                foreach (Control ctrl in currentActionPanel.Controls)
                {
                    if (ctrl is Button btn && buttonHandlers.ContainsKey(btn))
                    {
                        btn.Click -= buttonHandlers[btn];
                        buttonHandlers.Remove(btn);
                    }
                }

                this.Controls.Remove(currentActionPanel);
                currentActionPanel.Dispose();
                currentActionPanel = null;
            }
        }
        private Button CreateStyledButton(string text, Point location, EventHandler clickHandler = null)
        {
            Button button = new Button
            {
                Text = text,
                Height = 40,
                Width = 180,
                Location = location,
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                Font = new Font("Arial", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };

            if (clickHandler != null)
            {
                button.Click += clickHandler;
                buttonHandlers[button] = clickHandler;
            }

            return button;
        }

        private Button CreateBackButton()
        {
            Button btn = new Button
            {
                Text = "Înapoi",
                Location = new Point(20, 20),
                Size = new Size(80, 30),
                BackColor = Color.LightGray,
                ForeColor = Color.Black
            };
            return btn;
        }


        private void ShowAddUserPanel()
        {

            currentActionPanel = CreateActionPanel("Adăugare Utilizator", 500, 650);

            int yPos = 70;
            Label lblTip = new Label { Text = "Tip Utilizator:", Location = new Point(20, yPos), Font = new Font("Arial", 10, FontStyle.Bold) };
            currentActionPanel.Controls.Add(lblTip);

            RadioButton rbMedic = new RadioButton { Text = "Medic", Location = new Point(150, yPos), Checked = true };
            RadioButton rbReceptionist = new RadioButton { Text = "Receptionist", Location = new Point(250, yPos) };
            currentActionPanel.Controls.Add(rbMedic);
            currentActionPanel.Controls.Add(rbReceptionist);

            yPos += 40;
            TextBox txtId = AddLabeledTextBox(currentActionPanel, "ID (CNP):", "txtId", yPos);
            txtId.MaxLength = 13;

            yPos += 40;
            TextBox txtNume = AddLabeledTextBox(currentActionPanel, "Nume:", "txtNume", yPos);

            yPos += 40;
            TextBox txtPrenume = AddLabeledTextBox(currentActionPanel, "Prenume:", "txtPrenume", yPos);

            yPos += 40;
            TextBox txtParola = AddLabeledTextBox(currentActionPanel, "Parolă:", "txtParola", yPos, true);

            yPos += 40;
            TextBox txtEmail = AddLabeledTextBox(currentActionPanel, "Email:", "txtEmail", yPos);

            yPos += 40;
            TextBox txtTelefon = AddLabeledTextBox(currentActionPanel, "Telefon:", "txtTelefon", yPos);
            txtTelefon.MaxLength = 9;

            yPos += 40;
            Label lblSpecializare = new Label
            {
                Text = "Specializare:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Name = "lblSpecializare"
            };
            currentActionPanel.Controls.Add(lblSpecializare);

            ComboBox cmbSpecializare = new ComboBox
            {
                Location = new Point(150, yPos - 2),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cmbSpecializare"
            };
            cmbSpecializare.Items.AddRange(new[] { "Cardiologie", "Pediatrie", "Chirurgie", "Neurologie" });
            currentActionPanel.Controls.Add(cmbSpecializare);

            yPos += 40;
            Label lblDepartament = new Label
            {
                Text = "Departament:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Name = "lblDepartament"
            };
            currentActionPanel.Controls.Add(lblDepartament);

            ComboBox cmbDepartament = new ComboBox
            {
                Location = new Point(150, yPos - 2),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cmbDepartament"
            };
            currentActionPanel.Controls.Add(cmbDepartament);

            yPos += 40;
            Label lblFiliala = new Label
            {
                Text = "Filială:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Name = "lblFiliala"
            };
            currentActionPanel.Controls.Add(lblFiliala);

            ComboBox cmbFiliala = new ComboBox
            {
                Location = new Point(150, yPos - 2),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cmbFiliala"
            };
            currentActionPanel.Controls.Add(cmbFiliala);

            yPos += 40;
            Label lblDrepturi = new Label
            {
                Text = "Drepturi:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Name = "lblDrepturi"
            };
            currentActionPanel.Controls.Add(lblDrepturi);

            ComboBox cmbDrepturi = new ComboBox
            {
                Location = new Point(150, yPos - 2),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cmbDrepturi"
            };
            cmbDrepturi.Items.AddRange(new[] { "read_write", "read_only", "admin" });
            currentActionPanel.Controls.Add(cmbDrepturi);

            
            LoadComboBoxData(cmbDepartament, "SELECT IdDepartament, Nume FROM Departament", "Nume", "IdDepartament");
            LoadComboBoxData(cmbFiliala, "SELECT IdFiliala, Adresa FROM Filiala", "Adresa", "IdFiliala");

            rbReceptionist.CheckedChanged += (s, e) =>
            {
                bool isMedic = rbMedic.Checked;
                UpdateUserControlsVisibility(isMedic);

                if (!isMedic)
                {
                    // Resetează câmpurile pentru Receptionist
                    if (cmbSpecializare != null) cmbSpecializare.SelectedIndex = -1;
                    if (cmbDepartament != null) cmbDepartament.SelectedIndex = -1;
                    if (cmbFiliala != null) cmbFiliala.SelectedIndex = -1;
                    if (cmbDrepturi != null) cmbDrepturi.SelectedIndex = -1;
                    if (txtEmail != null) txtEmail.Text = "";
                    if (txtTelefon != null) txtTelefon.Text = "";
                }
            };
            UpdateUserControlsVisibility(true);

            Button btnSalveaza = new Button
            {
                Text = "Salvează",
                Location = new Point(150, yPos + 50),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(30, 144, 255),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnSalveaza.Click += (s, e) => SaveUser(
                rbMedic.Checked ? "Medic" : "Receptionist",
                txtId.Text,
                txtNume.Text,
                txtPrenume.Text,
                txtEmail.Text,
                txtTelefon.Text,
                txtParola.Text,
                cmbSpecializare.SelectedItem?.ToString(),
                cmbDepartament.SelectedValue?.ToString(),
                cmbFiliala.SelectedValue?.ToString(),
                cmbDrepturi.SelectedItem?.ToString());

            currentActionPanel.Controls.Add(btnSalveaza);
        }

        private void LoadComboBoxData(ComboBox comboBox, string query, string displayMember, string valueMember)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // First verify the query works
                    using (SqlCommand testCmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = testCmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                MessageBox.Show($"Query returned no data:\n{query}", "Warning",
                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        DataTable dt = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            comboBox.BeginUpdate();
                            comboBox.DataSource = null;
                            comboBox.Items.Clear();

                            
                            comboBox.DataSource = dt;
                            if (dt.Rows.Count > 0)
                            {
                                comboBox.DisplayMember = displayMember;
                                comboBox.ValueMember = valueMember;
                                comboBox.SelectedIndex = 0; 
                            }
                            else
                            {
                                comboBox.Text = string.Empty;
                            }
                            comboBox.EndUpdate();
                            da.Fill(dt);

                            
                            if (!dt.Columns.Contains(displayMember))
                                throw new Exception($"Display column '{displayMember}' not found");
                            if (!dt.Columns.Contains(valueMember))
                                throw new Exception($"Value column '{valueMember}' not found");

                            comboBox.BeginUpdate();
                            comboBox.DataSource = dt;
                            comboBox.DisplayMember = displayMember;
                            comboBox.ValueMember = valueMember;
                            comboBox.EndUpdate();
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                string errorDetails = $"SQL Error loading {displayMember}:\n" +
                                     $"Message: {sqlEx.Message}\n" +
                                     $"Query: {query}\n" +
                                     $"Server: {sqlEx.Server}\n" +
                                     $"Error Code: {sqlEx.Number}";

                MessageBox.Show(errorDetails, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading {displayMember}:\n{ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveUser(string tip, string id, string nume, string prenume, string email, string telefon, string parola,
    string specializare, string idDepartament, string idFiliala, string drepturi)
        {
            // Input validation remains the same
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(nume) ||
    string.IsNullOrWhiteSpace(prenume) || string.IsNullOrWhiteSpace(parola))
            {
                MessageBox.Show("Completați toate câmpurile obligatorii!", "Eroare",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tip == "Medic")
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(telefon) ||
                    string.IsNullOrWhiteSpace(specializare) || string.IsNullOrWhiteSpace(idDepartament) ||
                    string.IsNullOrWhiteSpace(idFiliala) || string.IsNullOrWhiteSpace(drepturi))
                {
                    MessageBox.Show("Completați toate câmpurile pentru medic!", "Eroare",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[a-zA-Z]+$"))
                {
                    MessageBox.Show("Introduceți un email valid!", "Eroare",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // Format login name based on user type
                        string loginName = tip == "Medic"
                            ? $"medic_{nume.ToLower()}_{prenume.ToLower()}"
                            : $"receptie_{nume.ToLower()}_{prenume.ToLower()}";

                        loginName = loginName
                            .Replace(" ", "")
                            .Replace("ă", "a").Replace("â", "a")
                            .Replace("î", "i").Replace("ș", "s").Replace("ț", "t");

                        string userName = loginName;
                        // Create login
                        string createLoginSql = $"CREATE LOGIN {loginName} WITH PASSWORD = '{parola}';";
                        using (SqlCommand cmd = new SqlCommand(createLoginSql, conn, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        // Create user
                        string createUserSql = $"CREATE USER {userName} FOR LOGIN {loginName};";
                        using (SqlCommand cmd = new SqlCommand(createUserSql, conn, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        // Grant permissions
                        string grantSql = "";
                        switch (drepturi)
                        {
                            case "read_write":
                                grantSql = $"GRANT SELECT, INSERT, UPDATE ON SCHEMA::dbo TO {userName};";
                                break;
                            case "read_only":
                                grantSql = $"GRANT SELECT ON SCHEMA::dbo TO {userName};";
                                break;
                            case "admin":
                                grantSql = $"GRANT SELECT, INSERT, UPDATE, DELETE, EXECUTE ON SCHEMA::dbo TO {userName};";
                                break;
                        }

                        using (SqlCommand cmd = new SqlCommand(grantSql, conn, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        if (tip == "Medic")
                        {
                            // Insert into Medic
                            string insertMedicSql = @"INSERT INTO Medic (IdMedic, Nume, Prenume, Specializare, NrTelefon, Email, IdDepartament, IdFiliala) 
                                   VALUES (@IdMedic, @Nume, @Prenume, @Specializare, @NrTelefon, @Email, @IdDepartament, @IdFiliala)";

                            using (SqlCommand cmd = new SqlCommand(insertMedicSql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@IdMedic", id);
                                cmd.Parameters.AddWithValue("@Nume", nume);
                                cmd.Parameters.AddWithValue("@Prenume", prenume);
                                cmd.Parameters.AddWithValue("@Specializare", specializare);
                                cmd.Parameters.AddWithValue("@NrTelefon", telefon);
                                cmd.Parameters.AddWithValue("@Email", email);
                                cmd.Parameters.AddWithValue("@IdDepartament", idDepartament);
                                cmd.Parameters.AddWithValue("@IdFiliala", idFiliala);

                                cmd.ExecuteNonQuery();
                            }
                        }
                        

                        transaction.Commit();

                        MessageBox.Show($"Utilizator {tip} adăugat cu succes!\nLogin creat: {loginName}",
                                      "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Only clear controls, don't remove panel
                        ClearInputControls(currentActionPanel);

                        // Reset to receptionist view if needed
                        if (tip == "Receptionist")
                        {
                            var rbReceptionist = currentActionPanel.Controls.OfType<RadioButton>()
                                .FirstOrDefault(r => r.Text == "Receptionist");
                            if (rbReceptionist != null) rbReceptionist.Checked = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Eroare la salvarea utilizatorului: {ex.Message}",
                                          "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception rollbackEx)
                        {
                            MessageBox.Show($"Eroare la rollback: {rollbackEx.Message}",
                                          "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la conectarea la baza de date: {ex.Message}",
                              "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputControls(Control container)
        {
            foreach (Control ctrl in container.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                else if (ctrl is ComboBox)
                    ((ComboBox)ctrl).SelectedIndex = -1;
            }

            // Resetează RadioButton la Medic și actualizează vizibilitatea
            var rbMedic = container.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Text == "Medic");
            if (rbMedic != null)
            {
                rbMedic.Checked = true;
                UpdateUserControlsVisibility(true); // Forțează vizibilitatea pentru Medic
            }
        }

        private TextBox AddLabeledTextBox(Panel panel, string labelText, string name, int y, bool isPassword = false)
        {
            // Adaugă prefix "lbl_" la numele Label-ului pentru a-l face unic
            Label lbl = new Label
            {
                Text = labelText,
                Location = new Point(20, y),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true,
                Name = "lbl_" + name  // Numele Label-ului începe cu "lbl_"
            };
            panel.Controls.Add(lbl);

            TextBox txt = new TextBox
            {
                Name = name,
                Location = new Point(150, y - 2),
                Width = 200,
                PasswordChar = isPassword ? '*' : '\0'
            };
            panel.Controls.Add(txt);

            return txt;
        }


        private Panel CreateActionPanel(string title, int width, int height)
        {
            RemoveActionPanel();

            Panel panel = new Panel
            {
                Name = "panelActiune",
                Width = width,
                Height = height,
                BackColor = Color.LightGray,
                Location = new Point((this.ClientSize.Width - width) / 2, (this.ClientSize.Height - height) / 2),
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(new Label
            {
                Text = title,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            });

            this.Controls.Add(panel);
            panel.BringToFront();

            return panel;
        }

        private void UpdateUserControlsVisibility(bool isMedic)
        {
            // Găsește toate controalele o singură dată
            var lblSpecializare = currentActionPanel.Controls.Find("lblSpecializare", true).FirstOrDefault() as Label;
            var cmbSpecializare = currentActionPanel.Controls.Find("cmbSpecializare", true).FirstOrDefault() as ComboBox;
            var lblDepartament = currentActionPanel.Controls.Find("lblDepartament", true).FirstOrDefault() as Label;
            var cmbDepartament = currentActionPanel.Controls.Find("cmbDepartament", true).FirstOrDefault() as ComboBox;
            var lblFiliala = currentActionPanel.Controls.Find("lblFiliala", true).FirstOrDefault() as Label;
            var cmbFiliala = currentActionPanel.Controls.Find("cmbFiliala", true).FirstOrDefault() as ComboBox;
            var lblDrepturi = currentActionPanel.Controls.Find("lblDrepturi", true).FirstOrDefault() as Label;
            var cmbDrepturi = currentActionPanel.Controls.Find("cmbDrepturi", true).FirstOrDefault() as ComboBox;
            var lblEmail = currentActionPanel.Controls.Find("lbl_txtEmail", true).FirstOrDefault() as Label;
            var txtEmail = currentActionPanel.Controls.Find("txtEmail", true).FirstOrDefault() as TextBox;
            var lblTelefon = currentActionPanel.Controls.Find("lbl_txtTelefon", true).FirstOrDefault() as Label;
            var txtTelefon = currentActionPanel.Controls.Find("txtTelefon", true).FirstOrDefault() as TextBox;

            // Actualizează vizibilitatea
            if (lblEmail != null) lblEmail.Visible = isMedic;
            if (txtEmail != null) txtEmail.Visible = isMedic;
            if (lblTelefon != null) lblTelefon.Visible = isMedic;
            if (txtTelefon != null) txtTelefon.Visible = isMedic;
            if (lblSpecializare != null) lblSpecializare.Visible = isMedic;
            if (cmbSpecializare != null) cmbSpecializare.Visible = isMedic;
            if (lblDepartament != null) lblDepartament.Visible = isMedic;
            if (cmbDepartament != null) cmbDepartament.Visible = isMedic;
            if (lblFiliala != null) lblFiliala.Visible = isMedic;
            if (cmbFiliala != null) cmbFiliala.Visible = isMedic;
            if (lblDrepturi != null) lblDrepturi.Visible = isMedic;
            if (cmbDrepturi != null) cmbDrepturi.Visible = isMedic;
        }
        private void ShowEditUserPanel()
        {
            currentActionPanel = CreateActionPanel("Editare Login și Parolă", 400, 350); 

            // 1. Combobox pentru selectarea login-urilor existente
            int yPos = 50;
            Label lblSelectLogin = new Label
            {
                Text = "Selectează login:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            currentActionPanel.Controls.Add(lblSelectLogin);

            ComboBox cmbLogins = new ComboBox
            {
                Location = new Point(150, yPos),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cmbLogins"
            };
            currentActionPanel.Controls.Add(cmbLogins);

            // 2. Încarcă login-urile din SQL Server
            LoadSqlLoginsIntoComboBox(cmbLogins);

            // 3. Câmp pentru noua parolă cu buton de show/hide
            yPos += 50;
            Label lblPassword = new Label
            {
                Text = "Parolă nouă:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            currentActionPanel.Controls.Add(lblPassword);

            TextBox txtNewPassword = new TextBox
            {
                Location = new Point(150, yPos),
                Width = 170,
                PasswordChar = '*',
                Name = "txtNewPassword"
            };
            currentActionPanel.Controls.Add(txtNewPassword);

            // Buton show/hide password
            Button btnShowHide = new Button
            {
                Text = "👁️",
                Location = new Point(325, yPos),
                Size = new Size(25, 23),
                Tag = false // false = password hidden
            };
            btnShowHide.Click += (s, e) =>
            {
                bool isVisible = (bool)btnShowHide.Tag;
                txtNewPassword.PasswordChar = isVisible ? '*' : '\0';
                btnShowHide.Tag = !isVisible;
            };
            currentActionPanel.Controls.Add(btnShowHide);

            // 4. Buton Salvează modificări
            Button btnSave = new Button
            {
                Text = "Salvează parola",
                Location = new Point(150, yPos + 50),
                Size = new Size(150, 40),
                BackColor = Color.LightGreen
            };
            btnSave.Click += (s, e) =>
            {
                SavePasswordChanges(cmbLogins, txtNewPassword);
            };
            currentActionPanel.Controls.Add(btnSave);
        }

        private void LoadSqlLoginsIntoComboBox(ComboBox cmbLogins)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query pentru a obține toate login-urile SQL
                    string query = @"SELECT name FROM sys.sql_logins 
                           WHERE name NOT IN ('sa', 'public', 'guest') 
                           AND is_disabled = 0";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbLogins.Items.Add(reader["name"].ToString());
                        }
                    }

                    if (cmbLogins.Items.Count > 0)
                        cmbLogins.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea login-urilor: {ex.Message}");
            }
        }

        private void SavePasswordChanges(ComboBox cmbLogins, TextBox txtNewPassword)
        {
            string loginName = cmbLogins.SelectedItem?.ToString();
            string newPassword = txtNewPassword.Text;

            if (string.IsNullOrWhiteSpace(loginName))
            {
                MessageBox.Show("Selectați un login!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8)
            {
                MessageBox.Show("Parola trebuie să aibă minim 8 caractere!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string masterConnectionString = connectionString.Replace("Initial Catalog=SpitalRepromed", "Initial Catalog=master");

                using (SqlConnection conn = new SqlConnection(masterConnectionString))
                {
                    conn.Open();

                    // 1. Verify login exists
                    string checkLoginQuery = "SELECT COUNT(*) FROM sys.sql_logins WHERE name = @LoginName";
                    SqlCommand checkCmd = new SqlCommand(checkLoginQuery, conn);
                    checkCmd.Parameters.AddWithValue("@LoginName", loginName);

                    if ((int)checkCmd.ExecuteScalar() == 0)
                    {
                        MessageBox.Show("Login-ul selectat nu există!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 2. Check if new password matches current one
                    string checkPasswordQuery = "SELECT PWDCOMPARE(@NewPassword, password_hash) FROM sys.sql_logins WHERE name = @LoginName";
                    SqlCommand checkPwdCmd = new SqlCommand(checkPasswordQuery, conn);
                    checkPwdCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                    checkPwdCmd.Parameters.AddWithValue("@LoginName", loginName);

                    var result = checkPwdCmd.ExecuteScalar();
                    bool passwordMatches = result != DBNull.Value && Convert.ToBoolean(result);

                    if (passwordMatches)
                    {
                        DialogResult dialogResult = MessageBox.Show("Noua parolă este identică cu cea veche. Sigur doriți să continuați?",
                            "Atenție",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (dialogResult != DialogResult.Yes)
                        {
                            return;
                        }
                    }

                    // 3. Update password 
                    string alterLoginQuery = $"ALTER LOGIN [{loginName}] WITH PASSWORD = '{newPassword.Replace("'", "''")}'";
                    SqlCommand alterCmd = new SqlCommand(alterLoginQuery, conn);
                    alterCmd.ExecuteNonQuery();

                    // Reset controls
                    txtNewPassword.Text = string.Empty;
                    txtNewPassword.PasswordChar = '*';
                    var btnShowHide = currentActionPanel.Controls.OfType<Button>().FirstOrDefault(b => b.Text == "👁️");
                    if (btnShowHide != null) btnShowHide.Tag = false;

                    MessageBox.Show("Parola a fost actualizată cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Eroare SQL: {sqlEx.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare neașteptată: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowDeleteUserPanel()
        {
            currentActionPanel = CreateActionPanel("Ștergere Utilizator", 400, 250);

            // 1. Combobox pentru selectarea login-urilor existente
            int yPos = 50;
            Label lblSelectLogin = new Label
            {
                Text = "Selectează login de șters:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            currentActionPanel.Controls.Add(lblSelectLogin);

            ComboBox cmbLogins = new ComboBox
            {
                Location = new Point(20, yPos + 30),
                Width = 350,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cmbLogins"
            };
            currentActionPanel.Controls.Add(cmbLogins);

            // 2. Încarcă login-urile din SQL Server
            LoadSqlLoginsIntoComboBox(cmbLogins);

            // 3. Buton Șterge
            Button btnDelete = new Button
            {
                Text = "Șterge Login",
                Location = new Point(150, yPos + 80),
                Size = new Size(150, 40),
                BackColor = Color.LightCoral,
                ForeColor = Color.White
            };
            btnDelete.Click += (s, e) => DeleteSqlLogin(cmbLogins);
            currentActionPanel.Controls.Add(btnDelete);
        }

        private void DeleteSqlLogin(ComboBox cmbLogins)
        {
            string loginName = cmbLogins.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(loginName))
            {
                MessageBox.Show("Selectați un login de șters!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmare ștergere
            DialogResult result = MessageBox.Show($"Sigur doriți să ștergeți login-ul '{loginName}'?",
                                                "Confirmare ștergere",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                
                string masterConnectionString = connectionString.Replace("Initial Catalog=SpitalRepromed", "Initial Catalog=master");

                using (SqlConnection conn = new SqlConnection(masterConnectionString))
                {
                    conn.Open();

                    // 1. Ștergem user-ul asociat din baza de date (dacă există)
                    string dropUserQuery = $"USE SpitalRepromed; DROP USER IF EXISTS [{loginName}];";
                    SqlCommand dropUserCmd = new SqlCommand(dropUserQuery, conn);
                    dropUserCmd.ExecuteNonQuery();

                    // 2. Ștergem login-ul din server
                    string dropLoginQuery = $"DROP LOGIN [{loginName}];";
                    SqlCommand dropLoginCmd = new SqlCommand(dropLoginQuery, conn);
                    dropLoginCmd.ExecuteNonQuery();

                    // Reîncărcăm lista de logins
                    LoadSqlLoginsIntoComboBox(cmbLogins);

                    MessageBox.Show($"Login-ul '{loginName}' a fost șters cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 15151) // Cannot drop the login because it does not exist
                {
                    MessageBox.Show("Login-ul selectat nu există în sistem!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (sqlEx.Number == 15434) // Could not drop login because it is currently logged in
                {
                    MessageBox.Show($"Nu se poate șterge login-ul '{loginName}' deoarece este în uz!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Eroare SQL la ștergerea login-ului: {sqlEx.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la ștergerea login-ului: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowAddBranchPanel()
        {
            currentActionPanel = CreateActionPanel("Adăugare Filială", 350, 200);

            int yPos = 50;
            Label lblAdresa = new Label
            {
                Text = "Adresa filială:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            currentActionPanel.Controls.Add(lblAdresa);

            TextBox txtAdresa = new TextBox
            {
                Location = new Point(150, yPos),
                Width = 170,
                MaxLength = 25,
                Name = "txtAdresa"
            };
            currentActionPanel.Controls.Add(txtAdresa);

            Button btnSave = new Button
            {
                Text = "Salvează",
                Location = new Point(150, yPos + 50),
                Size = new Size(100, 40),
                BackColor = Color.LightGreen
            };
            btnSave.Click += (s, e) => AddNewBranch(txtAdresa.Text);
            currentActionPanel.Controls.Add(btnSave);
        }

        private void AddNewBranch(string adresa)
        {
            if (string.IsNullOrWhiteSpace(adresa))
            {
                MessageBox.Show("Introduceți adresa filialei!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Filiala (Adresa) VALUES (@Adresa)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Adresa", adresa);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Filiala a fost adăugată cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        currentActionPanel.Controls.Clear();
                        ShowAddBranchPanel(); // Reset form
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Eroare la adăugarea filialei: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowEditBranchPanel()
        {
            currentActionPanel = CreateActionPanel("Modificare Filială", 350, 300); 

            int yPos = 40; 

            // 1. Label și Combobox pentru selectare filială
            Label lblSelect = new Label
            {
                Text = "Selectați filiala:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };
            currentActionPanel.Controls.Add(lblSelect);

            ComboBox cmbFiliale = new ComboBox
            {
                Location = new Point(20, yPos + 25), 
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cmbFiliale"
            };
            currentActionPanel.Controls.Add(cmbFiliale);
            LoadBranchesIntoComboBox(cmbFiliale);

            // 2. Label și TextBox pentru noua adresă
            yPos += 70; // Spațiu suficient între controale
            Label lblNewAdresa = new Label
            {
                Text = "Noua adresă:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };
            currentActionPanel.Controls.Add(lblNewAdresa);

            TextBox txtNewAdresa = new TextBox
            {
                Location = new Point(20, yPos + 25), // Poziționat sub label
                Width = 300,
                MaxLength = 25,
                Name = "txtNewAdresa"
            };
            currentActionPanel.Controls.Add(txtNewAdresa);

            // 3. Buton Actualizare
            Button btnSave = new Button
            {
                Text = "Actualizează",
                Location = new Point(100, yPos + 70), // Poziționat mai jos
                Size = new Size(150, 40),
                BackColor = Color.LightBlue
            };
            btnSave.Click += (s, e) =>
            {
                if (cmbFiliale.SelectedValue != null)
                {
                    UpdateBranch(Convert.ToInt32(cmbFiliale.SelectedValue), txtNewAdresa.Text);
                }
            };
            currentActionPanel.Controls.Add(btnSave);

            // Eveniment pentru autocompletare la selectare
            cmbFiliale.SelectedIndexChanged += (s, e) =>
            {
                if (cmbFiliale.SelectedValue != null)
                {
                    txtNewAdresa.Text = cmbFiliale.Text;
                }
            };
        }

        private void UpdateBranch(int idFiliala, string newAdresa)
        {
            if (string.IsNullOrWhiteSpace(newAdresa))
            {
                MessageBox.Show("Introduceți noua adresă!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Filiala SET Adresa = @Adresa WHERE IdFiliala = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Adresa", newAdresa);
                    cmd.Parameters.AddWithValue("@Id", idFiliala);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Filiala a fost actualizată cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Filiala selectată nu a fost găsită!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Eroare la actualizarea filialei: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowDeleteBranchPanel()
        {
            currentActionPanel = CreateActionPanel("Ștergere Filială", 350, 200);

            int yPos = 50;
            Label lblSelect = new Label
            {
                Text = "Selectați filiala:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            currentActionPanel.Controls.Add(lblSelect);

            ComboBox cmbFiliale = new ComboBox
            {
                Location = new Point(150, yPos),
                Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cmbFiliale"
            };
            currentActionPanel.Controls.Add(cmbFiliale);
            LoadBranchesIntoComboBox(cmbFiliale);

            Button btnDelete = new Button
            {
                Text = "Șterge",
                Location = new Point(150, yPos + 50),
                Size = new Size(100, 40),
                BackColor = Color.LightCoral
            };
            btnDelete.Click += (s, e) =>
            {
                if (cmbFiliale.SelectedValue != null)
                {
                    DeleteBranch(Convert.ToInt32(cmbFiliale.SelectedValue));
                }
            };
            currentActionPanel.Controls.Add(btnDelete);
        }

        private void DeleteBranch(int idFiliala)
        {
            DialogResult result = MessageBox.Show("Sigur doriți să ștergeți această filială?",
                                                "Confirmare ștergere",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Filiala WHERE IdFiliala = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", idFiliala);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Filiala a fost ștearsă cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        currentActionPanel.Controls.Clear();
                        ShowDeleteBranchPanel(); // Refresh list
                    }
                    else
                    {
                        MessageBox.Show("Filiala selectată nu a fost găsită!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 547) // FK constraint
            {
                MessageBox.Show("Nu puteți șterge filiala deoarece are date asociate!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Eroare la ștergerea filialei: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBranchesIntoComboBox(ComboBox cmb)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT IdFiliala, Adresa FROM Filiala ORDER BY Adresa";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmb.DataSource = dt;
                    cmb.DisplayMember = "Adresa";
                    cmb.ValueMember = "IdFiliala";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea filialelor: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void LoadUsersIntoComboBoxWithRights(ComboBox cmbUsers, Panel pnlRights)
        {
            try
            {
                cmbUsers.Items.Clear();
                cmbUsers.BeginUpdate();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT name FROM sys.database_principals WHERE type_desc = 'SQL_USER' AND name NOT IN ('dbo', 'guest')";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbUsers.Items.Add(reader["name"].ToString());
                        }
                    }
                }

                if (cmbUsers.Items.Count > 0)
                {
                    cmbUsers.SelectedIndex = 0;
                    // Forțează încărcarea drepturilor pentru primul utilizator
                    LoadUserCurrentRights(cmbUsers.Items[0].ToString(), pnlRights);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea utilizatorilor: {ex.Message}");
            }
            finally
            {
                cmbUsers.EndUpdate();
            }
        }

        private void LoadUserCurrentRights(string userName, Panel pnlRights)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Resetăm toate checkbox-urile
                    foreach (Control ctrl in pnlRights.Controls)
                    {
                        if (ctrl is CheckBox chk)
                        {
                            chk.Checked = false;
                            chk.Enabled = true;
                        }
                    }

                    // 2. Verificăm rolurile utilizatorului
                    string roleQuery = @"
                SELECT r.name AS role_name
                FROM sys.database_role_members rm
                JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
                JOIN sys.database_principals u ON rm.member_principal_id = u.principal_id
                WHERE u.name = @userName";

                    SqlCommand roleCmd = new SqlCommand(roleQuery, conn);
                    roleCmd.Parameters.AddWithValue("@userName", userName);

                    List<string> userRoles = new List<string>();
                    using (SqlDataReader roleReader = roleCmd.ExecuteReader())
                    {
                        while (roleReader.Read())
                        {
                            userRoles.Add(roleReader["role_name"].ToString());
                        }
                    }

                    // 3. Mapăm rolurile la drepturi specifice
                    Dictionary<string, List<string>> rolePermissions = new Dictionary<string, List<string>>()
            {
                {"db_datareader", new List<string>{"SELECT"}},
                {"db_datawriter", new List<string>{"INSERT", "UPDATE", "DELETE"}},
                {"db_owner", new List<string>{"SELECT", "INSERT", "UPDATE", "DELETE", "EXECUTE"}}
                // Adăugați alte roluri dacă este necesar
            };

                    // 4. Bifăm checkbox-urile corespunzătoare
                    foreach (string role in userRoles)
                    {
                        if (rolePermissions.ContainsKey(role))
                        {
                            foreach (string permission in rolePermissions[role])
                            {
                                foreach (Control ctrl in pnlRights.Controls)
                                {
                                    if (ctrl is CheckBox chk && chk.Tag.ToString() == permission)
                                    {
                                        chk.Checked = true;
                                        chk.Enabled = false; // Facem readonly pentru drepturi din roluri
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 5. Verificăm și drepturile directe (dacă există)
                    string directPermissionsQuery = @"
                SELECT 
                    p.permission_name,
                    p.state_desc
                FROM sys.database_permissions p
                JOIN sys.database_principals pr ON p.grantee_principal_id = pr.principal_id
                WHERE pr.name = @userName";

                    SqlCommand permCmd = new SqlCommand(directPermissionsQuery, conn);
                    permCmd.Parameters.AddWithValue("@userName", userName);

                    using (SqlDataReader permReader = permCmd.ExecuteReader())
                    {
                        while (permReader.Read())
                        {
                            string permission = permReader["permission_name"].ToString();
                            string state = permReader["state_desc"].ToString();

                            foreach (Control ctrl in pnlRights.Controls)
                            {
                                if (ctrl is CheckBox chk && chk.Tag.ToString() == permission)
                                {
                                    chk.Checked = (state == "GRANT");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea drepturilor pentru {userName}: {ex.Message}");
            }
        }
        private void ShowManageRightsPanel()
        {
            currentActionPanel = CreateActionPanel("Gestionare Drepturi", 500, 450);
            int yPos = 40;

            // 1. Selectare utilizator
            Label lblUser = new Label
            {
                Text = "Selectați utilizator:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };
            currentActionPanel.Controls.Add(lblUser);

            ComboBox cmbUsers = new ComboBox
            {
                Location = new Point(20, yPos + 25),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cmbUsers"
            };
            currentActionPanel.Controls.Add(cmbUsers);

            // 2. Lista de drepturi
            yPos += 70;
            Label lblRights = new Label
            {
                Text = "Drepturi curente (✓) și disponibile:",
                Location = new Point(20, yPos),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };
            currentActionPanel.Controls.Add(lblRights);

            Panel pnlRights = new Panel
            {
                Location = new Point(20, yPos + 25),
                Width = 300,
                Height = 200,
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                Name = "pnlRights"
            };
            currentActionPanel.Controls.Add(pnlRights);

            // 3. Butoane de acțiune
            yPos += 250;
            FlowLayoutPanel pnlButtons = new FlowLayoutPanel
            {
                Location = new Point(20, yPos),
                Width = 300,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight
            };

            Button btnRevoke = new Button
            {
                Text = "Revocă drepturi",
                Size = new Size(120, 40),
                BackColor = Color.LightCoral,
                Tag = "REVOKE"
            };
            btnRevoke.Click += (s, e) => ManageUserRights(cmbUsers, pnlRights, "REVOKE");

            Button btnGrant = new Button
            {
                Text = "Acordă drepturi",
                Size = new Size(120, 40),
                BackColor = Color.LightGreen,
                Tag = "GRANT"
            };
            btnGrant.Click += (s, e) => ManageUserRights(cmbUsers, pnlRights, "GRANT");

            pnlButtons.Controls.Add(btnGrant);
            pnlButtons.Controls.Add(btnRevoke);
            currentActionPanel.Controls.Add(pnlButtons);

            // Inițializare checkbox-uri
            InitializeRightsCheckboxes(pnlRights);

            // Eveniment pentru încărcarea drepturilor
            cmbUsers.SelectedIndexChanged += (s, e) =>
            {
                if (cmbUsers.SelectedItem != null)
                {
                    LoadUserCurrentRightsForManagement(cmbUsers.SelectedItem.ToString(), pnlRights);
                }
            };

            // Încărcare utilizatori
            LoadUsersIntoComboBoxWithRights(cmbUsers, pnlRights);
        }

        private readonly Dictionary<string, string> rights = new Dictionary<string, string>
{
    {"SELECT", "Vizualizare date"},
    {"INSERT", "Adăugare înregistrări"},
    {"UPDATE", "Modificare înregistrări"},
    {"DELETE", "Ștergere înregistrări"},
    {"EXECUTE", "Executare proceduri"}
};

        private void InitializeRightsCheckboxes(Panel pnlRights)
        {
            pnlRights.Controls.Clear();

            int checkY = 10;
            foreach (var right in rights)
            {
                CheckBox chk = new CheckBox
                {
                    Text = right.Value,
                    Tag = right.Key,
                    Location = new Point(10, checkY),
                    Width = 220,
                    AutoSize = true,
                    Name = "chk" + right.Key
                };
                pnlRights.Controls.Add(chk);
                checkY += 30;
            }
        }

        private void LoadUserCurrentRightsForManagement(string userName, Panel pnlRights)
        {
            try
            {
                // 1. Resetăm toate checkbox-urile și textul lor inițial
                foreach (Control ctrl in pnlRights.Controls)
                {
                    if (ctrl is CheckBox chk)
                    {
                        chk.Checked = false;
                        chk.Enabled = true;
                        chk.ForeColor = SystemColors.ControlText;

                        // Restaurăm textul original
                        var originalText = rights.FirstOrDefault(x => x.Key == chk.Tag.ToString()).Value;
                        chk.Text = originalText;
                    }
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 2. Interogare completă pentru drepturi directe și din roluri
                    string query = @"
                -- Drepturi directe
                SELECT 
                    p.permission_name,
                    p.state_desc,
                    'DIRECT' as source,
                    CAST(1 AS BIT) as is_direct
                FROM sys.database_permissions p
                JOIN sys.database_principals pr ON p.grantee_principal_id = pr.principal_id
                WHERE pr.name = @userName
                
                UNION ALL
                
                -- Drepturi din roluri (inclusiv cele standard)
                SELECT 
                    p.permission_name,
                    p.state_desc,
                    r.name as source,
                    CAST(0 AS BIT) as is_direct
                FROM sys.database_permissions p
                JOIN sys.database_principals r ON p.grantee_principal_id = r.principal_id
                JOIN sys.database_role_members rm ON r.principal_id = rm.role_principal_id
                JOIN sys.database_principals u ON rm.member_principal_id = u.principal_id
                WHERE u.name = @userName";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userName", userName);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string permission = reader["permission_name"].ToString();
                            string state = reader["state_desc"].ToString();
                            string source = reader["source"].ToString();
                            bool isDirect = (bool)reader["is_direct"];

                            foreach (Control ctrl in pnlRights.Controls)
                            {
                                if (ctrl is CheckBox chk && chk.Tag.ToString() == permission)
                                {
                                    chk.Checked = (state == "GRANT");

                                    if (!isDirect)
                                    {
                                        chk.ForeColor = Color.Gray;
                                        chk.Text += $" (din rol: {source})";
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    // 3. Verificăm manual rolurile standard dacă nu au apărut în interogare
                    // (pentru cazurile în care rolurile nu au permisiuni directe)
                    string checkRolesQuery = @"
                SELECT r.name AS role_name
                FROM sys.database_role_members rm
                JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
                JOIN sys.database_principals u ON rm.member_principal_id = u.principal_id
                WHERE u.name = @userName";

                    SqlCommand rolesCmd = new SqlCommand(checkRolesQuery, conn);
                    rolesCmd.Parameters.AddWithValue("@userName", userName);

                    Dictionary<string, List<string>> rolePermissions = new Dictionary<string, List<string>>()
            {
                {"db_datareader", new List<string>{"SELECT"}},
                {"db_datawriter", new List<string>{"INSERT", "UPDATE", "DELETE"}},
                {"db_owner", new List<string>{"SELECT", "INSERT", "UPDATE", "DELETE", "EXECUTE"}}
            };

                    using (SqlDataReader rolesReader = rolesCmd.ExecuteReader())
                    {
                        while (rolesReader.Read())
                        {
                            string role = rolesReader["role_name"].ToString();
                            if (rolePermissions.ContainsKey(role))
                            {
                                foreach (string permission in rolePermissions[role])
                                {
                                    foreach (Control ctrl in pnlRights.Controls)
                                    {
                                        if (ctrl is CheckBox chk && chk.Tag.ToString() == permission && !chk.Checked)
                                        {
                                            chk.Checked = true;
                                            chk.ForeColor = Color.Gray;
                                            chk.Text += $" (din rol: {role})";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea drepturilor pentru {userName}: {ex.Message}\n\n{ex.StackTrace}");
            }
        }
        private void ManageUserRights(ComboBox cmbUsers, Panel pnlRights, string actionType)
        {
            string userName = cmbUsers.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(userName))
            {
                MessageBox.Show("Selectați un utilizator!");
                return;
            }

            List<string> selectedRights = new List<string>();
            foreach (Control ctrl in pnlRights.Controls)
            {
                if (ctrl is CheckBox chk && chk.Checked)
                {
                    selectedRights.Add(chk.Tag.ToString());
                }
            }

            if (selectedRights.Count == 0)
            {
                MessageBox.Show("Selectați cel puțin un drept!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    if (actionType == "REVOKE")
                    {
                        // Verificăm dacă încercăm să revocăm drepturi din roluri
                        string checkRolesQuery = @"
                    SELECT COUNT(*) 
                    FROM sys.database_permissions p
                    JOIN sys.database_principals r ON p.grantee_principal_id = r.principal_id
                    JOIN sys.database_role_members rm ON r.principal_id = rm.role_principal_id
                    JOIN sys.database_principals m ON rm.member_principal_id = m.principal_id
                    WHERE m.name = @userName AND p.permission_name IN ({0})";

                        string inClause = string.Join(",", selectedRights.Select(r => $"'{r}'"));
                        checkRolesQuery = string.Format(checkRolesQuery, inClause);

                        SqlCommand checkCmd = new SqlCommand(checkRolesQuery, conn);
                        checkCmd.Parameters.AddWithValue("@userName", userName);
                        int roleBasedPermissions = (int)checkCmd.ExecuteScalar();

                        if (roleBasedPermissions > 0)
                        {
                            DialogResult result = MessageBox.Show(
                                $"Atenție! {roleBasedPermissions} drepturi sunt moștenite din roluri. Eliminarea lor va scoate utilizatorul din rolurile respective. Continuați?",
                                "Confirmare revocare",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);

                            if (result != DialogResult.Yes)
                                return;
                        }
                    }

                    foreach (string right in selectedRights)
                    {
                        if (actionType == "GRANT")
                        {
                            string grantQuery = $"GRANT {right} TO [{userName}]";
                            new SqlCommand(grantQuery, conn).ExecuteNonQuery();
                        }
                        else // REVOKE
                        {
                            // 1. Revocăm dreptul direct (dacă există)
                            string revokeQuery = $"REVOKE {right} FROM [{userName}] CASCADE";
                            new SqlCommand(revokeQuery, conn).ExecuteNonQuery();

                            // 2. Eliminăm utilizatorul din rolurile care acordă acest drept
                            string removeRolesQuery = @"
                        DECLARE @sql NVARCHAR(MAX) = '';
                        
                        SELECT @sql = @sql + 
                            'ALTER ROLE ' + QUOTENAME(r.name) + ' DROP MEMBER ' + QUOTENAME(m.name) + ';'
                        FROM sys.database_permissions p
                        JOIN sys.database_principals r ON p.grantee_principal_id = r.principal_id
                        JOIN sys.database_role_members rm ON r.principal_id = rm.role_principal_id
                        JOIN sys.database_principals m ON rm.member_principal_id = m.principal_id
                        WHERE m.name = @userName AND p.permission_name = @permission;
                        
                        EXEC sp_executesql @sql;";

                            SqlCommand roleCmd = new SqlCommand(removeRolesQuery, conn);
                            roleCmd.Parameters.AddWithValue("@userName", userName);
                            roleCmd.Parameters.AddWithValue("@permission", right);
                            roleCmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show($"Drepturile au fost {(actionType == "GRANT" ? "acordate" : "revocate")} cu succes!");
                    LoadUserCurrentRightsForManagement(userName, pnlRights);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare: {ex.Message}\n\nDetalii: {ex.StackTrace}");
            }
        }

        private void ShowBackupRestore()
        {
            mainMenuPanel.Visible = false;
            currentActionPanel = new Panel
            {
                Width = 350,
                Height = 250,
                BackColor = Color.FromArgb(30, 30, 45),
                ForeColor = Color.White,
                Location = new Point((this.ClientSize.Width - 350) / 2, 100)
            };
            this.Controls.Add(currentActionPanel);

            Label lblTitlu = new Label
            {
                Text = "Backup & Restore",
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40
            };
            currentActionPanel.Controls.Add(lblTitlu);

            Button btnBackup = CreateStyledButton("Backup", new Point((currentActionPanel.Width - 180) / 2, 50));
            btnBackup.Click += async (s, e) => await PerformBackupAsync();
            currentActionPanel.Controls.Add(btnBackup);

            Button btnRestore = CreateStyledButton("Restore", new Point((currentActionPanel.Width - 180) / 2, 130));
            btnRestore.Click += async (s, e) => await PerformRestoreAsync();
            currentActionPanel.Controls.Add(btnRestore);

            Button btnBack = CreateBackButton();
            btnBack.Click += (s, e) => ReturnToMainMenu();
            this.Controls.Add(btnBack);
            btnBack.BringToFront();
        }

        private async Task PerformBackupAsync()
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string backupPath = string.Empty;
            Form progressForm = null;

            try
            {
                // 1. Configurare cale backup
                string backupFolder = @"C:\SQL_Backups\SpitalRepromed\";
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                backupPath = Path.Combine(backupFolder, $"SpitalRepromed_Backup_{timestamp}.bak");

                // 2. Creare folder cu permisiuni
                if (!Directory.Exists(backupFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(backupFolder);

                        var security = Directory.GetAccessControl(backupFolder);
                        security.AddAccessRule(new FileSystemAccessRule(
                            "NT SERVICE\\MSSQLSERVER",
                            FileSystemRights.FullControl,
                            InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                            PropagationFlags.None,
                            AccessControlType.Allow));

                        security.AddAccessRule(new FileSystemAccessRule(
                            Environment.UserName,
                            FileSystemRights.FullControl,
                            InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                            PropagationFlags.None,
                            AccessControlType.Allow));

                        Directory.SetAccessControl(backupFolder, security);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Aplicația necesită drepturi de Administrator pentru a configura folderul de backup.",
                                      "Eroare permisiuni", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // 3. Verificare spațiu pe disc
                DriveInfo drive = new DriveInfo(Path.GetPathRoot(backupFolder));
                if (drive.AvailableFreeSpace < 1073741824)
                {
                    MessageBox.Show($"Spațiu insuficient pe disc ({drive.Name}). Minim 1GB necesar.",
                                  "Eroare spațiu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 4. Afișare formular de așteptare
                progressForm = CreateProgressForm("Backup în curs...");
                progressForm.Show();

                // 5. Detectare ediție SQL Server
                conn = new SqlConnection(connectionString);
                await conn.OpenAsync();

                bool isExpressEdition = false;
                using (SqlCommand editionCmd = new SqlCommand("SELECT SERVERPROPERTY('Edition')", conn))
                {
                    string edition = (await editionCmd.ExecuteScalarAsync())?.ToString() ?? "";
                    isExpressEdition = edition.Contains("Express");
                }

                // 6. Construire comanda BACKUP
                string backupQuery = $@"
BACKUP DATABASE [SpitalRepromed] 
TO DISK = '{backupPath}'
WITH 
    {(isExpressEdition ? "" : "COMPRESSION,")}
    STATS = 5,
    CHECKSUM,
    NAME = N'SpitalRepromed-Full Backup',
    DESCRIPTION = N'Backup creat la {DateTime.Now:yyyy-MM-dd HH:mm}';";

                // 7. Executare backup
                using (cmd = new SqlCommand(backupQuery, conn))
                {
                    await cmd.ExecuteNonQueryAsync();

                    // Verificare integritate
                    string verifyQuery = $@"RESTORE VERIFYONLY FROM DISK = '{backupPath}' WITH CHECKSUM;";
                    using (SqlCommand verifyCmd = new SqlCommand(verifyQuery, conn))
                    {
                        await verifyCmd.ExecuteNonQueryAsync();
                    }
                }

                // 8. Afișare rezultat final
                FileInfo backupFile = new FileInfo(backupPath);
                string resultMessage = $"Backup realizat cu succes!\n\n" +
                                     $"Locație: {backupPath}\n" +
                                     $"Mărime: {backupFile.Length / 1024 / 1024} MB\n" +
                                     $"Ediție SQL: {(isExpressEdition ? "Express (fără compresie)" : "Standard/Enterprise (cu compresie)")}";

                MessageBox.Show(resultMessage, "Backup complet", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException sqlEx)
            {
                string errorDetails = sqlEx.Message;
                if (errorDetails.Contains("COMPRESSION is not supported"))
                {
                    errorDetails += "\n\nSoluție: Ediția Express nu suportă compresia. Încercați din nou fără compresie.";
                }

                MessageBox.Show($"Eroare SQL la backup:\n{errorDetails}",
                               "Eroare backup", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (File.Exists(backupPath))
                {
                    try { File.Delete(backupPath); } catch { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare generală:\n{ex.Message}",
                               "Eroare critică", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressForm?.Close();
                cmd?.Dispose();
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn?.Dispose();
            }
        }

        private async Task<StringBuilder> GenerateSqlFromBak(string bakFilePath)
        {
            StringBuilder sqlScript = new StringBuilder();
            string tempDbName = "TempRestore_" + Guid.NewGuid().ToString("N");
            string backupFolder = Path.GetDirectoryName(bakFilePath);
            string tempFolder = Path.Combine(backupFolder, "TempRestore");

            try
            {
                // 1. Creează folder temporar în același loc cu backup-urile
                if (!Directory.Exists(tempFolder))
                {
                    Directory.CreateDirectory(tempFolder);

                    // Setează permisiuni
                    var security = Directory.GetAccessControl(tempFolder);
                    security.AddAccessRule(new FileSystemAccessRule(
                        "NT SERVICE\\MSSQLSERVER",
                        FileSystemRights.FullControl,
                        InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                        PropagationFlags.None,
                        AccessControlType.Allow));
                    Directory.SetAccessControl(tempFolder, security);
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    // 2. Obține lista fișiere din backup
                    DataTable fileList = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(
                        $"RESTORE FILELISTONLY FROM DISK = '{bakFilePath}'", conn))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(fileList);
                    }

                    // 3. Construiește căile pentru fișierele temporare
                    string mdfPath = Path.Combine(tempFolder, tempDbName + ".mdf");
                    string ldfPath = Path.Combine(tempFolder, tempDbName + ".ldf");

                    // 4. Comanda RESTORE
                    string restoreCmd = $@"
            CREATE DATABASE [{tempDbName}];
            RESTORE DATABASE [{tempDbName}] 
            FROM DISK = '{bakFilePath}' 
            WITH 
                MOVE '{fileList.Rows[0]["LogicalName"]}' TO '{mdfPath}',
                MOVE '{fileList.Rows[1]["LogicalName"]}' TO '{ldfPath}',
                REPLACE, RECOVERY;";

                    // 5. Execută restore
                    using (SqlCommand cmd = new SqlCommand(restoreCmd, conn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }

                    // 6. Generează script SQL
                    sqlScript.AppendLine("-- Script generat din: " + bakFilePath);
                    sqlScript.AppendLine("-- Data: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    sqlScript.AppendLine();

                    // 7. Obține toate tabelele
                    DataTable tables = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(
                        $"SELECT * FROM [{tempDbName}].INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'", conn))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(tables);
                    }

                    // 8. Pentru fiecare tabelă
                    foreach (DataRow table in tables.Rows)
                    {
                        string schema = table["TABLE_SCHEMA"].ToString();
                        string name = table["TABLE_NAME"].ToString();
                        string fullName = $"[{schema}].[{name}]";

                        // 9. Adaugă CREATE TABLE
                        sqlScript.AppendLine($"-- Tabela: {fullName}");
                        using (SqlCommand cmd = new SqlCommand(
                            $"SELECT definition FROM [{tempDbName}].sys.sql_modules WHERE object_id=OBJECT_ID('[{tempDbName}].{fullName}')", conn))
                        {
                            var def = await cmd.ExecuteScalarAsync();
                            if (def != null) sqlScript.AppendLine(def.ToString());
                        }
                        sqlScript.AppendLine("GO");

                        // 10. Adaugă datele (INSERT)
                        sqlScript.AppendLine($"-- Date pentru {fullName}");
                        using (SqlCommand cmd = new SqlCommand($"SELECT * FROM [{tempDbName}].{fullName}", conn))
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                sqlScript.Append($"INSERT INTO {fullName} VALUES (");
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (i > 0) sqlScript.Append(", ");
                                    sqlScript.Append(reader.IsDBNull(i) ? "NULL" : $"'{reader.GetValue(i).ToString().Replace("'", "''")}'");
                                }
                                sqlScript.AppendLine(");");
                            }
                        }
                        sqlScript.AppendLine("GO");
                        sqlScript.AppendLine();
                    }

                    // 11. Curăță baza temporară
                    string cleanupDb = $@"
            ALTER DATABASE [{tempDbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            DROP DATABASE [{tempDbName}];";
                    await new SqlCommand(cleanupDb, conn).ExecuteNonQueryAsync();
                }

                return sqlScript;
            }
            finally
            {
                // 12. Șterge fișierele temporare
                try
                {
                    if (Directory.Exists(tempFolder))
                    {
                        foreach (var file in Directory.GetFiles(tempFolder, tempDbName + ".*"))
                        {
                            File.Delete(file);
                        }
                    }
                }
                catch { /* Ignoră erorile */ }
            }
        }

        private async Task PerformRestoreAsync()
        {
            // 1. Selectare fișier .bak
            OpenFileDialog openBakDialog = new OpenFileDialog
            {
                Filter = "Backup files (*.bak)|*.bak",
                InitialDirectory = @"C:\SQL_Backups\SpitalRepromed\",
                Title = "Selectați fișierul .bak pentru conversie"
            };

            if (openBakDialog.ShowDialog() == DialogResult.OK)
            {
                string bakFilePath = openBakDialog.FileName;

                // 2. Selectare locație salvare .sql
                SaveFileDialog saveSqlDialog = new SaveFileDialog
                {
                    Filter = "SQL files (*.sql)|*.sql",
                    FileName = $"Restored_{Path.GetFileNameWithoutExtension(bakFilePath)}.sql",
                    InitialDirectory = Path.GetDirectoryName(bakFilePath),
                    Title = "Salvați fișierul SQL generat"
                };

                if (saveSqlDialog.ShowDialog() == DialogResult.OK)
                {
                    string sqlFilePath = saveSqlDialog.FileName;

                    try
                    {
                        using (var progressForm = CreateProgressForm("Generare fișier SQL..."))
                        {
                            progressForm.Show();

                            // 3. Generare script SQL din .bak
                            StringBuilder sqlScript = await GenerateSqlFromBak(bakFilePath);

                            // 4. Salvarea în fișier
                            await Task.Run(() => File.WriteAllText(sqlFilePath, sqlScript.ToString()));

                            MessageBox.Show($"Fișier SQL generat cu succes:\n{sqlFilePath}",
                                          "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Eroare la generare SQL:\n{ex.Message}",
                                      "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private Form CreateProgressForm(string message)
        {
            var form = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                Text = "Vă rugăm așteptați",
                ControlBox = false
            };

            var label = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 10)
            };

            var progressBar = new ProgressBar
            {
                Dock = DockStyle.Bottom,
                Style = ProgressBarStyle.Marquee,
                Height = 30
            };

            form.Controls.Add(label);
            form.Controls.Add(progressBar);

            return form;
        }
    }
    }
    