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
				List<PersonViewModel> result = new List<PersonViewModel>();
				foreach (var rec in context.Persons.Include(rec => rec.Messages).Include(rec => rec.Threads))
				{
					PersonViewModel mod = new PersonViewModel { };
					mod.Id = rec.Id;
					mod.Name = rec.Name;
					mod.Status = rec.Status;
					mod.RegistrationDate = rec.RegistrationDate;
					mod.Threads = rec.Threads?.ToDictionary(recOb => (int)recOb.Id,
						recOb => recOb.Name);
					mod.Messages = rec.Messages?
						.ToDictionary(recT => (int)recT.Id,
						recT => recT.Text);
					result.Add(mod);
				}
				return result;
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
				List<PersonViewModel> result = new List<PersonViewModel>();
				foreach (var rec in context.Persons
					.Include(rec => rec.Threads)
					.ThenInclude(rec => rec.Messages)
					.Where(rec => rec.Name.Contains(model.Name)))
				{
					PersonViewModel mod = new PersonViewModel { };
					mod.Id = rec.Id;
					mod.Name = rec.Name;
					mod.Status = rec.Status;
					mod.RegistrationDate = rec.RegistrationDate;
					mod.Threads = rec.Threads?.ToDictionary(recOb => (int)recOb.Id,
						recOb => recOb.Name);
					mod.Messages = rec.Messages?
						.ToDictionary(recT => (int)recT.Id,
						recT => recT.Text);
					result.Add(mod);
				}
				return result;
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
					.FirstOrDefault(rec => rec.Id == model.Id || rec.Name.Contains(model.Name));

				return person != null ?
					new PersonViewModel
					{
						Id = person.Id,
						Name = person.Name,
						Status = person.Status,
						RegistrationDate = person.RegistrationDate,
						Threads = person.Threads?
							.ToDictionary(recT => (int)recT.Id,
							recT => recT.Name),
						Messages = person.Messages?
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
			if (!model.Id.HasValue)
			{
				context.Persons.Add(person);
				context.SaveChanges();
			}
			return person;
		}
	}
}
