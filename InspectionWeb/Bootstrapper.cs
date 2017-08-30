using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using System.Web.Configuration;
using InspectionWeb.Models.DbContextFactory;
using InspectionWeb.Models;
using InspectionWeb.Models.Interface;
using InspectionWeb.Models.Repository;
using System.Web.Http;
using InspectionWeb.Services;
using InspectionWeb.Services.Interface;

namespace InspectionWeb
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            // nuget package unity.webapi
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    
            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            //��Ʈw�s���r��Ѱ�������Ҩӵ����A�Ӥ��g���b DbContextFactory ��.
            string connectionString = WebConfigurationManager.ConnectionStrings["inspectionEntities"].ConnectionString;

            container.RegisterType<IDbContextFactory, DbContextFactory>(
                new HierarchicalLifetimeManager(),
                new InjectionConstructor(connectionString));

            //Repiository
            container.RegisterType<IRepository<abnormalDefinition>, GenericRepository<abnormalDefinition>>();
            container.RegisterType<IRepository<abnormalRecord>, GenericRepository<abnormalRecord>>();
            container.RegisterType<IRepository<exhibitionItem>, GenericRepository<exhibitionItem>>();
            container.RegisterType<IRepository<reportDevice>, GenericRepository<reportDevice>>();
            container.RegisterType<IRepository<reportSource>, GenericRepository<reportSource>>();
            container.RegisterType<IRepository<user>, GenericRepository<user>>();

            //Service
            container.RegisterType<IAbnormalDefinitionService, AbnormalDefinitionService>();
            container.RegisterType<IAbnormalRecordService, AbnormalRecordService>();
            container.RegisterType<IExhibitionItemService, ExhibitionItemService>();
            container.RegisterType<IReportDeviceService, ReportDeviceService>();
            container.RegisterType<IReportSourceService, ReportSourceService>();
            container.RegisterType<IUserService, UserService>();
        }
    }
}