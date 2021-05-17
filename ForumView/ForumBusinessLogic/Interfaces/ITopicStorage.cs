using ForumBusinessLogic.BindingModels;
using ForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumBusinessLogic.Interfaces
{
    public interface ITopicStorage
    {
        List<TopicViewModel> GetFullList();
        List<TopicViewModel> GetFilteredList(TopicBindingModel model);
        TopicViewModel GetElement(TopicBindingModel model);
        void Insert(TopicBindingModel model);
        void Update(TopicBindingModel model);
        void Delete(TopicBindingModel model);
    }
}
