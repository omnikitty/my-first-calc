using Xamarin.Forms;

namespace Calc1
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new Calc1Page();

		}

		static public int ScreenHeight;

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
