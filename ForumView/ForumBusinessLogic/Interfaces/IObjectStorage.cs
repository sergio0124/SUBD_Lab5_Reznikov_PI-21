using ForumBusinessLogic.BindingModels;
using ForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumBusinessLogic.Interfaces
{
    public interface IObjectStorage
    {
        List<ObjectViewModel> GetFullList();
        List<ObjectViewModel> GetFilteredList(ObjectBindingModel model);
        ObjectViewModel GetElement(ObjectBindingModel model);
        void Insert(ObjectBindingModel model);
        void Update(ObjectBindingModel model);
        void Delete(ObjectBindingModel model);
    }
}
