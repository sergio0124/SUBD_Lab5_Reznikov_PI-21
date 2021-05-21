using ForumBusinessLogic.BusinessLogics;
using ForumBusinessLogic.Interfaces;
using ForumDatabaseImplement.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace ForumView
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<IObjectStorage, ObjectStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<ITopicStorage, TopicStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IThreadStorage, ThreadStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPersonStorage, PersonStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMessageStorage, MessageStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<ObjectLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<TopicLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ThreadLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<MessageLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<PersonLogic>(new
           HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
