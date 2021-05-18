using ForumBusinessLogic.BindingModels;
using ForumBusinessLogic.Interfaces;
using ForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumBusinessLogic.BusinessLogics
{
    public class PersonLogic
    {
        private readonly IPersonStorage _personStorage;
        public PersonLogic(IPersonStorage personStorage)
        {
            _personStorage = personStorage;
        }

        public List<PersonViewModel> Read(PersonBindingModel model)
        {
            if (model == null)
            {
                return _personStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<PersonViewModel> { _personStorage.GetElement(model) };
            }
            return _personStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(PersonBindingModel model)
        {
            var element = _personStorage.GetElement(new PersonBindingModel
            {
                Name = model.Name
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть пользователь с таким названием");
            }
            if (model.Id.HasValue)
            {
                _personStorage.Update(model);
            }
            else
            {
                _personStorage.Insert(model);
            }
        }
        public void Delete(PersonBindingModel model)

        {
            var element = _personStorage.GetElement(new PersonBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _personStorage.Delete(model);
        }
    }
}
