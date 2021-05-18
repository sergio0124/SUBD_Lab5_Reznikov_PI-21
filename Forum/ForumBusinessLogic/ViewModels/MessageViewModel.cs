using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ForumBusinessLogic.ViewModels
{
    public class MessageViewModel
    {
        public int Id { set; get; }
        [DisplayName("Дата создания")]
        public DateTime DateCreate { set; get; }
        [DisplayName("Текст сообщения")]
        public string Text { set; get; }
        [DisplayName("Логин пользователя")]
        public string PersonName { set; get; }
        [DisplayName("Название обсуждения")]
        public string ThreadName { set; get; }
        [DisplayName("Текст над-Сообщения")]
        public string MessageText { set; get; }
        public int PersonId { set; get; }
        public int ThreadId { set; get; }
        public int? MessageId { set; get; }
        public Dictionary<int, string> Messages { set; get; }
    }
}
