// =============================================
// HospitalUI - Form1.cs
// Presentation Layer — Calls BLL only
// =============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HospitalBLL;   // ← Reference to BLL namespace
using HospitalDAL;   // ← Reference to DAL namespace (for Patient model)

namespace HospitalUI
{
    public partial class Form1 : Form
    {
        // UI only talks to BLL — never to DAL directly
        private PatientBLL bll = new PatientBLL();

        // Controls
        private TextBox txtName, txtAge, txtDisease, txtDoctorName, txtSearchName, txtDeleteID;
        private ComboBox cboGender;
        private DateTimePicker dtpAdmitDate;
        private DataGridView dgvPatients;
        private Button btnInsert, btnSearch, btnDelete, btnLoadAll;

        public Form1()
        {
            InitializeComponent();
            BuildUI();
            LoadAll();
        }

        // =============================================
        // BUILD UI
        // =============================================
        private void BuildUI()
        {
            this.Text = "Hospital Management — BLL/DAL Architecture";
            this.Size = new Size(950, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.Font = new Font("Segoe UI", 9.5f);

            // Title
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(0, 102, 179) };
            var lblTitle = new Label
            {
                Text = "🏥  Hospital Patient Management  |  BLL + DAL Architecture",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelTop.Controls.Add(lblTitle);
            this.Controls.Add(panelTop);

            // Architecture label
            var lblArch = new Label
            {
                Text = "Flow:  UI (Presentation)  →  BLL (Business Logic)  →  DAL (Data Access)  →  SQL Server",
                Location = new Point(10, 68),
                Size = new Size(920, 22),
                ForeColor = Color.FromArgb(0, 102, 179),
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblArch);

            // Input Panel
            var panel = new Panel
            {
                Location = new Point(10, 95),
                Size = new Size(920, 210),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Row 1 — Name, Age, Gender, Date
            AddLabel(panel, "Patient Name:", 10, 15);
            txtName = AddTextBox(panel, 120, 12, 190);

            AddLabel(panel, "Age:", 325, 15);
            txtAge = AddTextBox(panel, 360, 12, 60);

            AddLabel(panel, "Gender:", 435, 15);
            cboGender = new ComboBox { Location = new Point(490, 12), Size = new Size(100, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboGender.Items.AddRange(new string[] { "Male", "Female", "Other" });
            cboGender.SelectedIndex = 0;
            panel.Controls.Add(cboGender);

            AddLabel(panel, "Admit Date:", 605, 15);
            dtpAdmitDate = new DateTimePicker { Location = new Point(690, 12), Size = new Size(120, 25), Format = DateTimePickerFormat.Short };
            panel.Controls.Add(dtpAdmitDate);

            // Row 2 — Disease, Doctor
            AddLabel(panel, "Disease:", 10, 55);
            txtDisease = AddTextBox(panel, 120, 52, 250);

            AddLabel(panel, "Doctor Name:", 385, 55);
            txtDoctorName = AddTextBox(panel, 480, 52, 200);

            // Insert Button
            btnInsert = MakeButton(panel, "➕  Insert Patient", 10, 95, 160, 36, Color.FromArgb(0, 153, 76));
            btnInsert.Click += BtnInsert_Click;

            // Divider
            var lbl = new Label { Text = new string('─', 120), Location = new Point(10, 145), AutoSize = true, ForeColor = Color.LightGray };
            panel.Controls.Add(lbl);

            // Search Row
            AddLabel(panel, "Search by Name:", 10, 165);
            txtSearchName = AddTextBox(panel, 125, 162, 180);
            btnSearch = MakeButton(panel, "🔍 Search", 318, 160, 110, 30, Color.FromArgb(0, 102, 179));
            btnSearch.Click += BtnSearch_Click;

            // Delete Row
            AddLabel(panel, "Delete by ID:", 450, 165);
            txtDeleteID = AddTextBox(panel, 545, 162, 70);
            btnDelete = MakeButton(panel, "🗑 Delete", 628, 160, 110, 30, Color.FromArgb(204, 0, 0));
            btnDelete.Click += BtnDelete_Click;

            // Load All
            btnLoadAll = MakeButton(panel, "🔄 Load All", 755, 160, 110, 30, Color.FromArgb(100, 100, 100));
            btnLoadAll.Click += (s, e) => LoadAll();

            this.Controls.Add(panel);

            // DataGridView
            dgvPatients = new DataGridView
            {
                Location = new Point(10, 315),
                Size = new Size(920, 350),
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
            this.Controls.Add(dgvPatients);
        }

        // =============================================
        // BUTTON EVENTS — UI calls BLL only
        // =============================================

        private void BtnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                // Build Patient object
                Patient p = new Patient
                {
                    Name = txtName.Text.Trim(),
                    Age = int.TryParse(txtAge.Text, out int a) ? a : 0,
                    Gender = cboGender.SelectedItem.ToString(),
                    Disease = txtDisease.Text.Trim(),
                    DoctorName = txtDoctorName.Text.Trim(),
                    AdmitDate = dtpAdmitDate.Value
                };

                // Call BLL (BLL validates → calls DAL → DAL hits DB)
                bll.InsertPatient(p);

                MessageBox.Show("Patient inserted successfully! ✅", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // Call BLL
                List<Patient> results = bll.SearchPatient(txtSearchName.Text.Trim());
                BindGrid(results);

                if (results.Count == 0)
                    MessageBox.Show("No patient found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtDeleteID.Text, out int id))
                {
                    MessageBox.Show("Enter a valid Patient ID.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show($"Delete Patient ID {id}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                // Call BLL
                bll.DeletePatient(id);

                MessageBox.Show("Patient deleted successfully! ✅", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDeleteID.Clear();
                LoadAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // HELPERS
        // =============================================

        private void LoadAll()
        {
            try
            {
                List<Patient> all = bll.GetAllPatients();
                BindGrid(all);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindGrid(List<Patient> list)
        {
            var dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Age");
            dt.Columns.Add("Gender");
            dt.Columns.Add("Disease");
            dt.Columns.Add("Doctor");
            dt.Columns.Add("Admit Date");

            foreach (var p in list)
                dt.Rows.Add(p.PatientID, p.Name, p.Age, p.Gender, p.Disease, p.DoctorName, p.AdmitDate.ToShortDateString());

            dgvPatients.DataSource = dt;
        }

        private void ClearFields()
        {
            txtName.Clear(); txtAge.Clear(); txtDisease.Clear(); txtDoctorName.Clear();
            cboGender.SelectedIndex = 0;
            dtpAdmitDate.Value = DateTime.Today;
        }

        private Label AddLabel(Panel p, string text, int x, int y)
        {
            var lbl = new Label { Text = text, Location = new Point(x, y), AutoSize = true, ForeColor = Color.FromArgb(50, 50, 50) };
            p.Controls.Add(lbl);
            return lbl;
        }

        private TextBox AddTextBox(Panel p, int x, int y, int w)
        {
            var tb = new TextBox { Location = new Point(x, y), Size = new Size(w, 25) };
            p.Controls.Add(tb);
            return tb;
        }

        private Button MakeButton(Panel p, string text, int x, int y, int w, int h, Color color)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            p.Controls.Add(btn);
            return btn;
        }
    }
}