using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ForumForumDatabaseImplement.Models
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
        public int PersonId { set; get; }
        public int? ThreadId { set; get; }
        public int? UpperMessageId { set; get; }
    }
}
