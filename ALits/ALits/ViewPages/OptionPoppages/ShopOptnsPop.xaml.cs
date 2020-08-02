using ALits.Models;
using ALits.ViewPages.ShoppingListPages;
using Rg.Plugins.Popup.Extensions;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALits.ViewPages.OptionPoppages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopOptnsPop : Rg.Plugins.Popup.Pages.PopupPage
    {
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "AListDb");
        private int m_strId;
        private string m_strListName;      

        private string listHeading;
        public string ListNameHeading
        {
            get { return listHeading; }
            set
            {
                listHeading = value;
                OnPropertyChanged(nameof(ListNameHeading)); // Notify that there was a change on this property
            }
        }
        public ShopOptnsPop()
        {
            InitializeComponent();
            m_strId = Convert.ToInt32(StaticShoppingListsVar.Id);
            m_strListName = StaticShoppingListsVar.ListName;
            ListNameHeading = m_strListName;
            BindingContext = this;
            ListNameHeading = ListNameHeading;
        }

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit List")
            {
                btnAddItems.IsVisible = false;
                btnDelete.Text = "Cancel";
                btnEdit.Text = "Save";
                entrListName.IsVisible = true;
                entrListName.Text = m_strListName;
            }
            else
            {
                if (!string.IsNullOrEmpty(entrListName.Text))
                {
                   
                    var db = new SQLiteConnection(dbPath);
                    List<AllUserShoppingLists> objAllShoppingLists = new List<AllUserShoppingLists>();
                    objAllShoppingLists = db.Table<AllUserShoppingLists>().Where(i => i.ListID == m_strId && i.ListName == m_strListName).ToList();
                    int iIndex = 1;
                    foreach (AllUserShoppingLists objInfo in objAllShoppingLists)
                    {
                        objInfo.ListName = entrListName.Text;
                        db.Update(objInfo);
                        iIndex++;
                    }
                    var ShoppingLists = db.Table<AllShoppingLists>().Where(i => i.Id == m_strId && i.ListName == m_strListName).FirstOrDefault();
                    ShoppingLists.ListName = entrListName.Text;
                    db.Update(ShoppingLists);
                    MessagingCenter.Send<App>((App)Application.Current, "OnShoppingListReload");
                    await Navigation.PopPopupAsync();
                }
                else
                {
                    await DisplayAlert("Name is Empty!", "Please give a name for the list", "OK");
                }
            }
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            if (btnDelete.Text == "Cancel")
            {
                btnAddItems.IsVisible = true;
                btnDelete.Text = "Delete List";
                btnEdit.Text = "Edit List";
                entrListName.IsVisible = false;
            }
            else
            {
                bool answer = await DisplayAlert("Delete ?", "Do you want to delete this whole section ?", "Yes", "No");
                if (answer)
                {
                   
                    var db = new SQLiteConnection(dbPath);
                    List<AllUserShoppingLists> objAllShoppingLists = new List<AllUserShoppingLists>();
                    objAllShoppingLists = db.Table<AllUserShoppingLists>().Where(i => i.ListID == m_strId && i.ListName == m_strListName).ToList();
                    int iIndex = 1;
                    foreach (AllUserShoppingLists objInfo in objAllShoppingLists)
                    {
                        db.Delete(objInfo);
                        iIndex++;
                    }
                    var ShoppingLists = db.Table<AllShoppingLists>().Where(i => i.Id == m_strId && i.ListName == m_strListName).FirstOrDefault();
                    db.Delete(ShoppingLists);
                    MessagingCenter.Send<App>((App)Application.Current, "OnShoppingListReload");
                    await Navigation.PopPopupAsync();
                }
                
            }
        }

        private void btnAddItems_Clicked(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
            Navigation.PushModalAsync(new NavigationPage(new DefaultUserShoppingList()));
        }
    }
}