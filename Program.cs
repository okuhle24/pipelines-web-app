using System;
using System.Collections.Generic;
using System.Linq;

public class Student
{
    public int Id { get; }
    public string Name { get; }
    public DateTime DateOfBirth { get; }
    public string ContactInfo { get; }
    public Dictionary<int, List<double>> Grades { get; } = new Dictionary<int, List<double>>();
    public Dictionary<int, List<bool>> Attendance { get; } = new Dictionary<int, List<bool>>();

    public Student(int id, string name, DateTime dateOfBirth, string contactInfo)
    {
        Id = id;
        Name = name;
        DateOfBirth = dateOfBirth;
        ContactInfo = contactInfo;
    }
}

public class Course
{
    public int Id { get; }
    public string CourseName { get; }

    public Course(int id, string courseName)
    {
        Id = id;
        CourseName = courseName;
    }
}

public class StudentManagementSystem
{
    private readonly List<Student> _students = new List<Student>();
    private readonly List<Course> _courses = new List<Course>();
    private int _studentIdCounter = 1;
    private int _courseIdCounter = 1;

    // Method to add a new student
    public void AddStudent(string name, DateTime dateOfBirth, string contactInfo)
    {
        var student = new Student(_studentIdCounter++, name, dateOfBirth, contactInfo);
        _students.Add(student);
        Console.WriteLine($"Student added: {student.Name}, ID: {student.Id}");
    }

    // Method to enroll a student in a course
    public void EnrollStudentInCourse(int studentId, int courseId)
    {
        var student = _students.FirstOrDefault(s => s.Id == studentId);
        if (student != null)
        {
            if (!student.Attendance.ContainsKey(courseId))
            {
                student.Attendance[courseId] = new List<bool>();
                student.Grades[courseId] = new List<double>();
                Console.WriteLine($"Student ID {studentId} enrolled in course ID {courseId}.");
            }
            else
            {
                Console.WriteLine($"Student ID {studentId} is already enrolled in course ID {courseId}.");
            }
        }
        else
        {
            Console.WriteLine($"Student ID {studentId} not found.");
        }
    }

    // Method to record attendance for a student in a course
    public void RecordAttendance(int studentId, int courseId, DateTime date, bool isPresent)
    {
        var student = _students.FirstOrDefault(s => s.Id == studentId);
        if (student != null && student.Attendance.ContainsKey(courseId))
        {
            student.Attendance[courseId].Add(isPresent);
            Console.WriteLine($"Attendance recorded for Student ID {studentId} in Course ID {courseId} on {date.ToShortDateString()}: {(isPresent ? "Present" : "Absent")}");
        }
        else
        {
            Console.WriteLine($"Student ID {studentId} not found or not enrolled in Course ID {courseId}.");
        }
    }

    // Method to add a grade for a student in a course
    public void AddGrade(int studentId, int courseId, string assessment, double grade)
    {
        var student = _students.FirstOrDefault(s => s.Id == studentId);
        if (student != null && student.Grades.ContainsKey(courseId))
        {
            student.Grades[courseId].Add(grade);
            Console.WriteLine($"Grade added for Student ID {studentId} in Course ID {courseId}: {assessment} - {grade}");
        }
        else
        {
            Console.WriteLine($"Student ID {studentId} not found or not enrolled in Course ID {courseId}.");
        }
    }

    // Method to generate a report card for a student
    public void GenerateReportCard(int studentId)
    {
        var student = _students.FirstOrDefault(s => s.Id == studentId);
        if (student != null)
        {
            Console.WriteLine($"\nReport Card for Student ID {student.Id}: {student.Name}");
            foreach (var courseId in student.Grades.Keys)
            {
                var grades = student.Grades[courseId];
                var attendance = student.Attendance[courseId];
                double averageGrade = grades.Count > 0 ? grades.Average() : 0;
                int totalClasses = attendance.Count;
                int classesAttended = attendance.Count(a => a);

                Console.WriteLine($"Course ID: {courseId} - Average Grade: {averageGrade:F2}, Attendance: {classesAttended}/{totalClasses}");
            }
        }
        else
        {
            Console.WriteLine($"Student ID {studentId} not found.");
        }
    }

    // Method to add a new course
    public void AddCourse(string courseName)
    {
        var course = new Course(_courseIdCounter++, courseName);
        _courses.Add(course);
        Console.WriteLine($"Course added: {course.CourseName}, ID: {course.Id}");
    }
}

// Main Program
class Program
{
    static void Main(string[] args)
    {
        StudentManagementSystem sms = new StudentManagementSystem();

        // Adding courses
        sms.AddCourse("Mathematics");
        sms.AddCourse("Science");

        // Adding students
        sms.AddStudent("Alice Johnson", new DateTime(2000, 5, 21), "alice.j@example.com");
        sms.AddStudent("Bob Smith", new DateTime(2001, 8, 10), "bob.s@example.com");

        // Enrolling students in courses
        sms.EnrollStudentInCourse(1, 1); // Alice in Mathematics
        sms.EnrollStudentInCourse(1, 2); // Alice in Science
        sms.EnrollStudentInCourse(2, 1); // Bob in Mathematics

        // Recording attendance
        sms.RecordAttendance(1, 1, DateTime.Now, true);
        sms.RecordAttendance(1, 1, DateTime.Now.AddDays(-1), false);
        sms.RecordAttendance(2, 1, DateTime.Now, true);

        // Adding grades
        sms.AddGrade(1, 1, "Midterm", 88.5);
        sms.AddGrade(1, 1, "Final", 92.0);
        sms.AddGrade(2, 1, "Midterm", 75.0);

        // Generating report cards
        sms.GenerateReportCard(1);
        sms.GenerateReportCard(2);
    }
}
