using ALits.Models;
using ALits.ViewPages.AddPopPages;
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
using ALits.ViewPages.ShoppingListPages;
using ALits.DesingsClass;
using ALits.ViewPages.OptionPoppages;

namespace ALits.ViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingPage : ContentPage
    {
        
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "AListDb");

        private StackLayout stackLayout;
        private Button BtnCreShpList;
        private Label labelID;
        private Label labelList;
        public Button ButtonDelete;
        private Frame FrameLayoutList;
        public Grid Gridlayoutlist;
        public ImageButton ImageButtonOptin;
        public ImageButton ImBtnCreShpList;
        private StackLayout stackLayoutList;
        private Frame FrameBlankSpace;
        private ImageButton ImageBtnStar;

        private RandomColor m_randomColor;

        private bool ListClicked;
        public ShoppingPage()
        {

            InitializeComponent();
            m_randomColor = new RandomColor();
            MessagingCenter.Subscribe<App>((App)Application.Current, "OnShoppingListReload", (sender) =>
            {
                ListClicked = false;
                this.Content = this.BuildView();
               
            });
           
            this.IconImageSource = "ShoppingWhite.png";
            var db = new SQLiteConnection(dbPath);

            //delete the whole table

            //db.DropTable<AllShoppingLists>();
            //db.DropTable<AllUserShoppingLists>();

            //db.CreateTable<AllShoppingLists>();
            //db.CreateTable<AllUserShoppingLists>();

            if (!IsTableAllShoppingListsExists())
            {
                db.CreateTable<AllShoppingLists>();

            }
            if (!IsTableAllUserShoppingListsExists())
            {
                db.CreateTable<AllUserShoppingLists>();
            }
            this.Content = this.BuildView();

        }

        private View BuildView()
        {
            var db = new SQLiteConnection(dbPath);

            Grid grid = new Grid();


            stackLayout = new StackLayout();
            List<AllShoppingLists> objAllShoppingLists = new List<AllShoppingLists>();
            objAllShoppingLists = db.Table<AllShoppingLists>().OrderByDescending(x => x.SatrCheckID).ToList();
            

            stackLayout = BuildListView(objAllShoppingLists);
            
            
            //notin use
            BtnCreShpList = new Button()
            {
                ImageSource = "DustbinWhite.png",
                Padding = 5,
            };
            BtnCreShpList.Clicked += BtnCreShpList_Clicked;
            BtnCreShpList.BorderWidth = 1;
            BtnCreShpList.CornerRadius = 35;
            BtnCreShpList.HorizontalOptions = LayoutOptions.End;
            BtnCreShpList.VerticalOptions = LayoutOptions.End;
            BtnCreShpList.FontAttributes = FontAttributes.Bold;
            BtnCreShpList.FontSize = 40;
            BtnCreShpList.BackgroundColor = System.Drawing.Color.FromArgb(75, 168, 98);
            BtnCreShpList.TextColor = Color.White;
            BtnCreShpList.WidthRequest = 70;
            BtnCreShpList.HeightRequest = 70;
            BtnCreShpList.Margin = 20;


            ImBtnCreShpList = new ImageButton()
            {
                Source = "AddImgWhite.png",
                Padding = 7,
                Margin = new Thickness(4, 2, 60, 30),
            };
            ImBtnCreShpList.BorderWidth = 1;
            ImBtnCreShpList.CornerRadius = 35;
            ImBtnCreShpList.HorizontalOptions = LayoutOptions.End;
            ImBtnCreShpList.VerticalOptions = LayoutOptions.End;
            ImBtnCreShpList.BackgroundColor = System.Drawing.Color.FromArgb(75, 168, 98);
            ImBtnCreShpList.WidthRequest = 70;
            ImBtnCreShpList.HeightRequest = 70;
            ImBtnCreShpList.Clicked += ImBtnCreShpList_Clicked;

            FrameBlankSpace = new Frame
            {
                HeightRequest = 400,
                Padding = 0,
                CornerRadius = 4,
                Margin = new Thickness(4, 2, 4, 2),
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.End,
            };
            stackLayout.Children.Add(FrameBlankSpace);

            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;

            grid.Children.Add(scrollView, 0, 0);
            grid.Children.Add(ImBtnCreShpList, 0, 0);
            return grid;
        }

        #region listing all shopping lists
        private StackLayout BuildListView(List <AllShoppingLists> objAllShoppingLists)
        {
            double Mediumsize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            int iIndex = 1;
            foreach (AllShoppingLists objInfo in objAllShoppingLists)
            {
                FrameLayoutList = new Frame
                {
                    HeightRequest = 90,
                    Padding = 0,
                    CornerRadius = 4,
                    Margin = new Thickness(4, 2, 4, 2),
                    BackgroundColor = Color.FromHex(objInfo.BackColorLight)

                };

                Gridlayoutlist = new Grid
                {
                    
                    
                };
                Gridlayoutlist.RowDefinitions.Add(new RowDefinition { Height =90 /*new GridLength(1, GridUnitType.Star)*/ });
                Gridlayoutlist.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                Gridlayoutlist.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Auto) });
                Gridlayoutlist.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                stackLayoutList = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Fill,
                    HorizontalOptions=LayoutOptions.Fill
                };

                TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
                tapGestureRecognizer.NumberOfTapsRequired = 1;

                labelID = new Label
                {
                    Text = (objInfo.Id).ToString(),
                    IsVisible = false

                };

                labelList = new Label
                {
                    Text = objInfo.ListName,
                    Margin = 20,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = Mediumsize,
                    HorizontalOptions=LayoutOptions.Center,
                    VerticalOptions=LayoutOptions.Center,
                    TextColor = Color.FromHex(objInfo.FontColorLight)

                };

                ImageButtonOptin = new ImageButton
                {
                    Source = "OptWhite.png",
                    HorizontalOptions = LayoutOptions.End,
                    WidthRequest = 35,
                    Padding = 5,
                    BackgroundColor = Color.FromHex(objInfo.BackColorLight),
                };
                ImageButtonOptin.Clicked += ImageButtonOptin_Clicked;

                ImageBtnStar = new ImageButton
                {

                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 35,
                    Padding = 5,
                    BackgroundColor = Color.FromHex(objInfo.BackColorLight),
                };
                if (objInfo.SatrCheck)
                {
                    ImageBtnStar.Source = "StarWhiteFilled.png";
                }
                else
                {
                    ImageBtnStar.Source = "StarWhiteUn.png";
                }

                ImageBtnStar.Clicked += ImageBtnStar_Clicked;

                Gridlayoutlist.Children.Add(labelID);
                Gridlayoutlist.Children.Add(labelList, 2, 0);
                Gridlayoutlist.Children.Add(ImageBtnStar, 1, 0);
                Gridlayoutlist.Children.Add(ImageButtonOptin, 3, 0); ;
                stackLayoutList.Children.Add(Gridlayoutlist);
                stackLayoutList.GestureRecognizers.Add(tapGestureRecognizer);
                FrameLayoutList.Content = stackLayoutList;
                stackLayout.Children.Add(FrameLayoutList);
                iIndex++;
            }
            return stackLayout;
        }
        #endregion listing all shopping lists

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var StackLayoutSender = (StackLayout)sender;
           
            if (StackLayoutSender.BackgroundColor != Color.AliceBlue && !ListClicked)
            {
                ListClicked = true;
                StackLayoutSender.BackgroundColor = Color.AliceBlue;

                var listStackChildren = StackLayoutSender.Children;
                var GridListChild = listStackChildren[0] as Grid;


                var GridListChildren = GridListChild.Children;
                var IDLabel = GridListChildren[0];
                Label labelListID = (Label)IDLabel;
                string labelID = labelListID.Text;
                var labelListName = GridListChildren[1];
                Label labelListNames = (Label)labelListName;
                string labelNames = labelListNames.Text;

                var ImageBtnStar = GridListChildren[2];
                ImageButton ImageBtnSta = (ImageButton)ImageBtnStar;
                ImageBtnSta.BackgroundColor= Color.AliceBlue;
                var ImageButtonOptin = GridListChildren[3];
                ImageButton ImageButtonOpti = (ImageButton)ImageButtonOptin;
                ImageButtonOpti.BackgroundColor = Color.AliceBlue;

                StaticShoppingListsVar.Id = labelID;
                StaticShoppingListsVar.ListName = labelNames;

                Navigation.PushModalAsync(new NavigationPage(new DefaultUserShoppingList()));
            }
        }
      

        public bool IsTableAllShoppingListsExists()
        {
            try
            {
                var db = new SQLiteConnection(dbPath);
                var tableInfo = db.GetTableInfo("ShoppingList");
                if (tableInfo.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private bool IsTableAllUserShoppingListsExists()
        {
            try
            {
                var db = new SQLiteConnection(dbPath);
                var tableInfo = db.GetTableInfo("AllUserShoppingList");
                if (tableInfo.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private void BtnCreShpList_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new CrteShopigList());
        }

        private void ImBtnCreShpList_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new CrteShopigList());
        }

        private void ImageButtonOptin_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var grid = button.Parent as Grid;
            var StackLayoutSender = grid.Parent as StackLayout;
            var listStackChildren = StackLayoutSender.Children;
            var GridListChild = listStackChildren[0] as Grid;
            var GridListChildren = GridListChild.Children;
            var IDLabel = GridListChildren[0];
            Label labelListID = (Label)IDLabel;
            string labelID = labelListID.Text;
            var labelListName = GridListChildren[1];
            Label labelListNames = (Label)labelListName;
            string labelNames = labelListNames.Text;
            StaticShoppingListsVar.Id = labelID;
            StaticShoppingListsVar.ListName = labelNames;
            PopupNavigation.Instance.PushAsync(new ShopOptnsPop());
        }

        private void ImageBtnStar_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var grid = button.Parent as Grid;
            var StackLayoutSender = grid.Parent as StackLayout;
            var listStackChildren = StackLayoutSender.Children;
            var GridListChild = listStackChildren[0] as Grid;
            var GridListChildren = GridListChild.Children;
            var IDLabel = GridListChildren[0];
            Label labelListID = (Label)IDLabel;
            int labelID = Convert.ToInt32(labelListID.Text);
            var labelListName = GridListChildren[1];
            Label labelListNames = (Label)labelListName;
            string labelNames = labelListNames.Text;

            var db = new SQLiteConnection(dbPath);
            var ShoppingLists = db.Table<AllShoppingLists>().Where(i => i.Id == labelID && i.ListName == labelNames).FirstOrDefault();
            var maxPK = db.Table<AllShoppingLists>().OrderByDescending(c => c.SatrCheckID).FirstOrDefault();
            if (!ShoppingLists.SatrCheck)
            {
                ShoppingLists.SatrCheck = true;
                ShoppingLists.SatrCheckID = (maxPK == null ? 1 : maxPK.SatrCheckID + 1);
                string color = m_randomColor.ChangeColorBrightness(ShoppingLists.BackColorLight);
                ShoppingLists.BackColorLight = color;
                ShoppingLists.FontColorLight = "#FFFFFF";//white
                ShoppingLists.FontColorDark = "#000000";//black;
            }
            else
            {

                ShoppingLists.SatrCheck = false;
                ShoppingLists.SatrCheckID = 0;
                ShoppingLists.BackColorLight = m_randomColor.RandomColorPicker(ShoppingLists.Id);
                ShoppingLists.FontColorLight = "#000000";//black
                ShoppingLists.FontColorDark = "#FFFFFF";//white;

            }
            db.Update(ShoppingLists);
            this.Content = this.BuildView();
        }
    }
}