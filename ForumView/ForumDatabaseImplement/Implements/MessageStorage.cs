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
    public class MessageStorage: IMessageStorage
    {
		public List<MessageViewModel> GetFullList()
		{
			using (var context = new ForumDatabase())
			{
				return context.Messages
					.Include(rec=>rec.Messages)
					.Select(rec => new MessageViewModel
					{
						Id = rec.Id,
						Text=rec.Text,
						DateCreate=rec.DateCreate,
						PersonId=rec.PersonId,
						ThreadId=rec.ThreadId,
						PersonName=rec.PersonName,
						ThreadName=rec.ThreadName,
						MessageText=rec.MessageText,
						MessageId=rec.MessageId,
						Messages = rec.Messages
							.ToDictionary(recOb => (int)recOb.Id,
							recOb => recOb.Text)
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
					.Include(rec => rec.Messages)
					.Where(rec=>rec.Text.Contains(model.Text))
					.Select(rec => new MessageViewModel
					{
						Id = rec.Id,
						Text = rec.Text,
						DateCreate = rec.DateCreate,
						PersonId = rec.PersonId,
						ThreadId = rec.ThreadId,
						PersonName = rec.PersonName,
						ThreadName = rec.ThreadName,
						MessageText = rec.MessageText,
						MessageId = rec.MessageId,
						Messages = rec.Messages
							.ToDictionary(recOb => (int)recOb.Id,
							recOb => recOb.Text)
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
					.Include(rec => rec.Messages)
					.FirstOrDefault(rec => rec.Text.Contains(model.Text) ||
					rec.Id == model.Id);

				return message != null ?
					new MessageViewModel
					{
						Id = message.Id,
						Text = message.Text,
						DateCreate = message.DateCreate,
						PersonId = message.PersonId,
						ThreadId = message.ThreadId,
						PersonName = message.PersonName,
						ThreadName = message.ThreadName,
						MessageText = message.MessageText,
						MessageId = message.MessageId,
						Messages = message.Messages
							.ToDictionary(recOb => (int)recOb.Id,
							recOb => recOb.Text)
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
			message.ThreadId = (int)model.ThreadId;
			message.PersonId = (int)model.PersonId;
			message.MessageId = (int)model.MessageId;
			if (message.Id == 0)
			{
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
