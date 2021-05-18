using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ForumDatabaseImplement.Models
{
    public class Thread
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public string Description { set; get; }
        public string TopicName { set; get; }
        [Required]
        public string PersonName { set; get; }
        [Required]
        public int PersonId { set; get; }
        public int? TopicId { set; get; }
        [ForeignKey("ThreadId")]
        public virtual List<Message> Messages { set; get; }
    }
}
