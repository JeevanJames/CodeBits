#region --- License & Copyright Notice ---
/*
CodeBits Code Snippets
Copyright (c) 2012 Jeevan James
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

using System;
using System.Reflection;
using System.ServiceProcess;

namespace CodeBits
{
    /// <summary>
    /// Provides functionality to execute a Windows service class as either a console application or
    /// a Windows service, depending on how the project properties are set.
    /// To run as a console application, set the project output type to "Console Application"
    /// To run as a Windows service, set the project output type to "Windows Application"
    /// </summary>
    public sealed class WindowsServiceRunner
    {
        /// <summary>
        /// Starts the service logic in the appropriate environment
        /// </summary>
        /// <typeparam name="TService">The type of the service to start</typeparam>
         public void Run<TService>()
             where TService : ServiceBase, new()
         {
             var service = new TService();
             if (Environment.UserInteractive)
                 RunAsConsole(service);
             else
                 RunAsService(service);
         }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ConsoleModeStarted;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ConsoleModeExceptionEventArgs> ConsoleModeException;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ConsoleModeStopped;

        private void RunAsConsole(ServiceBase service)
        {
            Type serviceHostType = service.GetType();
            MethodInfo onStartMethod = serviceHostType.GetMethod("OnStart", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo onStopMethod = serviceHostType.GetMethod("OnStop", BindingFlags.NonPublic | BindingFlags.Instance);

            try
            {
                onStartMethod.Invoke(service, new object[] { null });
                FireConsoleModeStarted();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                FireConsoleModeException(ex);
                Console.ReadLine();
            }
            finally
            {
                onStopMethod.Invoke(service, null);
                FireConsoleModeStopped();
            }
        }

        private void FireConsoleModeStarted()
        {
            EventHandler consoleModeStarted = ConsoleModeStarted;
            if (consoleModeStarted != null)
                consoleModeStarted(this, EventArgs.Empty);
        }

        private void FireConsoleModeException(Exception exception)
        {
            EventHandler<ConsoleModeExceptionEventArgs> consoleModeException = ConsoleModeException;
            if (consoleModeException != null)
                consoleModeException(this, new ConsoleModeExceptionEventArgs(exception));
        }

        private void FireConsoleModeStopped()
        {
            EventHandler consoleModeStopped = ConsoleModeStopped;
            if (consoleModeStopped != null)
                consoleModeStopped(this, EventArgs.Empty);
        }

        private static void RunAsService(ServiceBase service)
        {
            var servicesToRun = new[] { service };
            ServiceBase.Run(servicesToRun);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ConsoleModeExceptionEventArgs : EventArgs
    {
        private readonly Exception _exception;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public ConsoleModeExceptionEventArgs(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");
            _exception = exception;
        }

        /// <summary>
        /// 
        /// </summary>
        public Exception Exception
        {
            get { return _exception; }
        }
    }
}