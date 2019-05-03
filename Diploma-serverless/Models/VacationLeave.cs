using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Diploma_serverless.Models
{
    public class VacationLeave
    {
        [Required]
        public Guid Id { get; set;  }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public Guid UserId { get; set; }
    }
}