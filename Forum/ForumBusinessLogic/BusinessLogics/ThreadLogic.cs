using ForumBusinessLogic.BindingModels;
using ForumBusinessLogic.Interfaces;
using ForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumBusinessLogic.BusinessLogics
{
    public class ThreadLogic
    {
        private readonly IThreadStorage _threadStorage;
        public ThreadLogic(IThreadStorage threadStorage)
        {
            _threadStorage = threadStorage;
        }

        public List<ThreadViewModel> Read(ThreadBindingModel model)
        {
            if (model == null)
            {
                return _threadStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<ThreadViewModel> { _threadStorage.GetElement(model) };
            }
            return _threadStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(ThreadBindingModel model)
        {
            var element = _threadStorage.GetElement(new ThreadBindingModel
            {
                Name = model.Name
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть обсуждение с таким названием");
            }
            if (model.Id.HasValue)
            {
                _threadStorage.Update(model);
            }
            else
            {
                _threadStorage.Insert(model);
            }
        }
        public void Delete(ThreadBindingModel model)

        {
            var element = _threadStorage.GetElement(new ThreadBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _threadStorage.Delete(model);
        }
    }
}
