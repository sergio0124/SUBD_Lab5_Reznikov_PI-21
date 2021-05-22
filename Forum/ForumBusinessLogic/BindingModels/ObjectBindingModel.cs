using System;
using System.Collections.Generic;
using System.Text;

namespace ForumForumBusinessLogic.BindingModels
{
    public class ObjectBindingModel
    {
        public int? Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public int? ObjectId { set; get; }
    }
}
