using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQDemo
{
    // Custom Object
    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double Marks { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Collection of custom objects
            List<Student> students = new List<Student>()
            {
                new Student { Id = 1, Name = "Ali",     Age = 20, Marks = 85.5 },
                new Student { Id = 2, Name = "Sara",    Age = 22, Marks = 91.0 },
                new Student { Id = 3, Name = "Ahmed",   Age = 19, Marks = 73.0 },
                new Student { Id = 4, Name = "Zara",    Age = 21, Marks = 88.0 },
                new Student { Id = 5, Name = "Bilal",   Age = 20, Marks = 60.5 },
                new Student { Id = 6, Name = "Hina",    Age = 23, Marks = 95.0 },
            };

            // ─────────────────────────────────────────
            // 1. SELECT – get only names
            // ─────────────────────────────────────────
            Console.WriteLine("=== SELECT: All Student Names ===");
            var names = students.Select(s => s.Name);
            foreach (var name in names)
                Console.WriteLine(name);

            // ─────────────────────────────────────────
            // 2. WHERE – filter students with Marks > 80
            // ─────────────────────────────────────────
            Console.WriteLine("\n=== WHERE: Students with Marks > 80 ===");
            var topStudents = students.Where(s => s.Marks > 80);
            foreach (var s in topStudents)
                Console.WriteLine($"Name: {s.Name}, Marks: {s.Marks}");

            // ─────────────────────────────────────────
            // 3. ORDER BY – sort by Marks descending
            // ─────────────────────────────────────────
            Console.WriteLine("\n=== ORDER BY: Sorted by Marks (High to Low) ===");
            var sorted = students.OrderByDescending(s => s.Marks);
            foreach (var s in sorted)
                Console.WriteLine($"Name: {s.Name}, Marks: {s.Marks}");

            // ─────────────────────────────────────────
            // 4. COMBINED – WHERE + ORDER BY + SELECT
            // ─────────────────────────────────────────
            Console.WriteLine("\n=== COMBINED: Top Students (Marks > 80) Sorted by Marks ===");
            var combined = students
                .Where(s => s.Marks > 80)
                .OrderByDescending(s => s.Marks)
                .Select(s => new { s.Name, s.Marks });

            foreach (var s in combined)
                Console.WriteLine($"Name: {s.Name}, Marks: {s.Marks}");

            Console.ReadKey();
        }
    }
}