using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ForumForumDatabaseImplement.Models
{
    public class Topic
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        public int? UpperTopicId { set; get; }
        public int? ObjectId { set; get; }
        [ForeignKey("TopicId")]
        public virtual List<Thread> Threads { set; get; }
    }
}
