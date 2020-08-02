using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Drawing;

namespace ALits.Models
{
    [Table("ShoppingList")]
    class AllShoppingLists
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(25)]
        public string ListName { get; set; }
        public string BackColor { get; set; }
        public string BackColorLight { get; set; }
        public string BackColorDark { get; set; }
        public string FontColor { get; set; }
        public string FontColorLight { get; set; }
        public string FontColorDark { get; set; }
        public bool SatrCheck { get; set; }
        public int SatrCheckID { get; set; }

    }

    [Table("AllUserShoppingList")]
    class AllUserShoppingLists
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int ListID { get; set; }
        [NotNull]
        public string ListName { get; set; }
        [NotNull, MaxLength(25)]
        public string ItemName { get; set; }

        [MaxLength(30)]
        public string ItemDesc { get; set; }

        public double ItemPrice { get; set; }
        public string ItemBackColor { get; set; }
        public string ItemBackColorLight { get; set; }

        public string ItemBackColorDark { get; set; }
        public bool DoneCheck { get; set; }
        public int DoneCheckID { get; set; }
        public string ItemFontColor { get; set; }
        public string ItemFontColorLight { get; set; }
        public string ItemFontColorDark { get; set; }       

    }

    //static varibles for code
   public class StaticShoppingListsVar
    {
        public static string Id { get; set; }
        public static string ListName { get; set; }
    }

}



