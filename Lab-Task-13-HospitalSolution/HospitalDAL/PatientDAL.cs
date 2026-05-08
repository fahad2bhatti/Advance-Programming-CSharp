// =============================================
// HospitalDAL - PatientDAL.cs
// Data Access Layer — Talks directly to SQL Server
// =============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HospitalDAL
{
    public class PatientDAL
    {
        private string connectionString = @"Server=THINKPADX13\SQLEXPRESS;Database=HospitalDB;Integrated Security=True;";

        // ── INSERT ──────────────────────────────────
        public bool InsertPatient(Patient p)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_InsertPatient", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", p.Name);
                        cmd.Parameters.AddWithValue("@Age", p.Age);
                        cmd.Parameters.AddWithValue("@Gender", p.Gender);
                        cmd.Parameters.AddWithValue("@Disease", p.Disease);
                        cmd.Parameters.AddWithValue("@DoctorName", p.DoctorName);
                        cmd.Parameters.AddWithValue("@AdmitDate", p.AdmitDate.Date);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("DAL InsertPatient Error: " + ex.Message);
            }
        }

        // ── SEARCH ──────────────────────────────────
        public List<Patient> SearchPatient(string name)
        {
            List<Patient> list = new List<Patient>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SearchPatient", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", name);

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(new Patient
                            {
                                PatientID = Convert.ToInt32(reader["PatientID"]),
                                Name = reader["Name"].ToString(),
                                Age = Convert.ToInt32(reader["Age"]),
                                Gender = reader["Gender"].ToString(),
                                Disease = reader["Disease"].ToString(),
                                DoctorName = reader["DoctorName"].ToString(),
                                AdmitDate = Convert.ToDateTime(reader["AdmitDate"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DAL SearchPatient Error: " + ex.Message);
            }
            return list;
        }

        // ── DELETE ──────────────────────────────────
        public bool DeletePatient(int patientID)
        {
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
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("DAL DeletePatient Error: " + ex.Message);
            }
        }

        // ── GET ALL ─────────────────────────────────
        public List<Patient> GetAllPatients()
        {
            List<Patient> list = new List<Patient>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Patients ORDER BY PatientID DESC", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new Patient
                        {
                            PatientID = Convert.ToInt32(reader["PatientID"]),
                            Name = reader["Name"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Gender = reader["Gender"].ToString(),
                            Disease = reader["Disease"].ToString(),
                            DoctorName = reader["DoctorName"].ToString(),
                            AdmitDate = Convert.ToDateTime(reader["AdmitDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DAL GetAllPatients Error: " + ex.Message);
            }
            return list;
        }
    }
}