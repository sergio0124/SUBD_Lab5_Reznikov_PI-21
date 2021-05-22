using ForumForumBusinessLogic.BindingModels;
using ForumForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumForumBusinessLogic.Interfaces
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
