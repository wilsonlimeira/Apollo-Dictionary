using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace Wilson_oficial.Pages
{
	public class HomePage : TabbedPage //tem outras opções, ver depois: https://blog.xamarin.com/wp-content/uploads/2014/06/Pages-sml.png
    {
		public HomePage ()
		{
            Title = "Apollo";

            //TODO Verificar se a pagina nao foi aberta antes
            Children.Add(new SearchPage()
            {
                Title = "Search"
                //TODO Add icon
            });
		}
	}
}
