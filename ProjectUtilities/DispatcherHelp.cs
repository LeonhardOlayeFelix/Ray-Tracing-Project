
using System.Windows;
using System.Windows.Threading;

namespace ProjectUtilities
{
    public static class DispatcherHelp
    {
        private static Dispatcher _uiDispatcher { get; set; }
        public static void Initialise()
        {
            if (_uiDispatcher != null && _uiDispatcher.Thread.IsAlive)
                return;
            _uiDispatcher = Dispatcher.CurrentDispatcher;
        }
        /// <summary>
        /// Executes an action on the UI Thread, dispatching the action to the appropriate dispatcher whenever necessary.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="dispatcherPriority"></param>
        public static void CheckInvokeOnUI(Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Send)
        {
            try
            {
                if (action == null)
                    return;
                CheckDispatcher();

                if (_uiDispatcher.CheckAccess())
                {
                    action();
                }
                else
                {
                    _uiDispatcher.Invoke(action, dispatcherPriority);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void CheckDispatcher()
        {
            if (_uiDispatcher == null)
                throw new InvalidOperationException("The DispatcherHelp Class was not initialised before use. Call DispatcherHelp.Initialise() in the static App constructor.");
        }
    }

}
