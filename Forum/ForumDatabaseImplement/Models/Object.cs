using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ForumForumDatabaseImplement.Models
{
    public class Object
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public string Description { set; get; }
        public int? UpperObjectId { set; get; }
        [ForeignKey("ObjectId")]
        public virtual List<Topic> Topics { set; get; }
    }
}
