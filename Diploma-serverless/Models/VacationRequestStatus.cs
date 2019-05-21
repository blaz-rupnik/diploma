﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Diploma_serverless.Models
{
    public class VacationRequestStatus
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}