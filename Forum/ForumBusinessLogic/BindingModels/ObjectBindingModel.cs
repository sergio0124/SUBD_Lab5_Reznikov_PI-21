using System;
using System.Collections.Generic;
using System.Text;

namespace ForumBusinessLogic.BindingModels
{
    public class ObjectBindingModel
    {
        public int? Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public int? ObjectId { set; get; }
    }
}
