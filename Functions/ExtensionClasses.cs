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

}
