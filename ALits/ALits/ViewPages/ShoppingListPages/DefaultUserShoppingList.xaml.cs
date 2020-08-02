using ALits.DesingsClass;
using ALits.Models;
using ALits.ViewPages.OptionPoppages;
using ALits.ViewPages.ShoppingListPages.AddPopUps;
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

namespace ALits.ViewPages.ShoppingListPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DefaultUserShoppingList : ContentPage
    {
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "AListDb");
        private StackLayout stackLayout;
        private Button BtnAddItem;
        private Label labelID;
        private Label labelItemname;
        private Label labelItemDesc;
        private Label labelItemDate;
        private Label labelItemCost;
        private ImageButton ImageBtnCheck;
        private StackLayout stackLayoutTotal;
        private StackLayout stackLayoutList;
        private int m_strId;
        private string m_strListName;
        private bool BtnAddItemClicked;
        private string listHeading;
        private ImageButton ImBtnAddItem;
        private Frame FrameLayoutList;
        private Grid Gridlayoutlist;
        private Frame FrameBlankSpace;

        private RandomColor m_randomColor;
        private ImageButton ImageButtonOptin;
        private ImageButton ImageButtonAddCost;

        public string ListNameHeading
        {
            get { return listHeading; }
            set
            {
                listHeading = value;
                OnPropertyChanged(nameof(ListNameHeading)); // Notify that there was a change on this property
            }
        }
        public DefaultUserShoppingList()
        {
            InitializeComponent();
            m_randomColor = new RandomColor();
            m_strId = Convert.ToInt32(StaticShoppingListsVar.Id);
            m_strListName = StaticShoppingListsVar.ListName;
            this.Content = this.BuildView();

            ListNameHeading = m_strListName;
            BindingContext = this;
            ListNameHeading = ListNameHeading;

            BtnAddItemClicked = false;

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnUserShoppingListReload", (sender) =>
            {
                BtnAddItemClicked = false;//for evade the physical back button prob
                this.Content = this.BuildView();
            });
        }


        private View BuildView()
        {
            var db = new SQLiteConnection(dbPath);

            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = 70 });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            stackLayoutTotal = new StackLayout
            {
                HeightRequest = 70,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill,
                Margin = new Thickness(0, 0, 0, 2),
                BackgroundColor = Color.FromHex("#2196F3")
            };

            stackLayout = new StackLayout();
            List<AllUserShoppingLists> objAllUserShoppingLists = new List<AllUserShoppingLists>();
            objAllUserShoppingLists = db.Table<AllUserShoppingLists>().Where(i => i.ListID == m_strId && i.ListName == m_strListName).ToList();

            var objAllUserShoppingList= objAllUserShoppingLists.OrderBy(x => x.DoneCheckID).ToList();

            stackLayout = BuildListView(objAllUserShoppingList);           

            BtnAddItem = new Button();
            BtnAddItem.Text = "Add";
            BtnAddItem.Clicked += BtnAddItem_Clicked;
            BtnAddItem.BorderColor = System.Drawing.Color.FromArgb(43, 60, 60);
            BtnAddItem.BorderWidth = 1;
            BtnAddItem.CornerRadius = 35;
            BtnAddItem.HorizontalOptions = LayoutOptions.End;
            BtnAddItem.VerticalOptions = LayoutOptions.End;
            BtnAddItem.FontAttributes = FontAttributes.Bold;
            BtnAddItem.BackgroundColor = System.Drawing.Color.FromArgb(75, 168, 98);
            BtnAddItem.TextColor = Color.White;
            BtnAddItem.WidthRequest = 160;
            BtnAddItem.HeightRequest = 70;
            BtnAddItem.Margin = 20;

            ImBtnAddItem = new ImageButton()
            {
                Source = "AddImgWhite.png",
                Padding = 7,
                Margin = new Thickness(4, 2, 60, 30),
            };
            ImBtnAddItem.BorderWidth = 1;
            ImBtnAddItem.CornerRadius = 35;
            ImBtnAddItem.HorizontalOptions = LayoutOptions.End;
            ImBtnAddItem.VerticalOptions = LayoutOptions.End;
            ImBtnAddItem.BackgroundColor = System.Drawing.Color.FromArgb(75, 168, 98);
            ImBtnAddItem.WidthRequest = 70;
            ImBtnAddItem.HeightRequest = 70;
            ImBtnAddItem.Clicked += ImBtnAddItem_Clicked;

            ScrollView scrollView = new ScrollView
            {
                VerticalOptions = LayoutOptions.Fill,
            };
            scrollView.Content = stackLayout;

            grid.Children.Add(stackLayoutTotal, 0, 0);
            grid.Children.Add(scrollView, 0, 1);
            grid.Children.Add(ImBtnAddItem, 0, 1);           
            return grid;
        }

        private StackLayout BuildListView(List<AllUserShoppingLists> objAllUserShoppingLists)
        {
            double Fontsize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            double Fontsizesmall = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            double FontsizeMicro = Device.GetNamedSize(NamedSize.Micro, typeof(Label));



            int iIndex = 1;
            foreach (AllUserShoppingLists objInfo in objAllUserShoppingLists)
            {
                //objInfo.ItemBackColor = objInfo.ItemBackColorLight;
                FrameLayoutList = new Frame
                {
                    HeightRequest = 110,
                    Padding = new Thickness(0, 5, 0, 0),
                    CornerRadius = 4,
                    Margin = new Thickness(4, 2, 4, 2),

                };
                FrameLayoutList.BackgroundColor = Color.FromHex(objInfo.ItemBackColor);


                Gridlayoutlist = new Grid
                {
                    Margin = 0,
                    Padding = 0,
                    //BackgroundColor=Color.Yellow
                };
                Gridlayoutlist.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                Gridlayoutlist.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                Gridlayoutlist.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });
                Gridlayoutlist.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Gridlayoutlist.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
                Gridlayoutlist.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Gridlayoutlist.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                stackLayoutList = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Fill,
                    Padding = 0,
                    Margin = 0
                };

                labelID = new Label
                {
                    Text = (objInfo.Id).ToString(),
                    IsVisible = false
                };

                labelItemname = new Label
                {
                    Text = objInfo.ItemName,
                    Margin = new Thickness(5, 0, 0, 0),
                    FontSize = Fontsize,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center,
                };

                labelItemDesc = new Label
                {
                    Text = objInfo.ItemDesc,
                    FontSize = Fontsizesmall,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(5, 0, 0, 0),
                };

                labelItemDate = new Label
                {
                    Text = "completed on 12.0.12 10 15 am",
                    FontSize = Fontsizesmall,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(5, 0, 0, 0),
                };

                labelItemCost = new Label
                {
                    Text = "completed on 12.0.12 10 15 am",
                    FontSize = FontsizeMicro,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(4, 0, 0, 0),
                };

                ImageBtnCheck = new ImageButton
                {

                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 40,
                    Padding = 5,
                    BackgroundColor = Color.Transparent,
                };
                if (objInfo.DoneCheck)
                {
                    ImageBtnCheck.Source = "StarWhiteFilled.png";
                    labelItemname.TextColor = Color.FromHex(objInfo.ItemFontColorLight);
                    labelItemDesc.TextColor = Color.FromHex(objInfo.ItemFontColorLight);
                    labelItemDate.TextColor = Color.FromHex(objInfo.ItemFontColorLight);
                }
                else
                {
                    ImageBtnCheck.Source = "StarWhiteUn.png";
                    labelItemname.TextColor = Color.FromHex(objInfo.ItemFontColorDark);
                    labelItemDesc.TextColor = Color.FromHex(objInfo.ItemFontColorDark);
                    labelItemDate.TextColor = Color.FromHex(objInfo.ItemFontColorDark);
                }
                ImageBtnCheck.Clicked += ImageBtnCheck_Clicked;

                ImageButtonAddCost = new ImageButton
                {
                    Source = "OptWhite.png",
                    HorizontalOptions = LayoutOptions.End,
                    WidthRequest = 40,
                    Padding = 5,
                    BackgroundColor = Color.FromHex(objInfo.ItemBackColor),
                };
                ImageButtonAddCost.Clicked += ImageButtonAddCost_Clicked;

                ImageButtonOptin = new ImageButton
                {
                    Source = "OptWhite.png",
                    HorizontalOptions = LayoutOptions.End,
                    WidthRequest = 40,
                    Padding = 5,
                    BackgroundColor = Color.FromHex(objInfo.ItemBackColor),
                };
                ImageButtonOptin.Clicked += ImageButtonOptin_Clicked;

                Gridlayoutlist.Children.Add(labelID);
                Gridlayoutlist.Children.Add(ImageBtnCheck, 0, 1, 0, 3);

                Gridlayoutlist.Children.Add(labelItemname, 1, 0);
                Gridlayoutlist.Children.Add(labelItemDesc, 1, 1);
                Gridlayoutlist.Children.Add(labelItemCost, 1, 2);
                
                Gridlayoutlist.Children.Add(ImageButtonAddCost, 0, 3, 0,3 );
                Gridlayoutlist.Children.Add(ImageButtonOptin, 0, 4, 0, 3);
                stackLayoutList.Children.Add(Gridlayoutlist);
                FrameLayoutList.Content = stackLayoutList;
                stackLayout.Children.Add(FrameLayoutList);
                iIndex++;
            }
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
            return stackLayout;
        }

        private void ImageButtonAddCost_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

        //fix neded
        private void ImageBtnCheck_Clicked(object sender, EventArgs e)
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
            var labelItemName = GridListChildren[2];
            Label labelitemtNames = (Label)labelItemName;
            string labelNames = labelitemtNames.Text;

            var db = new SQLiteConnection(dbPath);
            var ShoppingLists = db.Table<AllUserShoppingLists>().Where(i => i.Id == labelID && i.ItemName == labelNames).FirstOrDefault();
            var maxPK = db.Table<AllUserShoppingLists>().OrderBy(c => c.DoneCheck).FirstOrDefault();
            if (ShoppingLists.DoneCheck)
            {
                ShoppingLists.DoneCheck = false;
                ShoppingLists.DoneCheckID = 0;
                ShoppingLists.ItemBackColor = m_randomColor.RandomColorPicker(ShoppingLists.Id);
                ShoppingLists.ItemFontColorDark = "#000000";//black
                ShoppingLists.ItemFontColorLight = "#FFFFFF";//white;
            }
            else
            {
                ShoppingLists.DoneCheck = true;
                ShoppingLists.DoneCheckID = (maxPK == null ? 1 : maxPK.DoneCheckID + 1);
                string color = m_randomColor.ChangeColorBrightness(ShoppingLists.ItemBackColor);
                ShoppingLists.ItemBackColor = color;
                ShoppingLists.ItemFontColorLight = "#FFFFFF";//white
                ShoppingLists.ItemFontColorDark = "#000000";//black;


            }
            db.Update(ShoppingLists);
            this.Content = this.BuildView();
        }

        private void ImBtnAddItem_Clicked(object sender, EventArgs e)
        {
            if (!BtnAddItemClicked)
            {
                BtnAddItemClicked = true;
                PopupNavigation.Instance.PushAsync(new DfltShngAdditem());
            }
        }

        private void BtnAddItem_Clicked(object sender, EventArgs e)
        {
            BtnAddItemClicked = true;
            PopupNavigation.Instance.PushAsync(new DfltShngAdditem());
        }

        public bool IsTableExists()
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

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<App>((App)Application.Current, "OnShoppingListReload");
            Navigation.PopModalAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            if (BtnAddItemClicked)
            {
                PopupNavigation.Instance.PopAsync();
            }
            MessagingCenter.Send<App>((App)Application.Current, "OnShoppingListReload");
            Navigation.PopModalAsync();           
            return true;
        }

    }
}