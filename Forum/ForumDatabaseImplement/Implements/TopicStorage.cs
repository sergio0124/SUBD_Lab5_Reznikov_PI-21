using ForumBusinessLogic.Interfaces;
using ForumBusinessLogic.ViewModels;
using ForumBusinessLogic.BindingModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ForumDatabaseImplement.Implements
{
    public class TopicStorage: ITopicStorage
    {
		public List<TopicViewModel> GetFullList()
		{
			using (var context = new ForumDatabase())
			{
				return context.Topics
					.Include(rec => rec.Topics)
					.ThenInclude(rec=>rec.Threads)
					.Select(rec => new TopicViewModel
					{
						Id = rec.Id,
						Name = rec.Name,
						TopicName = rec.TopicName,
						TopicId = rec.TopicId,
						ObjectId=rec.ObjectId,
						ObjectName=rec.ObjectName,
						Topics = rec.Topics
							.ToDictionary(recTop => (int)recTop.Id,
							recTop => recTop.Name),
						Threads = rec.Threads
							.ToDictionary(recT => (int)recT.Id,
							recT => recT.Name),
					})
					.ToList();
			}
		}
		public List<TopicViewModel> GetFilteredList(TopicBindingModel model)
		{
			if (model == null)
			{
				return null;
			}

			using (var context = new ForumDatabase())
			{
				return context.Topics
					.Include(rec => rec.Topics)
					.ThenInclude(rec => rec.Threads)
					.Where(rec=>rec.Name.Contains(model.Name))
					.Select(rec => new TopicViewModel
					{
						Id = rec.Id,
						Name = rec.Name,
						TopicName = rec.TopicName,
						TopicId = rec.TopicId,
						ObjectId = rec.ObjectId,
						ObjectName = rec.ObjectName,
						Topics = rec.Topics
							.ToDictionary(recTop => (int)recTop.TopicId,
							recTop => recTop.TopicName),
						Threads = rec.Threads
							.ToDictionary(recT => (int)recT.TopicId,
							recT => recT.TopicName),
					})
					.ToList();
			}
		}

		public TopicViewModel GetElement(TopicBindingModel model)
		{
			if (model == null)
			{
				return null;
			}

			using (var context = new ForumDatabase())
			{
				var topic = context.Topics
					.Include(rec => rec.Topics)
					.ThenInclude(rec=>rec.Threads)
					.FirstOrDefault(rec => rec.Name.Contains(model.Name) ||
					rec.Id == model.Id);

				return topic != null ?
					new TopicViewModel
					{
						Id = topic.Id,
						Name = topic.Name,
						TopicName = topic.TopicName,
						TopicId = topic.TopicId,
						ObjectId = topic.ObjectId,
						ObjectName = topic.ObjectName,
						Topics = topic.Topics
							.ToDictionary(recTop => (int)recTop.TopicId,
							recTop => recTop.TopicName),
						Threads = topic.Threads
							.ToDictionary(recT => (int)recT.TopicId,
							recT => recT.TopicName),
					} :
					null;
			}
		}

		public void Insert(TopicBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						CreateModel(model, new Models.Topic(), context);
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
		public void Update(TopicBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						var topic = context.Topics.FirstOrDefault(rec => rec.Id == model.Id);
						if (topic == null)
						{
							throw new Exception("Объект не найден");
						}
						CreateModel(model, topic, context);
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
		public void Delete(TopicBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				var blank = context.Topics.FirstOrDefault(rec => rec.Id == model.Id);
				if (blank == null)
				{
					throw new Exception("Материал не найден");
				}
				context.Topics.Remove(blank);
				context.SaveChanges();
			}
		}
		private Models.Topic CreateModel(TopicBindingModel model, Models.Topic topic, ForumDatabase context)
		{
			topic.Name = model.Name;
			topic.ObjectId = model.ObjectId;
			topic.TopicId = model.TopicId;
			if (topic.Id == 0)
			{
				context.Topics.Add(topic);
				context.SaveChanges();
			}

			if (model.Id.HasValue)
			{
				context.Remove(context.Topics.Where(rec => rec.Id == topic.Id));
				context.Add(topic);
				context.SaveChanges();
			}
			return topic;
		}
	}
}
