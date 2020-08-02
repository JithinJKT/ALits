using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using ALits.Models;
using ALits.DesingsClass;
using SQLite;
using Rg.Plugins.Popup.Extensions;

namespace ALits.ViewPages.AddPopPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrteShopigList : Rg.Plugins.Popup.Pages.PopupPage
    {

        private RandomColor m_randomColor;
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "AListDb");

        public CrteShopigList()
        {
            InitializeComponent();
            m_randomColor = new RandomColor();
        }
           

        private async  void AddList_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(entrListName.Text))
                {

                    var db = new SQLiteConnection(dbPath);                 

                    var maxPK = db.Table<AllShoppingLists>().OrderByDescending(c => c.Id).FirstOrDefault();
                    AllShoppingLists allShoppingLists = new AllShoppingLists()
                    {
                        Id = (maxPK == null ? 1 : maxPK.Id + 1),
                        ListName = entrListName.Text,
                        BackColorLight = m_randomColor.RandomColorPicker(maxPK == null ? 1 : maxPK.Id + 1),
                        FontColorLight = "#000000",//black
                        FontColorDark= "#FFFFFF"//white
                    };
                    db.Insert(allShoppingLists);

                    MessagingCenter.Send<App>((App)Application.Current, "OnShoppingListReload");
                    await Navigation.PopPopupAsync();                 

                }
                else
                {
                    await DisplayAlert("Name is Empty!", "Please give a name for the list", "OK");
                }


            }
            catch (Exception ex)
            {

            }
        }

    }
}