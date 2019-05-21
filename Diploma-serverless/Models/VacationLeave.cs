using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("Status")]
        public Guid StatusId { get; set; }

        public virtual User User { get; set; }

        public virtual VacationRequestStatus Status { get; set; }
    }
}