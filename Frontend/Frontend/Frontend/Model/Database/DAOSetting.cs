using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Model.Database
{
    [Table("Settings")]
    class DAOSetting
    {
        [PrimaryKey, NotNull, Unique]
        public string key { get; set; }
        public string value { get; set; }

        public DAOSetting() { }

        public DAOSetting(String key, String value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
