using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace Wilson_oficial.Pages
{
	public class HomePage : TabbedPage
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
