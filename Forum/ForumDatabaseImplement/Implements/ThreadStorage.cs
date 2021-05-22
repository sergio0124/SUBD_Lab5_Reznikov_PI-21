using ForumForumBusinessLogic.Interfaces;
using ForumForumBusinessLogic.BindingModels;
using ForumForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ForumForumDatabaseImplement.Implements
{
    public class ThreadStorage: IThreadStorage
    {
		public List<ThreadViewModel> GetFullList()
		{
			using (var context = new ForumDatabase())
			{
				List<ThreadViewModel> result = new List<ThreadViewModel>();
				foreach (var rec in context.Threads.Include(rec => rec.Messages))
				{
					ThreadViewModel mod = new ThreadViewModel { };
					mod.Id = rec.Id;
					mod.Name = rec.Name;
					mod.Description = rec.Description;
					mod.PersonId = (int)rec.PersonId;
					mod.PersonName = rec.PersonName;
					mod.TopicId = rec.TopicId;
					mod.TopicName = rec.TopicName;
					mod.Messages = rec.Messages?
						.ToDictionary(recT => (int)recT.Id,
						recT => recT.Text);
					result.Add(mod);
				}
				return result;
			}
		}
		public List<ThreadViewModel> GetFilteredList(ThreadBindingModel model)
		{
			if (model == null)
			{
				return null;
			}

			using (var context = new ForumDatabase())
			{
				List<ThreadViewModel> result = new List<ThreadViewModel>();
				foreach (var rec in context.Threads
					.Include(rec => rec.Messages)
					.Where(rec => rec.Name.Contains(model.Name)))
				{
					ThreadViewModel mod = new ThreadViewModel { };
					mod.Id = rec.Id;
					mod.Name = rec.Name;
					mod.Description = rec.Description;
					mod.PersonId = (int)rec.PersonId;
					mod.PersonName = rec.PersonName;
					mod.TopicId = rec.TopicId;
					mod.TopicName = rec.TopicName;
					mod.Messages = rec.Messages?
						.ToDictionary(recT => (int)recT.Id,
						recT => recT.Text);
					result.Add(mod);
				}
				return result;
			}
		}

		public ThreadViewModel GetElement(ThreadBindingModel model)
		{
			if (model == null)
			{
				return null;
			}

			using (var context = new ForumDatabase())
			{
				var thread = context.Threads
					.Include(rec => rec.Messages)
					.FirstOrDefault(rec => rec.Name.Contains(model.Name) ||
					rec.Id == model.Id);
				if (thread == null) return null;

				ThreadViewModel mod = new ThreadViewModel { };
				mod.Id = thread.Id;
				mod.Name = thread.Name;
				mod.Description = thread.Description;
				mod.PersonId = (int)thread.PersonId;
				mod.PersonName = thread.PersonName;
				mod.TopicId = thread.TopicId;
				mod.TopicName = thread.TopicName;
				mod.Messages = thread.Messages
					.ToDictionary(recOb => (int)recOb.Id,
					recOb => recOb.Text);
				return mod;
					
			}
		}

		public void Insert(ThreadBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						CreateModel(model, new Models.Thread(), context);
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
		public void Update(ThreadBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						var thread = context.Threads.FirstOrDefault(rec => rec.Id == model.Id);
						if (thread == null)
						{
							throw new Exception("Объект не найден");
						}
						CreateModel(model, thread, context);
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
		public void Delete(ThreadBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				var blank = context.Threads.FirstOrDefault(rec => rec.Id == model.Id);
				if (blank == null)
				{
					throw new Exception("Обсуждение не найдено");
				}
				context.Threads.Remove(blank);
				context.SaveChanges();
			}
		}
		private Models.Thread CreateModel(ThreadBindingModel model, Models.Thread thread, ForumDatabase context)
		{
			thread.Name = model.Name;
			thread.Description = model.Description;
			if (model.PersonId.HasValue)
			{
				thread.PersonId = (int)model.PersonId;
				thread.PersonName = context.Persons.FirstOrDefault(rec => rec.Id == model.PersonId).Name;
			}
			if (model.TopicId.HasValue)
			{
				thread.TopicId = (int)model.TopicId;
				thread.TopicName = context.Topics.FirstOrDefault(rec => rec.Id == model.TopicId)?.Name;
			}
			if (thread.Id == 0)
			{
				context.Threads.Add(thread);
				context.SaveChanges();
			}
			return thread;
		}
	}
}
