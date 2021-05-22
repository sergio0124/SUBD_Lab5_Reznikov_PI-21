using ForumForumBusinessLogic.BindingModels;
using ForumForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumForumBusinessLogic.Interfaces
{
    public interface IPersonStorage
    {
        List<PersonViewModel> GetFullList();
        List<PersonViewModel> GetFilteredList(PersonBindingModel model);
        PersonViewModel GetElement(PersonBindingModel model);
        void Insert(PersonBindingModel model);
        void Update(PersonBindingModel model);
        void Delete(PersonBindingModel model);
    }
}
