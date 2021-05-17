using System;
using System.Collections.Generic;
using System.Text;

namespace ForumBusinessLogic.BindingModels
{
    public class MessageBindingModel
    {
        public int? Id { set; get; }
        public DateTime DateCreate { set; get; }
        public string Text { set; get; }
        public int? PersonId { set; get; }
        public int? ThreadId { set; get; }
        public int? MessageId { set; get; }
    }
}
