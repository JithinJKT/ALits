using ALits.DesingsClass;
using ALits.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALits.ViewPages.ShoppingListPages.AddPopUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DfltShngAdditem : Rg.Plugins.Popup.Pages.PopupPage
    {
        //private Entry entrItemtName;
        //private Button btnAddItem;
        private int m_strId;
        private string m_strListName;
        private RandomColor m_randomColor;      

        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "AListDb");       

        public DfltShngAdditem()
        {
            InitializeComponent();
            m_strId = Convert.ToInt32(StaticShoppingListsVar.Id);
            m_strListName = StaticShoppingListsVar.ListName;
            m_randomColor = new RandomColor();
        }

        private async void BtnAddItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(entrItemtName.Text))
                {

                    var db = new SQLiteConnection(dbPath);

                    var maxPK = db.Table<AllUserShoppingLists>().OrderByDescending(c => c.Id).FirstOrDefault();
                    AllUserShoppingLists AllUserShoppingLists = new AllUserShoppingLists()
                    {
                        Id = (maxPK == null ? 1 : maxPK.Id + 1),
                        ListID = m_strId,
                        ListName = m_strListName,
                        ItemName = entrItemtName.Text,
                        ItemDesc = entrItemtDesc.Text,
                        ItemBackColor = m_randomColor.RandomColorPicker(maxPK == null ? 1 : maxPK.Id + 1),
                        ItemFontColorDark = "#000000",//black
                        ItemFontColorLight = "#FFFFFF"//white

                    };
                    AllUserShoppingLists.ItemBackColor = m_randomColor.RandomColorPicker(maxPK == null ? 1 : maxPK.Id + 1);
                    db.Insert(AllUserShoppingLists);

                    MessagingCenter.Send<App>((App)Application.Current, "OnUserShoppingListReload");
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
        protected override bool OnBackgroundClicked()
        {
            MessagingCenter.Send<App>((App)Application.Current, "OnUserShoppingListReload");
            return true;
        }
    }
}