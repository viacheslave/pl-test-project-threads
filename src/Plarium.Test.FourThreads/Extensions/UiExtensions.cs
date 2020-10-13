using System.Windows.Forms;

namespace Plarium.Test.FourThreads.Extensions
{
    internal static class UiExtensions
    {
        // Allows to invoke a form control from a thread that is different from current
        public static void InvokeIfRequired(this Control control, MainWindow mainWindow, MethodInvoker action)
        {
            // This check is for the case when the form is being closed
            if (mainWindow != null && mainWindow.IsDisposed)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}