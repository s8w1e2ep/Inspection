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
            //��Ʈw�s���r��Ѱ������Ҩӵ����A�Ӥ��g���b DbContextFactory ��.
            string connectionString = WebConfigurationManager.ConnectionStrings["inspectionEntities"].ConnectionString;

            container.RegisterType<IDbContextFactory, DbContextFactory>(
                new HierarchicalLifetimeManager(),
                new InjectionConstructor(connectionString));

            //Repiository
            container.RegisterType<IRepository<abnormalDefinition>, GenericRepository<abnormalDefinition>>();
            container.RegisterType<IRepository<abnormalRecord>, GenericRepository<abnormalRecord>>();
            container.RegisterType<IRepository<exhibitionItem>, GenericRepository<exhibitionItem>>();
            container.RegisterType<IRepository<itemInspectionDispatch>, GenericRepository<itemInspectionDispatch>>();
            container.RegisterType<IRepository<roomInspectionDispatch>, GenericRepository<roomInspectionDispatch>>();
            container.RegisterType<IRepository<itemInspectionDispatchDetail>, GenericRepository<itemInspectionDispatchDetail>>();
            container.RegisterType<IRepository<roomInspectionDispatchDetail>, GenericRepository<roomInspectionDispatchDetail>>();
            container.RegisterType<IRepository<noCheckDate>, GenericRepository<noCheckDate>>();
            container.RegisterType<IRepository<exhibitionItem>, GenericRepository<exhibitionItem>>();
            container.RegisterType<IRepository<exhibitionRoom>, GenericRepository<exhibitionRoom>>();
            container.RegisterType<IRepository<reportDevice>, GenericRepository<reportDevice>>();
            container.RegisterType<IRepository<reportSource>, GenericRepository<reportSource>>();
            container.RegisterType<IRepository<roomCheckRecord>, GenericRepository<roomCheckRecord>>();
            container.RegisterType<IRepository<itemCheckRecord>, GenericRepository<itemCheckRecord>>();
            container.RegisterType<IRepository<reportSource>, GenericRepository<reportSource>>();
            container.RegisterType<IRepository<user>, GenericRepository<user>>();
            container.RegisterType<IRepository<userGroup>, GenericRepository<userGroup>>();
            container.RegisterType<IRepository<fieldMap>, GenericRepository<fieldMap>>();
            container.RegisterType<IRepository<quickSolution>, GenericRepository<quickSolution>>();
            container.RegisterType<IRepository<exhibitionRoom>, GenericRepository<exhibitionRoom>>();
            container.RegisterType<IRepository<otherAbnormalRecord>, GenericRepository<otherAbnormalRecord>>();
            container.RegisterType<IRepository<softwareVersion>, GenericRepository<softwareVersion>>();
            container.RegisterType<IRepository<systemSettings>, GenericRepository<systemSettings>>();
            container.RegisterType<IRepository<reportDevice>, GenericRepository<reportDevice>>();


            //Service
            container.RegisterType<IAbnormalDefinitionService, AbnormalDefinitionService>();
            container.RegisterType<IAbnormalRecordService, AbnormalRecordService>();
            container.RegisterType<IExhibitionItemService, ExhibitionItemService>();
            container.RegisterType<IExhibitionRoomService, ExhibitionRoomService>();
            container.RegisterType<IReportDeviceService, ReportDeviceService>();
            container.RegisterType<IReportSourceService, ReportSourceService>();
            container.RegisterType<INoCheckDateService, NoCheckDateService>();
            container.RegisterType<IItemInspectionDispatchService, ItemInspectionDispatchService>();
            container.RegisterType<IRoomInspectionDispatchService, RoomInspectionDispatchService>();
            container.RegisterType<IRoomCheckRecordService, RoomCheckRecordService>();
            container.RegisterType<IItemCheckRecordService, ItemCheckRecordService>();
            container.RegisterType<INoCheckDateService, NoCheckDateService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IUserGroupService, UserGroupService>();
            container.RegisterType<IFieldMapService, FieldMapService>();
            container.RegisterType<ISolutionService, SolutionService>();
            container.RegisterType<IExhibitionRoomService, ExhibitionRoomService>();
            container.RegisterType<IOtherAbnormalRecordService, OtherAbnormalRecordService>();
            container.RegisterType<IMaintenanceWorkService, MaintenanceWorkService>();
            container.RegisterType<ISoftwareVersionService, SoftwareVersionService>();
            container.RegisterType<ISystemArgService, SystemArgService>();
            container.RegisterType<IReportDeviceService, ReportDeviceService>();

        }
    }
}
