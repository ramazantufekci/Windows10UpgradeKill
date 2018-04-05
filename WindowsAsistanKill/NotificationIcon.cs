/*
 * Created by SharpDevelop.
 * User: ramazan
 * Date: 4/4/2018
 * Time: 2:57 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace WindowsAsistanKill
{
	public sealed class NotificationIcon
	{
		private NotifyIcon notifyIcon;
		private ContextMenu notificationMenu;
		
		#region Initialize icon and menu
		public NotificationIcon()
		{
			notifyIcon = new NotifyIcon();
			notificationMenu = new ContextMenu(InitializeMenu());
			notifyIcon.DoubleClick += IconDoubleClick;
			notifyIcon.Icon = new Icon("WindowsAsistanKill.ico");
			notifyIcon.ContextMenu = notificationMenu;
		}
		
		private MenuItem[] InitializeMenu()
		{
			MenuItem[] menu = new MenuItem[] {
				new MenuItem("Bilgi", menuAboutClick),
				new MenuItem("Çıkış", menuExitClick)
			};
			return menu;
		}
		#endregion
		
		#region Main - Program entry point
		/// <summary>Program entry point.</summary>
		/// <param name="args">Command Line Arguments</param>
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			var timer = new System.Timers.Timer(4000);
			timer.Elapsed += timer_Elapsed;
			timer.AutoReset = true;
			timer.Enabled = true;
			bool isFirstInstance;
			// Please use a unique name for the mutex to prevent conflicts with other programs
			using (Mutex mtx = new Mutex(true, "WindowsAsistanKill", out isFirstInstance)) {
				if (isFirstInstance) {
					NotificationIcon notificationIcon = new NotificationIcon();
					notificationIcon.notifyIcon.Visible = true;
					Application.Run();
					notificationIcon.notifyIcon.Dispose();
				} else {
					// The application is already running
					// TODO: Display message box or change focus to existing application instance
				}
			} // releases the Mutex
			
		}
		static bool deneme=true;

		static void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			try {
				if(deneme)
				{
					
					deneme = false;
					RegistryKey registry = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
					String[] deneme5 = registry.GetValueNames();
					foreach (string sayfa in deneme5) {
						if (!sayfa.Contains("WindowsUpgradeKill")) {
							registry.SetValue("WindowsUpgradeKill","\""+Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)+"\\DR Yazilim\\WindowsAsistanKill.exe"+"\"");
						}
						
					}
					registry.Close();
					Process[] process = Process.GetProcesses();
					foreach (Process element in process) {
						if(element.ProcessName.Contains("Windows10UpgraderApp"))
						{
							element.Kill();
						}
					}
					deneme = true;
				}else
				{
					
				}
				
			
				
			} catch (Exception) {
				
				throw;
			}
			
		}

		#endregion
		
		#region Event Handlers
		private void menuAboutClick(object sender, EventArgs e)
		{
			if(MessageBox.Show("Ramazan TÜFEKÇİ","DR Yazılım",MessageBoxButtons.OKCancel,MessageBoxIcon.Asterisk) == DialogResult.OK)
			{
				Process.Start("www.ramazantufekci.com");
			}
		}
		
		private void menuExitClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
		
		private void IconDoubleClick(object sender, EventArgs e)
		{
			MessageBox.Show("Beta");
		}
		#endregion
	}
}
