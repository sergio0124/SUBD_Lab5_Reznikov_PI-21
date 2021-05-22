using ForumForumBusinessLogic.BindingModels;
using ForumForumBusinessLogic.Interfaces;
using ForumForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumForumBusinessLogic.BusinessLogics
{
    public class TopicLogic
    {
        private readonly ITopicStorage _topicStorage;
        public TopicLogic(ITopicStorage topicStorage)
        {
            _topicStorage = topicStorage;
        }

        public List<TopicViewModel> Read(TopicBindingModel model)
        {
            if (model == null)
            {
                return _topicStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<TopicViewModel> { _topicStorage.GetElement(model) };
            }
            return _topicStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(TopicBindingModel model)
        {
            var element = _topicStorage.GetElement(new TopicBindingModel
            {
                Name = model.Name
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть тема с таким названием");
            }
            if (model.Id.HasValue)
            {
                _topicStorage.Update(model);
            }
            else
            {
                _topicStorage.Insert(model);
            }
        }
        public void Delete(TopicBindingModel model)

        {
            var element = _topicStorage.GetElement(new TopicBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _topicStorage.Delete(model);
        }
    }
}
