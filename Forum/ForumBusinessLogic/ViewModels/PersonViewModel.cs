using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ForumForumBusinessLogic.ViewModels
{
    public class PersonViewModel
    {
        public int Id { set; get; }
        [DisplayName("Логин")]
        public string Name { set; get; }
        [DisplayName("Дата регистрации")]
        public DateTime RegistrationDate { set; get; }
        [DisplayName("Статус")]
        public string Status { set; get; }
        public Dictionary<int, string> Threads { get; set; }
        public Dictionary<int, string> Messages { get; set; }
    }
}
