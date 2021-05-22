using ForumForumBusinessLogic.BindingModels;
using ForumForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumForumBusinessLogic.Interfaces
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
