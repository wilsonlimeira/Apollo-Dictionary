using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;

namespace Wilson_oficial.Pages
{
	public partial class MyPopupPage : PopupPage
	{
        ApolloDictionary app;

        StackLayout layout;
        Entry typedWord, typedDefinition, typedCategory;
        Button addItemButton;
        Rectangle rect;

        public MyPopupPage ()
		{
			InitializeComponent ();

            app = new ApolloDictionary();

            //Layout
            typedWord = new Entry { Placeholder = "New word" };
            typedDefinition = new Entry { Placeholder = "Word definition..." };
            typedCategory = new Entry { Placeholder = "Category (e.g. Acronyms)" };

            addItemButton = new Button
            {
                Text = "Add"
            };

            layout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Accent,
                Padding = new Thickness(5,5,5,5),
                Children = { typedWord, typedDefinition, typedCategory, addItemButton }
            };

            Content = layout;

            addItemButton.Clicked += AddItemButton_Clicked;
		}

        private void AddItemButton_Clicked(object sender, EventArgs e)
        {
            string name = ToUpperFirstLetter(typedWord.Text);
            string definition = ToUpperFirstLetter(typedDefinition.Text);
            string category = ToUpperFirstLetter(typedCategory.Text);

            //add a new word to the database
            try
            {
                app.AddSingleWord = new WordDefinition
                {
                    Name = name,
                    Definition = definition,
                    Category = category
                };

                //Send a message to every view to refresh their screen with this new word
                MessagingCenter.Send<MyPopupPage>(this, "newWord");
            }
            catch (Exception except) //in case this word already exist
            {
                DisplayAlert("Sorry", except.Message, "OK");
            }

            OnBackButtonPressed();

        }

        private static string ToUpperFirstLetter(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return string.Empty;
            }

            var aux = word.ToCharArray();
            aux[0] = char.ToUpper(aux[0]);

            return new string(aux);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // Method for animation child in PopupPage
        // Invoced after custom animation end
        protected virtual Task OnAppearingAnimationEnd()
        {
            return Content.FadeTo(0.5);
        }

        // Method for animation child in PopupPage
        // Invoked before custom animation begin
        protected virtual Task OnDisappearingAnimationBegin()
        {
            return Content.FadeTo(1); ;
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            //return base.OnBackButtonPressed();
            Navigation.PopModalAsync();
            return true;
        }

        // Invoced when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return default value - CloseWhenBackgroundIsClicked
            Navigation.PopModalAsync();
            return base.OnBackgroundClicked();
        }
    }
}
