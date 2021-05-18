using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ForumBusinessLogic.BindingModels
{
    public class ThreadBindingModel
    {
        public int? Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public int? PersonId { set; get; }
        public int? TopicId { set; get; }
    }
}
