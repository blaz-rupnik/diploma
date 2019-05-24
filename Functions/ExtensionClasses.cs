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

}
