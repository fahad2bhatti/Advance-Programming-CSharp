// =============================================
// HospitalBLL - PatientBLL.cs
// Business Logic Layer — Validates & calls DAL
// =============================================

using System;
using System.Collections.Generic;
using HospitalDAL;  // ← Reference to DAL namespace

namespace HospitalBLL
{
    public class PatientBLL
    {
        // BLL creates DAL object — UI never touches DAL directly
        private PatientDAL dal = new PatientDAL();

        // ── INSERT with Validation ───────────────────
        public bool InsertPatient(Patient p)
        {
            // Business Rules
            if (string.IsNullOrWhiteSpace(p.Name))
                throw new Exception("Patient name cannot be empty.");

            if (p.Age <= 0 || p.Age > 150)
                throw new Exception("Age must be between 1 and 150.");

            if (string.IsNullOrWhiteSpace(p.Disease))
                throw new Exception("Disease field cannot be empty.");

            if (string.IsNullOrWhiteSpace(p.DoctorName))
                throw new Exception("Doctor name cannot be empty.");

            if (p.AdmitDate > DateTime.Today)
                throw new Exception("Admit date cannot be in the future.");

            // All validations passed — call DAL
            return dal.InsertPatient(p);
        }

        // ── SEARCH with Validation ───────────────────
        public List<Patient> SearchPatient(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Please enter a name to search.");

            return dal.SearchPatient(name);
        }

        // ── DELETE with Validation ───────────────────
        public bool DeletePatient(int patientID)
        {
            if (patientID <= 0)
                throw new Exception("Invalid Patient ID.");

            return dal.DeletePatient(patientID);
        }

        // ── GET ALL ─────────────────────────────────
        public List<Patient> GetAllPatients()
        {
            return dal.GetAllPatients();
        }
    }
}