using ForumBusinessLogic.BindingModels;
using ForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumBusinessLogic.Interfaces
{
    public interface IMessageStorage
    {
        List<MessageViewModel> GetFullList();
        List<MessageViewModel> GetFilteredList(MessageBindingModel model);
        MessageViewModel GetElement(MessageBindingModel model);
        void Insert(MessageBindingModel model);
        void Update(MessageBindingModel model);
        void Delete(MessageBindingModel model);
    }
}
