using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Model.Database
{
    [Table("Kategorie")]
    class DAOKategorie
    {
        [PrimaryKey, Column("id"), NotNull, AutoIncrement]
        public uint? Id { get; set; }
        [MaxLength(255)]
        public string description { get; set; }
        [MaxLength(128), Unique]
        public string title { get; set; }
        [Ignore]
        public static uint? lastId { get; set; } = 0;
        public DAOKategorie()
        {
            this.Id = lastId+1;
        }
    }
}
