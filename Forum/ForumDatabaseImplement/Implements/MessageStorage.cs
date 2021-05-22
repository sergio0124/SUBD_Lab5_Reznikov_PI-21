using ForumForumBusinessLogic.Interfaces;
using ForumForumBusinessLogic.BindingModels;
using ForumForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ForumForumDatabaseImplement.Models;

namespace ForumForumDatabaseImplement.Implements
{
    public class MessageStorage: IMessageStorage
    {
		public List<MessageViewModel> GetFullList()
		{
			using (var context = new ForumDatabase())
			{
				return context.Messages
					.Select(rec => new MessageViewModel
					{
						Id = rec.Id,
						Text=rec.Text,
						DateCreate=rec.DateCreate,
						PersonId= (int)rec.PersonId,
						ThreadId= (int)rec.ThreadId,
						PersonName = context.Persons.FirstOrDefault(recrec => recrec.Id == rec.PersonId).Name,
						ThreadName = context.Threads.FirstOrDefault(recrec => recrec.Id == rec.ThreadId).Name,
						MessageText = context.Messages.FirstOrDefault(recrec => recrec.Id == rec.UpperMessageId).Text,
						MessageId =rec.UpperMessageId
					})
					.ToList();
			}
		}
		public List<MessageViewModel> GetFilteredList(MessageBindingModel model)
		{
			if (model == null)
			{
				return null;
			}

			using (var context = new ForumDatabase())
			{
				return context.Messages
					.Where(rec=>rec.Text.Contains(model.Text))
					.Select(rec => new MessageViewModel
					{
						Id = rec.Id,
						Text = rec.Text,
						DateCreate = rec.DateCreate,
						PersonId = (int)rec.PersonId,
						ThreadId = (int)rec.ThreadId,
						PersonName = context.Persons.FirstOrDefault(recrec => recrec.Id == rec.PersonId).Name,
						ThreadName = context.Threads.FirstOrDefault(recrec => recrec.Id == rec.ThreadId).Name,
						MessageText = context.Messages.FirstOrDefault(recrec => recrec.Id == rec.UpperMessageId).Text,
						MessageId = rec.UpperMessageId
					})
					.ToList();
			}
		}

		public MessageViewModel GetElement(MessageBindingModel model)
		{
			if (model == null)
			{
				return null;
			}

			using (var context = new ForumDatabase())
			{
				var message = context.Messages
					.FirstOrDefault(rec => rec.Text.Contains(model.Text) ||
					rec.Id == model.Id);

				return message != null ?
					new MessageViewModel
					{
						Id = message.Id,
						Text = message.Text,
						DateCreate = message.DateCreate,
						PersonId = (int)message.PersonId,
						ThreadId = (int)message.ThreadId,
						PersonName = context.Persons.FirstOrDefault(rec=>rec.Id==message.PersonId)?.Name,
						ThreadName = context.Threads.FirstOrDefault(rec => rec.Id == message.ThreadId)?.Name,
						MessageText = context.Messages.FirstOrDefault(rec => rec.Id == message.UpperMessageId)?.Text,
						MessageId = message.UpperMessageId
					} :
					null;
			}
		}

		public void Insert(MessageBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						CreateModel(model, new Models.Message(), context);
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
		public void Update(MessageBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						var message = context.Messages.FirstOrDefault(rec => rec.Id == model.Id);
						if (message == null)
						{
							throw new Exception("Объект не найден");
						}
						CreateModel(model, message, context);
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
		public void Delete(MessageBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				var blank = context.Messages.FirstOrDefault(rec => rec.Id == model.Id);
				if (blank == null)
				{
					throw new Exception("Сообщение не найден");
				}
				context.Messages.Remove(blank);
				context.SaveChanges();
			}
		}
		private Models.Message CreateModel(MessageBindingModel model, Models.Message message, ForumDatabase context)
		{
			message.Text = model.Text;
			if (message.Id == 0)
			{
				message.ThreadId = model.ThreadId;
				message.ThreadName = context.Threads.FirstOrDefault(rec => rec.Id == model.ThreadId)?.Name;
				message.PersonId = (int)model.PersonId;
				message.PersonName = context.Persons.FirstOrDefault(rec => rec.Id == model.PersonId)?.Name;
				message.UpperMessageId = (int)model.MessageId;
				message.MessageText = context.Messages.FirstOrDefault(rec => rec.Id == model.MessageId)?.Text;
				context.Messages.Add(message);
				context.SaveChanges();
			}

			if (model.Id.HasValue)
			{
				context.Remove(context.Messages.Where(rec => rec.Id == message.Id));
				context.Add(message);
				context.SaveChanges();
			}
			return message;
		}
	}
}
