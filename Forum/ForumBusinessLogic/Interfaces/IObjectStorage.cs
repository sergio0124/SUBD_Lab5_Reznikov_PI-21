using ForumForumBusinessLogic.BindingModels;
using ForumForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumForumBusinessLogic.Interfaces
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
