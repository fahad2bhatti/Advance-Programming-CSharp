// =============================================
// HospitalDAL - Patient.cs
// Model Class (Entity)
// =============================================

namespace HospitalDAL
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Disease { get; set; }
        public string DoctorName { get; set; }
        public System.DateTime AdmitDate { get; set; }
    }
}