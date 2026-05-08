// =============================================
// Lab Task 12: Hospital Patient Management
// Windows Forms Application - C#
// =============================================
// 
// SETUP INSTRUCTIONS:
// 1. Open Visual Studio Community
// 2. Create New Project -> Windows Forms App (.NET Framework)
// 3. Name it: HospitalManagement
// 4. Replace Form1.cs content with this code
// 5. Add NuGet Package: System.Data.SqlClient (if needed)
// 6. Update connection string with your SQL Server name
// =============================================

using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class Form1 : Form
    {
        // *** UPDATE THIS CONNECTION STRING ***
        private string connectionString = @"Server=THINKPADX13\SQLEXPRESS;Database=HospitalDB;Integrated Security=True;";

        // UI Controls
        private TextBox txtName, txtAge, txtDisease, txtDoctorName, txtSearchName, txtDeleteID;
        private ComboBox cboGender;
        private DateTimePicker dtpAdmitDate;
        private DataGridView dgvPatients;
        private Button btnInsert, btnSearch, btnDelete, btnLoadAll;
        private Label lblTitle;
        private Panel panelTop, panelForm, panelActions;

        public Form1()
        {
            InitializeComponent();
            BuildUI();
            LoadAllPatients();
        }

        // =============================================
        // BUILD UI DYNAMICALLY
        // =============================================
        private void BuildUI()
        {
            this.Text = "Hospital Patient Management System";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.Font = new Font("Segoe UI", 9.5f);

            // --- Title Panel ---
            panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(0, 102, 179)
            };
            lblTitle = new Label
            {
                Text = "🏥  Hospital Patient Management",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelTop.Controls.Add(lblTitle);

            // --- Form Panel (Input Fields) ---
            panelForm = new Panel
            {
                Location = new Point(10, 70),
                Size = new Size(870, 200),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Row 1
            AddLabel(panelForm, "Patient Name:", 10, 15);
            txtName = AddTextBox(panelForm, 130, 12, 200);

            AddLabel(panelForm, "Age:", 345, 15);
            txtAge = AddTextBox(panelForm, 390, 12, 80);

            AddLabel(panelForm, "Gender:", 485, 15);
            cboGender = new ComboBox
            {
                Location = new Point(545, 12),
                Size = new Size(100, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboGender.Items.AddRange(new string[] { "Male", "Female", "Other" });
            cboGender.SelectedIndex = 0;
            panelForm.Controls.Add(cboGender);

            AddLabel(panelForm, "Admit Date:", 660, 15);
            dtpAdmitDate = new DateTimePicker
            {
                Location = new Point(750, 12),
                Size = new Size(110, 25),
                Format = DateTimePickerFormat.Short
            };
            panelForm.Controls.Add(dtpAdmitDate);

            // Row 2
            AddLabel(panelForm, "Disease:", 10, 55);
            txtDisease = AddTextBox(panelForm, 130, 52, 300);

            AddLabel(panelForm, "Doctor Name:", 445, 55);
            txtDoctorName = AddTextBox(panelForm, 560, 52, 200);

            // Insert Button
            btnInsert = new Button
            {
                Text = "➕  Insert Patient",
                Location = new Point(10, 90),
                Size = new Size(160, 38),
                BackColor = Color.FromArgb(0, 153, 76),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnInsert.FlatAppearance.BorderSize = 0;
            btnInsert.Click += BtnInsert_Click;
            panelForm.Controls.Add(btnInsert);

            // Separator line
            AddLabel(panelForm, "──────────────────────────────────────────────────────────────────────────────────", 10, 140);

            // Search Row
            AddLabel(panelForm, "Search by Name:", 10, 160);
            txtSearchName = AddTextBox(panelForm, 130, 157, 200);
            btnSearch = new Button
            {
                Text = "🔍  Search",
                Location = new Point(345, 155),
                Size = new Size(120, 32),
                BackColor = Color.FromArgb(0, 102, 179),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;
            panelForm.Controls.Add(btnSearch);

            // Delete Row
            AddLabel(panelForm, "Delete by ID:", 490, 160);
            txtDeleteID = AddTextBox(panelForm, 600, 157, 80);
            btnDelete = new Button
            {
                Text = "🗑  Delete",
                Location = new Point(695, 155),
                Size = new Size(110, 32),
                BackColor = Color.FromArgb(204, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;
            panelForm.Controls.Add(btnDelete);

            // Load All Button
            btnLoadAll = new Button
            {
                Text = "🔄  Load All Patients",
                Location = new Point(820, 155),
                Size = new Size(40, 32),
                BackColor = Color.FromArgb(100, 100, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            // --- DataGridView ---
            dgvPatients = new DataGridView
            {
                Location = new Point(10, 280),
                Size = new Size(865, 350),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                GridColor = Color.FromArgb(220, 220, 220)
            };
            dgvPatients.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 179);
            dgvPatients.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPatients.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvPatients.EnableHeadersVisualStyles = false;
            dgvPatients.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);

            // Add to Form
            this.Controls.Add(panelTop);
            this.Controls.Add(panelForm);
            this.Controls.Add(dgvPatients);
        }

        private Label AddLabel(Panel p, string text, int x, int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = Color.FromArgb(50, 50, 50)
            };
            p.Controls.Add(lbl);
            return lbl;
        }

        private TextBox AddTextBox(Panel p, int x, int y, int width)
        {
            var tb = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 25)
            };
            p.Controls.Add(tb);
            return tb;
        }

        // =============================================
        // DATABASE OPERATIONS
        // =============================================

        // --- INSERT PATIENT ---
        private void BtnInsert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                string.IsNullOrWhiteSpace(txtDisease.Text) ||
                string.IsNullOrWhiteSpace(txtDoctorName.Text))
            {
                MessageBox.Show("Please fill all fields!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age <= 0 || age > 150)
            {
                MessageBox.Show("Please enter a valid age!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_InsertPatient", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Age", age);
                        cmd.Parameters.AddWithValue("@Gender", cboGender.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@Disease", txtDisease.Text.Trim());
                        cmd.Parameters.AddWithValue("@DoctorName", txtDoctorName.Text.Trim());
                        cmd.Parameters.AddWithValue("@AdmitDate", dtpAdmitDate.Value.Date);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Patient inserted successfully! ✅", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields();
                LoadAllPatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- SEARCH PATIENT ---
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchName.Text))
            {
                MessageBox.Show("Please enter a name to search!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SearchPatient", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", txtSearchName.Text.Trim());

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgvPatients.DataSource = dt;

                        if (dt.Rows.Count == 0)
                            MessageBox.Show("No patient found with that name.", "Search Result",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- DELETE PATIENT ---
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDeleteID.Text))
            {
                MessageBox.Show("Please enter a Patient ID to delete!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtDeleteID.Text, out int patientID) || patientID <= 0)
            {
                MessageBox.Show("Please enter a valid Patient ID!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete Patient ID: {patientID}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_DeletePatient", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PatientID", patientID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Patient deleted successfully! ✅", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtDeleteID.Clear();
                LoadAllPatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- LOAD ALL PATIENTS ---
        private void LoadAllPatients()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT * FROM Patients ORDER BY PatientID DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvPatients.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not connect to database.\nError: " + ex.Message,
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- CLEAR FIELDS ---
        private void ClearFields()
        {
            txtName.Clear();
            txtAge.Clear();
            txtDisease.Clear();
            txtDoctorName.Clear();
            cboGender.SelectedIndex = 0;
            dtpAdmitDate.Value = DateTime.Today;
        }
    }
}