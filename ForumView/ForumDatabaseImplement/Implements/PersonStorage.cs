using ForumBusinessLogic.Interfaces;
using ForumBusinessLogic.BindingModels;
using ForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ForumDatabaseImplement.Implements
{
    public class PersonStorage: IPersonStorage
    {
		public List<PersonViewModel> GetFullList()
		{
			using (var context = new ForumDatabase())
			{
				return context.Persons
					.Include(rec => rec.Threads)
					.ThenInclude(rec=>rec.Messages)
					.Select(rec => new PersonViewModel
					{
						Id = rec.Id,
						Name = rec.Name,
						Status=rec.Status,
						RegistrationDate=rec.RegistrationDate,
						Threads = rec.Threads
							.ToDictionary(recT => (int)recT.Id,
							recT => recT.Name),
						Messages = rec.Messages
							.ToDictionary(recM => (int)recM.Id,
							recM => recM.Text),
					})
					.ToList();
			}
		}
		public List<PersonViewModel> GetFilteredList(PersonBindingModel model)
		{
			if (model == null)
			{
				return null;
			}

			using (var context = new ForumDatabase())
			{
				return context.Persons
					.Include(rec => rec.Threads)
					.ThenInclude(rec => rec.Messages)
					.Where(rec => rec.Name.Contains(model.Name))
					.Select(rec => new PersonViewModel
					{
						Id = rec.Id,
						Name = rec.Name,
						Status = rec.Status,
						RegistrationDate = rec.RegistrationDate,
						Threads = rec.Threads
							.ToDictionary(recT => (int)recT.Id,
							recT => recT.Name),
						Messages = rec.Messages
							.ToDictionary(recM => (int)recM.Id,
							recM => recM.Text),
					})
					.ToList();
			}
		}

		public PersonViewModel GetElement(PersonBindingModel model)
		{
			if (model == null)
			{
				return null;
			}

			using (var context = new ForumDatabase())
			{
				var person = context.Persons
					.Include(rec => rec.Threads)
					.ThenInclude(rec => rec.Messages)
					.FirstOrDefault(rec => rec.Name.Contains(model.Name) ||
					rec.Id == model.Id);

				return person != null ?
					new PersonViewModel
					{
						Id = person.Id,
						Name = person.Name,
						Status = person.Status,
						RegistrationDate = person.RegistrationDate,
						Threads = person.Threads
							.ToDictionary(recT => (int)recT.Id,
							recT => recT.Name),
						Messages = person.Messages
							.ToDictionary(recM => (int)recM.Id,
							recM => recM.Text),
					} :
					null;
			}
		}

		public void Insert(PersonBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						CreateModel(model, new Models.Person(), context);
						context.SaveChanges();
						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}
		public void Update(PersonBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						var obj = context.Persons.FirstOrDefault(rec => rec.Id == model.Id);
						if (obj == null)
						{
							throw new Exception("Пользователь не найден");
						}
						CreateModel(model, obj, context);
						context.SaveChanges();
						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}
		public void Delete(PersonBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				var blank = context.Persons.FirstOrDefault(rec => rec.Id == model.Id);
				if (blank == null)
				{
					throw new Exception("Материал не найден");
				}
				context.Persons.Remove(blank);
				context.SaveChanges();
			}
		}
		private Models.Person CreateModel(PersonBindingModel model, Models.Person person, ForumDatabase context)
		{
			person.Name = model.Name;
			person.RegistrationDate = (DateTime)model.RegistrationDate;
			person.Status = model.Status;
			if (person.Id == 0)
			{
				context.Persons.Add(person);
				context.SaveChanges();
			}

			if (model.Id.HasValue)
			{
				context.Remove(context.Persons.Where(rec => rec.Id == person.Id));
				context.Add(person);
				context.SaveChanges();
			}
			return person;
		}
	}
}
