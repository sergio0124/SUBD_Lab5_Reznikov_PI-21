using ForumBusinessLogic.BindingModels;
using ForumBusinessLogic.Interfaces;
using ForumBusinessLogic.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForumDatabaseImplement.Implements
{
    public class ObjectStorage : IObjectStorage
    {
        public List<ObjectViewModel> GetFullList()
        {
            using (var context = new ForumDatabase())
            {
                List<ObjectViewModel> result = new List<ObjectViewModel>();
                foreach (var rec in context.Objects.Include(rec=>rec.Topics))
                {
                    ObjectViewModel model = new ObjectViewModel { };
                    model.Id = rec.Id;
                    model.Name = rec.Name;
                    model.ObjectName = rec.ObjectName;
                    model.Description = rec.Description;
                    model.ObjectId = rec.UpperObjectId;
                    model.Topics = rec.Topics?
                        .ToDictionary(recT => (int)recT.Id,
                        recT => recT.Name);
                    result.Add(model);
                }
                return result;
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
                List<ObjectViewModel> result = new List<ObjectViewModel>();
                foreach (var rec in context.Objects.Include(rec => rec.Topics))
                {
                    ObjectViewModel mod = new ObjectViewModel { };
                    mod.Id = rec.Id;
                    mod.Name = rec.Name;
                    mod.ObjectName = rec.ObjectName;
                    mod.Description = rec.Description;
                    mod.ObjectId = rec.UpperObjectId;
                    mod.Topics = rec.Topics?
                        .ToDictionary(recT => (int)recT.Id,
                        recT => recT.Name);
                    result.Add(mod);
                }
                return result;
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
                        ObjectId = obj.UpperObjectId,
                        Topics = obj.Topics
                            .ToDictionary(recT => (int)recT.Id,
                            recT => recT.Name),
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
            obj.UpperObjectId = (int)model.ObjectId;
            Models.Object o = context.Objects.FirstOrDefault(rec => rec.Id == model.ObjectId);
            if (o != null) {
                obj.ObjectName = o.Name;
            }           
            if (obj.Id == 0)
            {
                context.Objects.Add(obj);
                return obj;
            }
            return obj;
        }
    }
}
