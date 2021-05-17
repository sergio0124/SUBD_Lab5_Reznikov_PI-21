using System;
using System.Collections.Generic;
using System.Text;

namespace ForumBusinessLogic.BindingModels
{
    public class PersonBindingModel
    {
        public int? Id { set; get; }
        public string Name { set; get; }
        public DateTime? RegistrationDate { set; get; }
        public string Status { set; get; }
    }
}
