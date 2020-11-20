using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixXamarinApp.Models
{
    public class ViewsModel
    {
        [PrimaryKey]
        public int ViewID { get; set; }
        public string ViewName { get; set; }

        public bool IsSelected { get; set; } = false;
    }


    public class ViewsModel2
    {

        [PrimaryKey]
        public int ViewID { get; set; }
        public string ViewName { get; set; }
        public bool IsSelected { get; set; } = false;
    }

}
