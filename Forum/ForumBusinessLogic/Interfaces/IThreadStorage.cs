using ForumForumBusinessLogic.BindingModels;
using ForumForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumForumBusinessLogic.Interfaces
{
    public interface IThreadStorage
    {
        List<ThreadViewModel> GetFullList();
        List<ThreadViewModel> GetFilteredList(ThreadBindingModel model);
        ThreadViewModel GetElement(ThreadBindingModel model);
        void Insert(ThreadBindingModel model);
        void Update(ThreadBindingModel model);
        void Delete(ThreadBindingModel model);
    }
}
