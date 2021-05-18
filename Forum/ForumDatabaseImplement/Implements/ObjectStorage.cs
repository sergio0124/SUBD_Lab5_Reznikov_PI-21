using ForumBusinessLogic.BindingModels;
using ForumBusinessLogic.Interfaces;
using ForumBusinessLogic.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForumDatabaseImplement.Implements
{
    public class ObjectStorage: IObjectStorage
    {
		public List<ObjectViewModel> GetFullList()
		{
			using (var context = new ForumDatabase())
			{
				return context.Objects
					.Select(rec => new ObjectViewModel
					{
						Id = rec.Id,
						Name = rec.Name,
						ObjectName=rec.ObjectName,
						Description=rec.Description,
						ObjectId=rec.ObjectId,
						//Objects = rec.Objects
      //                      .ToDictionary(recOb => (int) recOb.Id,
						//	recOb => recOb.Name),
						//Topics = rec.Topics
						//	.ToDictionary(recT => (int)recT.Id,
						//	recT => recT.Name),
					})
					.ToList();
			}
		}
		public List<ObjectViewModel> GetFilteredList(ObjectBindingModel model)
		{
			if (model == null)
			{
				return null;
			}

			using (var context = new ForumDatabase())
			{
				return context.Objects
					.Include(rec => rec.Topics)
					.Where(rec => rec.Name.Contains(model.Name))
					.Select(rec => new ObjectViewModel
					{
						Id = rec.Id,
						Name = rec.Name,
						ObjectName = rec.ObjectName,
						Description = rec.Description,
						ObjectId = rec.ObjectId,
						Objects = rec.Objects
							.ToDictionary(recOb => (int)recOb.ObjectId,
							recOb => recOb.ObjectName),
						Topics = rec.Topics
							.ToDictionary(recT => (int)recT.TopicId,
							recT => recT.TopicName),
					})
					.ToList();
			}
		}

		public ObjectViewModel GetElement(ObjectBindingModel model)
		{
			if (model == null)
			{
				return null;
			}

			using (var context = new ForumDatabase())
			{
				var obj = context.Objects
					.Include(rec => rec.Topics)
					.FirstOrDefault(rec => rec.Name.Contains(model.Name) ||
					rec.Id == model.Id);

				return obj != null ?
					new ObjectViewModel
					{
						Id = obj.Id,
						Name = obj.Name,
						ObjectName = obj.ObjectName,
						Description = obj.Description,
						ObjectId = obj.ObjectId,
						Objects = obj.Objects
							.ToDictionary(recOb => (int)recOb.ObjectId,
							recOb => recOb.ObjectName),
						Topics = obj.Topics
							.ToDictionary(recT => (int)recT.TopicId,
							recT => recT.TopicName),
					} :
					null;
			}
		}

		public void Insert(ObjectBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						CreateModel(model, new Models.Object(), context);
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
		public void Update(ObjectBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						var obj = context.Objects.FirstOrDefault(rec => rec.Id == model.Id);
						if (obj == null)
						{
							throw new Exception("Объект не найден");
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
		public void Delete(ObjectBindingModel model)
		{
			using (var context = new ForumDatabase())
			{
				var blank = context.Objects.FirstOrDefault(rec => rec.Id == model.Id);
				if (blank == null)
				{
					throw new Exception("Материал не найден");
				}
				context.Objects.Remove(blank);
				context.SaveChanges();
			}
		}
		private Models.Object CreateModel(ObjectBindingModel model, Models.Object obj, ForumDatabase context)
		{
			obj.Name = model.Name;
			obj.Description = model.Description;
			obj.ObjectId = model.ObjectId;
			if (obj.Id==0)
			{
				context.Objects.Add(obj);
				return obj;
			}

			if (model.Id.HasValue)
			{
				context.Remove(context.Objects.Where(rec=>rec.Id==obj.Id));
				context.Add(obj);
				return obj;
			}			
			return obj;
		}
	}
}
