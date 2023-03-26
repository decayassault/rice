﻿using System.ComponentModel.DataAnnotations;
namespace Models
{
    public partial class Thread
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int EndpointId { get; set; }
    }
}
