using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calc1
{
	public partial class Calc1Page : ContentPage
	{
		string victim;
		string term = null;
		RelativeLayout myLayout;
		Grid myGrid;

		string answer;

		static double factor = App.ScreenHeight * 0.02;
		public static Thickness GridMargin =
			new Thickness { Top = factor, Bottom = factor, Right = 10, Left = 10 };

		public static double Spacing
		{ get { return App.ScreenHeight / 150; } }

		public static double ButtonFontSize
		{get { return App.ScreenHeight/26; }}

		public static double MiscFontSize
		{get { return App.ScreenHeight / 30; }}

		public static double BorderWidth
		{ get { return App.ScreenHeight / 300; } }

		public static double ScreenFontSize
		{ get { return App.ScreenHeight / 12; } }

		public static double HistFontSize
		{ get { return App.ScreenHeight / 22; } }

		public Calc1Page()
		{
			InitializeComponent();
			myLayout = Content as RelativeLayout;
			myGrid = myLayout.Children[1] as Grid;
			myLayout.Children.Add(myGrid, Constraint.RelativeToParent((parent) =>
			{
				return ((parent.Width - (parent.Height * 0.55)) / 2);
			}),
			  Constraint.RelativeToParent((parent) =>
			{
				return 0;
			}),
			  Constraint.RelativeToParent((parent) =>
			{
				return (parent.Height * 0.55);
			}),
			   Constraint.RelativeToParent((parent) =>
			{
				return parent.Height;
			}));

			screen.Text = null;
		//MAKES ALL THE NUMBER BUTTONS YOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
			for (int i = 0; i < 12; i++)
			{
				int nRow;
				int nColumn = 80;

				if (0 < i && i < 4)
					nRow = 3;
				else if (3 < i && i < 7)
					nRow = 4;
				else if (6 < i && i < 10)
					nRow = 5;
				else
					nRow = 6;

				switch (i % 3)
				{
					case 0:
						nColumn = i == 0 ? 1 : 2;
						break;
					case 1:
						nColumn = i == 10 ? 2 : 0; //10 is .
						break;
					case 2:
						nColumn = i == 11 ? 0 : 1; //11 is SS
						break;
					default:
						break;
				}

				Button nButton = new Button()
				{ };

				if (i == 11)
					nButton.Text = "???";
				else if (i == 10)
					nButton.Text = ".";
				else
					nButton.Text = i.ToString();
				
				nButton.SetDynamicResource(StyleProperty, "nStyle");
				Resources["nStyle"] = Resources["NumStyle"];
				myGrid.Children.Add(nButton, nColumn, nRow);

				//event handler for when it's clicked
				if (i != 11)
				{
					nButton.Clicked += (sender, e) =>
					{
						if (screen.Text == null)
							screen.Text += (sender as Button).Text;
						else if ((nButton.Text == "." && screen.Text[screen.Text.Length - 1] == '.') == false)
							screen.Text += (sender as Button).Text;
					};
				}
				else
					nButton.Clicked += OnSSClicked; //sends it to the other event handler
			}


			//MAKING ALL THE OP BUTTONS YOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
			for (int i = 0; i < 4; i++)
			{
				Button oButton = new Button()
				{ };

				if (i == 0)
					oButton.Text = "x";
				else if (i == 1)
					oButton.Text = "/";
				else if (i == 2)
					oButton.Text = "+";
				else if (i == 3)
					oButton.Text = "-";

				oButton.SetDynamicResource(StyleProperty, "oStyle");
				Resources["oStyle"] = Resources["OpStyle"];

				int oColumn = i;
				int oRow = 2;
				myGrid.Children.Add(oButton, oColumn, oRow);

				oButton.Clicked += (sender, e) =>
				{
					if (screen.Text != null) //if there's already something on the screen
					{
						switch (screen.Text[screen.Text.Length - 1]) //if the last character is...
						{
							case 'x':
							case '/':
								if (oButton.Text == "-")
									screen.Text += (sender as Button).Text;
								break;
							case '+':
							case '-':
								break;
							default:
								screen.Text += (sender as Button).Text;
								break;
						}
					}
					else if (oButton.Text == "-")// - is the only operator allowed to be the first
						screen.Text += (sender as Button).Text;
				};
			}

			//MAKING ALL THE MISC BUTTONS YOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
			for (int i = 0; i < 3; i++)
			{
				Button mButton = new Button()
				{ };
				int mColumn = 3;
				int mRow = i + 3;
				myGrid.Children.Add(mButton, mColumn, mRow);

				if (i == 0)
				{
					mButton.Text = "clear";
					mButton.Clicked += OnClearClicked;
				}
				else if (i == 1)
				{
					mButton.Text = "del";
					mButton.Clicked += OnDelClicked;
				}
				else
				{
					mButton.Text = "=";
					mButton.Clicked += OnEqualsClicked;
					mButton.SetValue(Grid.RowSpanProperty, 2);
				}

				mButton.SetDynamicResource(StyleProperty, "mStyle");
				Resources["mStyle"] = Resources["MiscStyle"];
			}


		}


		
		List<string> allTerms = new List<string>();
		void OnEqualsClicked(object sender, EventArgs args)
		{
			term = null;
			answer = null;
			allTerms.Clear();
			history.Text = screen.Text; 
			victim = screen.Text[0] == '-' ? screen.Text : '+' + screen.Text;

			//CREATING THE TERMS & PUTTING IN LIST
			term += victim[0];
				
			for (int i = 1; i < victim.Length; i++)
			{
				if (victim[i] == '+' || victim[i] == '-' || victim[i] == 'x' || victim[i] == '/')
				{
					allTerms.Add(term);
					term = null;
					term += victim[i];
				}
				else if (i == victim.Length-1)
				{
					term += victim[i];
					allTerms.Add(term);
				}
				else
					term += victim[i];
			}


			while (allTerms.Count() > 1) //yall is there a way to make this less ugly
			{
				for (int i = 1; i < allTerms.Count(); i++) //multiply and divide first
				{
						string term = allTerms[i]; 
						double termpair = Convert.ToDouble(allTerms[i - 1]);
						
						if (term.Contains("x"))
						{
							double pureterm = Convert.ToDouble(term.Substring(1));
							answer = (Convert.ToString(termpair * pureterm))[0] == '-' ? Convert.ToString(termpair * pureterm) : '+' + Convert.ToString(termpair * pureterm);

							allTerms.RemoveAt(i);
							allTerms.RemoveAt(i - 1);
							allTerms.Insert(i - 1, answer);
							i--;
						}

						else if (term.Contains("/"))
						{
							double pureterm = Convert.ToDouble(term.Substring(1));
							answer = (Convert.ToString(termpair / pureterm))[0] == '-' ? Convert.ToString(termpair / pureterm) : '+' + Convert.ToString(termpair / pureterm);

							allTerms.RemoveAt(i);
							allTerms.RemoveAt(i - 1);
							allTerms.Insert(i - 1, answer);
							i--;
						}
					
						else
							continue;
				}


				for (int k = 1; k < allTerms.Count(); k++) //the addition and subtraction
				{
					string term = allTerms[k];
					double termpair = Convert.ToDouble(allTerms[k - 1]); 

					if (term.Contains("+"))
					{
						double pureterm = Convert.ToDouble(term.Substring(1));
						answer = (Convert.ToString(termpair + pureterm))[0] == '-' ? Convert.ToString(termpair + pureterm) : '+' + Convert.ToString(termpair + pureterm);

						allTerms.RemoveAt(k);
						allTerms.RemoveAt(k - 1);
						allTerms.Insert(k - 1, answer);
						k--;
					}

					else if (term.Contains("-"))
					{
						double pureterm = Convert.ToDouble(term.Substring(1));
						answer = (Convert.ToString(termpair - pureterm))[0] == '-' ? Convert.ToString(termpair - pureterm) : '+' + Convert.ToString(termpair - pureterm);

						allTerms.RemoveAt(k);
						allTerms.RemoveAt(k - 1);
						allTerms.Insert(k - 1, answer);
						k--;
					}

					else
						continue;
				}
			}

			answer = answer[0] == '+'? answer.Substring(1) : answer;
			if (answer.Contains("."))
			{
				int IOfDeci = answer.IndexOf('.');
				int commas = IOfDeci == 0 ? 0 : (IOfDeci - 1) / 3;
				for (int i = 0; i < commas; i++)
				{
					answer = answer.Insert((IOfDeci - 3) - 3*i, ",");
				}
			}
			else
			{
				int AnsLength = answer.Length;
				int commas = (AnsLength - 1) / 3;
				for (int i = 0; i < commas; i++)
				{
					answer = answer.Insert((AnsLength - 3) - 3*i, ",");
				}
			}

			screen.Text = answer;
		
		}

		void OnClearClicked(object sender, EventArgs args)
		{
			screen.Text = null;
			history.Text = null;
			allTerms.Clear();
		}

		void OnDelClicked(object sender, EventArgs args)
		{
			if (screen.Text != null)
			screen.Text = screen.Text.Remove(screen.Text.Length - 1);
		}

	//SPECIAL !!!
		Image surprise = new Image(){};
		int ssnowflake = 0;
		void OnSSClicked(object sender, EventArgs args)
		{
			screen.Text += "??";

			switch (ssnowflake % 6)
			{
				case 0:
					surprise.Source = "gavin1.jpg";
					surprise.IsVisible = true;
					break;
				case 1:
					surprise.Source = "gavin2.jpg";
					surprise.IsVisible = true;
					break;
				case 2:
					surprise.Source = "gavin3.jpg";
					surprise.IsVisible = true;
					break;
				case 3:
					surprise.Source = "gavin4.jpg";
					surprise.IsVisible = true;
					break;
				case 4:
					surprise.Source = "gavin5.jpg";
					surprise.IsVisible = true;
					break;
				case 5:
					surprise.IsVisible = false;
					screen.Text = null;
					history.Text = null;
					break;
			}
			myGrid.Children.Add(surprise, 0, 1);
			surprise.SetValue(Grid.RowSpanProperty, 5);
			surprise.SetValue(Grid.ColumnSpanProperty, 4);
			ssnowflake++;
		}
	}
}

