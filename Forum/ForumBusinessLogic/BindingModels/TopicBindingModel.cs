using System;
using System.Collections.Generic;
using System.Text;

namespace ForumBusinessLogic.BindingModels
{
    public class TopicBindingModel
    {
        public int? Id { set; get; }
        public string Name { set; get; }
        public int? TopicId { set; get; }
        public int? ObjectId { set; get; }
    }
}
