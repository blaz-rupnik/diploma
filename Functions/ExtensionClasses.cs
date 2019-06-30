using System;
using System.Collections.Generic;
using System.Text;

namespace Functions
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class TaskNotification
    {
        public string TaskType { get; set; }
        public Guid EntityInstanceId { get; set; }
        public string TaskName { get; set; }
        public int TaskAction { get; set; }
    }

    public class VacationRequestStatus
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class VacationLeave
    {
        public Guid Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int? DaysPending { get; set; }
        public Guid UserId { get; set; }
        public Guid StatusId { get; set; }
        public User User { get; set; }
        public VacationRequestStatus Status { get; set; }
    }

    public class MonthlyGrade
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Grade { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }

    public class MonthlyGradeSummary
    {
        public string Month { get; set; }
        public double AverageGrade { get; set; }
    }

    public class CustomLog
    {
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public enum Months
    {
        Januar = 1,
        Februar = 2,
        Marec = 3,
        April = 4,
        Maj = 5,
        Junij = 6,
        Julij = 7,
        Avgust = 8,
        September = 9,
        Oktober = 10,
        November = 11,
        December = 12
    }

}
