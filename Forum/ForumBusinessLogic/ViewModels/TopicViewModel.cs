using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ForumForumBusinessLogic.ViewModels
{
    public class TopicViewModel
    {
        public int Id { set; get; }
        [DisplayName("Название темы")]
        public string Name { set; get; }
        [DisplayName("Название надтемы")]
        public string TopicName { set; get; }
        [DisplayName("Название объекта")]
        public string ObjectName { set; get; }
        public int? TopicId { set; get; }
        public int? ObjectId { set; get; }
        public Dictionary<int, string> Threads { set; get; }
    }
}
