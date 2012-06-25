using System;
using System.Reflection;
using System.ServiceProcess;

namespace CodeBits
{
    public sealed class WindowsServiceRunner
    {
        /// <summary>
        /// Starts the service logic.
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

        public event EventHandler ConsoleModeStarted;
        public event EventHandler<ConsoleModeExceptionEventArgs> ConsoleModeException;
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

    public sealed class ConsoleModeExceptionEventArgs : EventArgs
    {
        private readonly Exception _exception;

        public ConsoleModeExceptionEventArgs(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");
            _exception = exception;
        }

        public Exception Exception
        {
            get { return _exception; }
        }
    }
}