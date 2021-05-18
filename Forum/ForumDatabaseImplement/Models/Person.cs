using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ForumDatabaseImplement.Models
{
    public class Person
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public DateTime RegistrationDate { set; get; }
        [Required]
        public string Status { set; get; }
        [ForeignKey("PersonId")]
        public virtual List<Thread> Threads { get; set; }
        [ForeignKey("PersonId")]
        public virtual List<Message> Messages { get; set; }
    }
}
