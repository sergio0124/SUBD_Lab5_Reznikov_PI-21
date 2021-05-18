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
    public class ThreadStorage: IThreadStorage
    {
		public List<ThreadViewModel> GetFullList()
		{
			using (var context = new ForumDatabase())
			{
				return context.Threads
					.Include(rec => rec.Messages)
					.Select(rec => new ThreadViewModel
					{
						Id = rec.Id,
						Name = rec.Name,
						Description=rec.Description,
						PersonId = (int)rec.PersonId,
						PersonName = rec.PersonName,
						TopicId = rec.TopicId,
						TopicName=rec.TopicName,
						Messages = rec.Messages
							.ToDictionary(recOb => (int)recOb.Id,
							recOb => recOb.Text),
					})
					.ToList();
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
				return context.Threads
					.Include(rec => rec.Messages)
					.Where(rec => rec.Name.Contains(model.Name))
					.Select(rec => new ThreadViewModel
					{
						Id = rec.Id,
						Name = rec.Name,
						Description = rec.Description,
						PersonId = (int)rec.PersonId,
						PersonName = rec.PersonName,
						TopicId = rec.TopicId,
						TopicName = rec.TopicName,
						Messages = rec.Messages
							.ToDictionary(recOb => (int)recOb.Id,
							recOb => recOb.Text),
					})
					.ToList();
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

				return thread != null ?
					new ThreadViewModel
					{
						Id = thread.Id,
						Name = thread.Name,
						Description = thread.Description,
						PersonId = (int)thread.PersonId,
						PersonName = thread.PersonName,
						TopicId = thread.TopicId,
						TopicName = thread.TopicName,
						Messages = thread.Messages
							.ToDictionary(recOb => (int)recOb.Id,
							recOb => recOb.Text),
					} :
					null;
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
			thread.PersonId = (int)model.PersonId;
			thread.TopicId = model.TopicId;
			if (thread.Id == 0)
			{
				context.Threads.Add(thread);
				context.SaveChanges();
			}

			if (model.Id.HasValue)
			{
				context.Remove(context.Threads.Where(rec => rec.Id == thread.Id));
				context.Add(thread);
				context.SaveChanges();
			}
			return thread;
		}
	}
}
