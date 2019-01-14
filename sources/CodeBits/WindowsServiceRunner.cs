#region --- License & Copyright Notice ---
/*
Useful code blocks that can included in your C# projects through NuGet
Copyright (c) 2012-2019 Jeevan James
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

/*
Required References:
 * System.ServiceProcess
Documentation: http://codebits.codeplex.com/wikipage?title=WindowsServiceRunner
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace CodeBits
{
    /// <summary>
    ///     Provides functionality to execute a Windows service class as either a console application or
    ///     a Windows service, depending on how the project properties are set.
    ///     To run as a console application, set the project output type to "Console Application"
    ///     To run as a Windows service, set the project output type to "Windows Application"
    /// </summary>
    public sealed class WindowsServiceRunner
    {
        private readonly List<Func<ServiceBase>> _serviceFactories = new List<Func<ServiceBase>>();

        static WindowsServiceRunner()
        {
            if (Environment.UserInteractive)
                Trace.Listeners.Add(new ConsoleTraceListener());
        }

        public WindowsServiceRunner()
        {
        }

        public WindowsServiceRunner(IEnumerable<Type> serviceTypes)
        {
            foreach (Type serviceType in serviceTypes)
                Register(serviceType);
        }

        public static void Run(params Type[] serviceTypes)
        {
            var runner = new WindowsServiceRunner(serviceTypes);
            runner.Run();
        }

        /// <summary>
        ///     Registers the specified service type to be run.
        /// </summary>
        /// <typeparam name="TService"> The type of service to run. This type must be a subclass of ServiceBase and should have a default constructor </typeparam>
        public void Register<TService>() where TService : ServiceBase, new()
        {
            _serviceFactories.Add(() => new TService());
        }

        /// <summary>
        /// Registers the specified service type to run
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="args"></param>
        public void Register<TService>(params object[] args) where TService : ServiceBase
        {
            _serviceFactories.Add(() => (ServiceBase)Activator.CreateInstance(typeof(TService), args));
        }

        public void Register(Type serviceType, params object[] args)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");
            if (!serviceType.IsSubclassOf(typeof(ServiceBase)))
            {
                throw new ArgumentException(
                    string.Format("Specified service type '{0}' does not subclass System.ServiceProcess.ServiceBase", serviceType),
                    "serviceType");
            }

            _serviceFactories.Add(() => (ServiceBase)Activator.CreateInstance(serviceType, args));
        }

        /// <summary>
        ///     Starts the services' logic in the appropriate environment.
        /// </summary>
        public void Run()
        {
            if (_serviceFactories.Count == 0)
                throw new InvalidOperationException("No services available to run. Add services using the Register method.");
            if (Environment.UserInteractive)
                RunAsConsole();
            else
                RunAsService();
        }

        /// <summary>
        ///     Fired when the service logic starts in console mode.
        /// </summary>
        public event EventHandler<ConsoleModeEventArgs> ConsoleModeStarted;

        /// <summary>
        ///     Fired if an exception is thrown in console mode.
        /// </summary>
        public event EventHandler<ConsoleModeExceptionEventArgs> ConsoleModeException;

        /// <summary>
        ///     Fired when the service logic completes in console mode.
        /// </summary>
        public event EventHandler<ConsoleModeEventArgs> ConsoleModeStopped;

        private void RunAsConsole()
        {
            Parallel.ForEach(_serviceFactories, factory => {
                ServiceBase service = factory();
                Type serviceHostType = service.GetType();
                MethodInfo onStartMethod = serviceHostType.GetMethod("OnStart", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo onStopMethod = serviceHostType.GetMethod("OnStop", BindingFlags.NonPublic | BindingFlags.Instance);

                try
                {
                    FireConsoleModeStarted(service, serviceHostType);
                    onStartMethod.Invoke(service, new object[] { null });
                }
                catch (Exception ex)
                {
                    FireConsoleModeException(service, serviceHostType, ex);
                }
                finally
                {
                    onStopMethod.Invoke(service, null);
                    FireConsoleModeStopped(service, serviceHostType);
                }
            });
            Console.ReadLine();
        }

        private void FireConsoleModeStarted(ServiceBase serviceInstance, Type serviceType)
        {
            EventHandler<ConsoleModeEventArgs> consoleModeStarted = ConsoleModeStarted;
            if (consoleModeStarted != null)
                consoleModeStarted(this, new ConsoleModeEventArgs(serviceInstance, serviceType));
        }

        private void FireConsoleModeException(ServiceBase serviceInstance, Type serviceType, Exception exception)
        {
            EventHandler<ConsoleModeExceptionEventArgs> consoleModeException = ConsoleModeException;
            if (consoleModeException != null)
                consoleModeException(this, new ConsoleModeExceptionEventArgs(serviceInstance, serviceType, exception));
        }

        private void FireConsoleModeStopped(ServiceBase serviceInstance, Type serviceType)
        {
            EventHandler<ConsoleModeEventArgs> consoleModeStopped = ConsoleModeStopped;
            if (consoleModeStopped != null)
                consoleModeStopped(this, new ConsoleModeEventArgs(serviceInstance, serviceType));
        }

        private void RunAsService()
        {
            ServiceBase.Run(_serviceFactories.Select(func => func()).ToArray());
        }
    }

    /// <summary>
    ///     Provides data for the console mode lifecycle events.
    /// </summary>
    public class ConsoleModeEventArgs : EventArgs
    {
        private readonly ServiceBase _serviceInstance;
        private readonly Type _serviceType;

        internal ConsoleModeEventArgs(ServiceBase serviceInstance, Type serviceType)
        {
            _serviceInstance = serviceInstance;
            _serviceType = serviceType;
        }

        public ServiceBase ServiceInstance
        {
            get { return _serviceInstance; }
        }

        public Type ServiceType
        {
            get { return _serviceType; }
        }
    }

    /// <summary>
    ///     Provides data for the ConsoleModeException event
    /// </summary>
    public sealed class ConsoleModeExceptionEventArgs : ConsoleModeEventArgs
    {
        private readonly Exception _exception;

        internal ConsoleModeExceptionEventArgs(ServiceBase serviceInstance, Type serviceType, Exception exception)
            : base(serviceInstance, serviceType)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");
            _exception = exception;
        }

        /// <summary>
        ///     The exception that was thrown.
        /// </summary>
        public Exception Exception
        {
            get { return _exception; }
        }
    }
}