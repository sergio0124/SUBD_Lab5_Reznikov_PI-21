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
				List<TopicViewModel> result = new List<TopicViewModel>();
				foreach (var rec in context.Topics.Include(rec => rec.Threads).Include(rec=>rec.Topics))
				{
					TopicViewModel mod = new TopicViewModel { };
					mod.Id = rec.Id;
					mod.Name = rec.Name;
					mod.ObjectId = rec.ObjectId;
					mod.ObjectName = rec.ObjectName;
					mod.TopicId = rec.TopicId;
					mod.TopicName = rec.TopicName;
					mod.Topics = rec.Topics?
						.ToDictionary(recT => (int)recT.Id,
						recT => recT.Name);
					mod.Threads = rec.Threads?
						.ToDictionary(recT => (int)recT.Id,
						recT => recT.Name);
					result.Add(mod);
				}
				return result;
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
				List<TopicViewModel> result = new List<TopicViewModel>();
				foreach (var rec in context.Topics
					.Include(rec => rec.Topics)
					.ThenInclude(rec => rec.Threads)
					.Where(rec => rec.Name.Contains(model.Name)))
				{
					TopicViewModel mod = new TopicViewModel { };
					mod.Id = rec.Id;
					mod.Name = rec.Name;
					mod.ObjectId = rec.ObjectId;
					mod.ObjectName = rec.ObjectName;
					mod.TopicId = rec.TopicId;
					mod.TopicName = rec.TopicName;
					mod.Topics = rec.Topics?
						.ToDictionary(recT => (int)recT.Id,
						recT => recT.Name);
					mod.Threads = rec.Threads?
						.ToDictionary(recT => (int)recT.Id,
						recT => recT.Name);
					result.Add(mod);
				}
				return result;

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
				if (topic == null) return null;

				TopicViewModel mod = new TopicViewModel { };
				mod.Id = topic.Id;
				mod.Name = topic.Name;
				mod.ObjectId = topic.ObjectId;
				mod.ObjectName = topic.ObjectName;
				mod.TopicId = topic.TopicId;
				mod.TopicName = topic.TopicName;
				mod.Topics = topic.Topics?
					.ToDictionary(recT => (int)recT.Id,
					recT => recT.Name);
				mod.Threads = topic.Threads?
					.ToDictionary(recT => (int)recT.Id,
					recT => recT.Name);
				return mod;
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
			if (model.ObjectId!=0)
			{
				topic.ObjectId = model.ObjectId;
				topic.ObjectName = context.Objects.FirstOrDefault(rec => rec.Id == model.ObjectId).Name;
			}
			if (model.TopicId!=0)
			{
				topic.TopicId = model.TopicId;
				topic.TopicName = context.Topics.FirstOrDefault(rec => rec.Id == model.TopicId).Name;
			}
			if (topic.Id == 0)
			{
				context.Topics.Add(topic);
				context.SaveChanges();
			}
			return topic;
		}
	}
}
