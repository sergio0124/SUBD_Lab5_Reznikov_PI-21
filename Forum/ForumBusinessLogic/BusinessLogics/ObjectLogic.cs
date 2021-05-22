using ForumForumBusinessLogic.BindingModels;
using ForumForumBusinessLogic.Interfaces;
using ForumForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumForumBusinessLogic.BusinessLogics
{
    public class ObjectLogic
    {
        private readonly IObjectStorage _objectStorage;
        public ObjectLogic(IObjectStorage objectStorage)
        {
            _objectStorage = objectStorage;
        }

        public List<ObjectViewModel> Read(ObjectBindingModel model)
        {
            if (model == null)
            {
                return _objectStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<ObjectViewModel> { _objectStorage.GetElement(model) };
            }
            return _objectStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(ObjectBindingModel model)
        {
            var element = _objectStorage.GetElement(new ObjectBindingModel
            {
                Name = model.Name
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть объект с таким названием");
            }
            if (model.Id.HasValue)
            {
                _objectStorage.Update(model);
            }
            else
            {
                _objectStorage.Insert(model);
            }
        }
        public void Delete(ObjectBindingModel model)

        {
            var element = _objectStorage.GetElement(new ObjectBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _objectStorage.Delete(model);
        }
    }
}
