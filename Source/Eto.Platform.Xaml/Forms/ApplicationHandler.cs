using System;
using System.Collections.Generic;
using Eto.Forms;
using System.Diagnostics;
using sw = Windows.UI.Xaml;
using wf = Windows.Foundation;
using swm = Windows.UI.Xaml.Media;
using System.Threading;

namespace Eto.Platform.Xaml.Forms
{
	/// <summary>
	/// Application handler.
	/// </summary>
	/// <copyright>(c) 2014 by Vivek Jhaveri</copyright>
	/// <copyright>(c) 2012-2013 by Curtis Wensley</copyright>
	/// <license type="BSD-3">See LICENSE for full terms</license>
	public class ApplicationHandler : WidgetHandler<sw.Application, Application>, IApplication
	{
		bool attached;
		bool shutdown;
		string badgeLabel;
		static ApplicationHandler instance;
		List<sw.Window> delayShownWindows;

		public static ApplicationHandler Instance
		{
			get { return instance; }
		}

		public static bool EnableVisualStyles = true;

		public static void InvokeIfNecessary(Action action)
		{
#if TODO_XAML
			if (sw.Application.Current == null || Thread.CurrentThread == sw.Application.Current.Dispatcher.Thread)
				action();
			else
			{
				sw.Application.Current.Dispatcher.Invoke(action);
			}
#else
			throw new NotImplementedException();
#endif
		}

		public List<sw.Window> DelayShownWindows
		{
			get
			{
				if (delayShownWindows == null)
					delayShownWindows = new List<sw.Window>();
				return delayShownWindows;
			}
		}

		public bool IsStarted { get; private set; }

		public override sw.Application CreateControl()
		{
			return new sw.Application();
		}

		protected override void Initialize()
		{
			base.Initialize();
			instance = this;
#if TODO_XAML
			Control.Startup += HandleStartup;
#endif
		}

#if TODO_XAML
		void HandleStartup(object sender, sw.StartupEventArgs e)
		{
			IsActive = true;
			IsStarted = true;
			Control.Activated += (sender2, e2) => IsActive = true;
			Control.Deactivated += (sender2, e2) => IsActive = false;
			if (delayShownWindows != null)
			{
				foreach (var window in delayShownWindows)
				{
					window.Show();
				}
				delayShownWindows = null;
			}
		}
#endif
		public bool IsActive { get; private set; }

		public string BadgeLabel
		{
			get { return badgeLabel; }
			set
			{
				badgeLabel = value;
#if TODO_XAML
				var mainWindow = sw.Application.Current.MainWindow;
				if (mainWindow != null)
				{
					if (mainWindow.TaskbarItemInfo == null)
						mainWindow.TaskbarItemInfo = new sw.Shell.TaskbarItemInfo();
					if (!string.IsNullOrEmpty(badgeLabel))
					{
						var ctl = new CustomControls.OverlayIcon();
						ctl.Content = badgeLabel;
						ctl.Measure(new wf.Size(16, 16));
						var size = ctl.DesiredSize;

						var m = sw.PresentationSource.FromVisual(mainWindow).CompositionTarget.TransformToDevice;

						var bmp = new swm.Imaging.RenderTargetBitmap((int)size.Width, (int)size.Height, m.M22 * 96, m.M22 * 96, swm.PixelFormats.Default);
						ctl.Arrange(new wf.Rect(size));
						bmp.Render(ctl);
						mainWindow.TaskbarItemInfo.Overlay = bmp;
					}
					else
						mainWindow.TaskbarItemInfo.Overlay = null;
				}
#endif
			}
		}


		public void RunIteration()
		{
		}

		public void Quit()
		{
			bool cancel = false;
#if TODO_XAML
			foreach (sw.Window window in Control.Windows)
			{
				window.Close();
				cancel |= window.IsVisible;
			}
			if (!cancel)
			{
				Control.Shutdown();
				shutdown = true;
			}
#endif
		}

		public void Invoke(Action action)
		{
#if TODO_XAML
			Control.Dispatcher.Invoke(action, sw.Threading.DispatcherPriority.Background);
#else
			throw new NotImplementedException();
#endif
		}

		public void AsyncInvoke(Action action)
		{
#if TODO_XAML
			Control.Dispatcher.BeginInvoke(action);
#else
			throw new NotImplementedException();
#endif
		}

		public IEnumerable<Command> GetSystemCommands()
		{
			yield break;
		}

		public void CreateStandardMenu(MenuItemCollection menuItems, IEnumerable<Command> commands)
		{
		}

		public Keys CommonModifier
		{
			get { return Keys.Control; }
		}

		public Keys AlternateModifier
		{
			get { return Keys.Alt; }
		}

		public void Open(string url)
		{
#if TODO_XAML
			Process.Start(url);
#else
					throw new NotImplementedException();
#endif
		}

		public void Run(string[] args)
		{
			Widget.OnInitialized(EventArgs.Empty);
			if (!attached)
			{
				if (shutdown) return;
				if (Widget.MainForm != null)
				{
#if TODO_XAML
					Control.Run((Windows.UI.Xaml.Window)Widget.MainForm.ControlObject);
#else
					throw new NotImplementedException();
#endif
				}
				else
				{
#if TODO_XAML
					Control.ShutdownMode = sw.ShutdownMode.OnExplicitShutdown;
					Control.Run();
#else
					throw new NotImplementedException();
#endif
				}
			}
		}

		public void Attach(object context)
		{
			attached = true;
			Control = sw.Application.Current;
		}

		public void OnMainFormChanged()
		{
		}

		public void Restart()
		{
#if TODO_XAML
			Process.Start(Windows.UI.Xaml.Application.ResourceAssembly.Location);
			Windows.UI.Xaml.Application.Current.Shutdown();
#else
			throw new NotImplementedException();
#endif
		}

		public override void AttachEvent(string id)
		{
			switch (id)
			{
				case Application.TerminatingEvent:
					// handled by WpfWindow
					break;
				default:
					base.AttachEvent(id);
					break;
			}
		}
	}
}
