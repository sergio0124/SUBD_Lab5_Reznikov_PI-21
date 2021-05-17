using ForumBusinessLogic.BindingModels;
using ForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumBusinessLogic.Interfaces
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
