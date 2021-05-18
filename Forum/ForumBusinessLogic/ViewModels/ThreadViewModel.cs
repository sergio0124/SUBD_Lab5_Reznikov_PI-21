using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ForumBusinessLogic.ViewModels
{
    public class ThreadViewModel
    {
        public int Id { set; get; }
        [DisplayName("Название обсуждения")]
        public string Name { set; get; }
        [DisplayName("Описание обсуждения")]
        public string Description { set; get; }
        [DisplayName("Название темы")]
        public string TopicName { set; get; }
        [DisplayName("Логин пользователя")]
        public string PersonName { set; get; }
        public int PersonId { set; get; }
        public int? TopicId { set; get; }
        public Dictionary<int, string> Messages { set; get; }
    }
}
