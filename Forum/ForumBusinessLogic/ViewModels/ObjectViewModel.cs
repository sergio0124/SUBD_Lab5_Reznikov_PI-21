using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ForumBusinessLogic.ViewModels
{
    public class ObjectViewModel
    {
        public int Id { set; get; }
        [DisplayName("Название объекта")]
        public string Name { set; get; }
        [DisplayName("Описание объекта")]
        public string Description { set; get; }
        [DisplayName("Название над-Объекта")]
        public string ObjectName { set; get; }
        public int? ObjectId { set; get; }
        public Dictionary<int, string> Topics { set; get; }
    }
}
