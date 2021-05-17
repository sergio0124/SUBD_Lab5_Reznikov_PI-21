using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ForumDatabaseImplement.Models
{
    public class Topic
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        public string TopicName { set; get; }
        public string ObjectName { set; get; }
        public int? TopicId { set; get; }
        public int? ObjectId { set; get; }
        [ForeignKey("TopicId")]
        public virtual List<Thread> Threads { set; get; }
        [ForeignKey("TopicId")]
        public virtual List<Topic> Topics { set; get; }
    }
}
