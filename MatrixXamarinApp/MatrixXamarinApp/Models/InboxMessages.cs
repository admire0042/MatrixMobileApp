using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixXamarinApp.Models
{
   public class InboxMessages
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string subject { get; set; }
        public int number { get; set; }
        public int direction { get; set; }
    }
}