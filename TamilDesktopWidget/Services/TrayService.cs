using System;
using System.Windows;

using Forms = System.Windows.Forms;
using Drawing = System.Drawing;

namespace TamilDesktopWidget.Services
{
    public class TrayService : IDisposable
    {
        private readonly Forms.NotifyIcon _notifyIcon;
        private readonly MainWindow _mainWindow;

        public TrayService(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            _notifyIcon = new Forms.NotifyIcon
            {
                Text = "Tamil Desktop Widget",
                Icon = Drawing.SystemIcons.Application,
                Visible = true
            };

            var menu = new Forms.ContextMenuStrip();


            menu.Items.Add("🔓 Unlock Widget", null, (s, e) =>
            {
                _mainWindow.UnlockWidget();
            });

            menu.Items.Add("🔒 Lock Widget", null, (s, e) =>
            {
                _mainWindow.LockWidget();
            });

            menu.Items.Add(new Forms.ToolStripSeparator());

            menu.Items.Add("❌ Exit", null, (s, e) =>
            {
                _notifyIcon.Visible = false;
                System.Windows.Application.Current.Shutdown();
            });

            _notifyIcon.ContextMenuStrip = menu;
        }

        public void Dispose()
        {
            _notifyIcon.Dispose();
        }
    }
}