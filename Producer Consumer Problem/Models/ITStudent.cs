namespace Producer_Consumer_Problem.Models
{
    public class ITStudent
    {
        public string StudentName { get; set; }
        public int StudentID { get; set; }
        public string Programme { get; set; }
        public List<Course> Courses { get; set; }

        public double CalculateAverageMark()
        {
            // Calculate the average mark based on the marks allocated to the courses
            double totalMarks = Courses.Sum(course => course.Mark);
            return totalMarks / Courses.Count;
        }

        public bool IsPass()
        {
            // Determine whether the student passed or failed (pass mark is 50%)
            return CalculateAverageMark() >= 50;
        }
    }
}
