using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ForumDatabaseImplement.Models
{
    public class Message
    {
        public int Id { set; get; }
        [Required]
        public DateTime DateCreate { set; get; }
        [Required]
        public string Text { set; get; }
        [Required]
        public string PersonName { set; get; }
        [Required]
        public string ThreadName { set; get; }
        public string MessageText { set; get; }
        [Required]
        public int PersonId { set; get; }
        [Required]
        public int ThreadId { set; get; }
        public int? MessageId { set; get; }
        [ForeignKey("MessageId")]
        public virtual List<Message> Messages { set; get; }
    }
}
