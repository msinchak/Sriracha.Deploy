﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Autofac;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Sriracha.Deploy.Data;
using Sriracha.Deploy.Data.Impl;
using Sriracha.Deploy.Data.ServiceJobs;
using Sriracha.Deploy.Data.ServiceJobs.ServiceJobImpl;
using Sriracha.Deploy.Data.Tasks;
using Sriracha.Deploy.Data.Tasks.TaskImpl;

namespace Sriracha.Deploy.AutofacModules
{
	public class SrirachaAutofacorator : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<AutofacDIFactory>().As<IDIFactory>().SingleInstance();

			if (HttpContext.Current != null)
			{
				builder.RegisterType<WebUserIdentity>().As<IUserIdentity>();
				//this.SetupLogging(builder, new WebUserIdentity());
			}
			else
			{
				builder.RegisterType<ConsoleUserIdentity>().As<IUserIdentity>();
				//this.SetupLogging(builder, new ConsoleUserIdentity());
			}

			builder.RegisterType<ProjectManager>().As<IProjectManager>();
			builder.RegisterType<BuildManager>().As<IBuildManager>();
			builder.RegisterType<FileManager>().As<IFileManager>();
			builder.RegisterType<TaskManager>().As<ITaskManager>();
			builder.RegisterType<DeployHistoryManager>().As<IDeployHistoryManager>();
			builder.RegisterType<ModuleInspector>().As<IModuleInspector>();
			builder.RegisterType<DeployRunner>().As<IDeployRunner>();
			builder.RegisterType<DeployTaskStatusManager>().As<IDeployTaskStatusManager>();
			builder.RegisterType<DeployComponentRunner>().As<IDeployComponentRunner>();
			builder.RegisterType<DeployTaskFactory>().As<IDeployTaskFactory>().SingleInstance();
			builder.RegisterType<DeployRequestManager>().As<IDeployRequestManager>();
			builder.RegisterType<DeploymentValidator>().As<IDeploymentValidator>().SingleInstance();
			builder.RegisterType<ProcessRunner>().As<IProcessRunner>().SingleInstance();
			builder.RegisterType<ParameterParser>().As<IParameterParser>().SingleInstance();
			builder.RegisterType<FileWriter>().As<IFileWriter>().SingleInstance();
			builder.RegisterType<BuildPublisher>().As<IBuildPublisher>();
			builder.RegisterType<DeployStateManager>().As<IDeployStateManager>();
			builder.RegisterType<JobScheduler>().As<IJobScheduler>();
			builder.RegisterType<Zipper>().As<IZipper>();
			builder.RegisterType<DataGenerator>().As<IDataGenerator>();
			builder.RegisterType<DefaultSystemSettings>().As<ISystemSettings>();

			builder.RegisterType<RunDeploymentJob>().As<IRunDeploymentJob>();
			builder.RegisterType<JobFactory>().As<IJobFactory>();
			builder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>();
			builder.Register(CreateScheduler).As<IScheduler>().SingleInstance();

			builder.RegisterModule(new RavenDBAutofacModule());

			this.SetupLogging(builder);
		}

		public static IScheduler CreateScheduler(IComponentContext context)
		{
			var schedulerFactory = context.Resolve<ISchedulerFactory>();
			var scheduler = schedulerFactory.GetScheduler();
			scheduler.JobFactory = context.Resolve<IJobFactory>();
			return scheduler;
		}

		private void SetupLogging(ContainerBuilder builder)
		{
			builder.Register(context =>
					{
						var identity = context.Resolve<IUserIdentity>();
						var dbTarget = new NLogDBLogTarget(new AutofacDIFactory(context), identity);
						dbTarget.Layout = "${message} ${exception:format=message,stacktrace:separator=\r\n}";
						var loggingConfig = NLog.LogManager.Configuration;
						if (loggingConfig == null)
						{
							loggingConfig = new NLog.Config.LoggingConfiguration();
						}
						loggingConfig.AddTarget("dbTarget", dbTarget);
						var rule = new NLog.Config.LoggingRule("*", NLog.LogLevel.Trace, dbTarget);
						loggingConfig.LoggingRules.Add(rule);
						NLog.LogManager.Configuration = loggingConfig;

						var logger = NLog.LogManager.GetCurrentClassLogger();

						return logger;
					})
					.As<NLog.Logger>()
					.SingleInstance();
		}
	}
}
