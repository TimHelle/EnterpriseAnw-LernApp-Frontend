using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Model.Database
{
    [Table("FrageKategorie")]
    class DAOFrageKategorie
    {
        [PrimaryKey, Column("id"), AutoIncrement, NotNull]
        public uint? id { get; set; }
        //[NotNull]
        //public uint? FID { get; set; }
        //[NotNull]
        //public uint? KID { get; set; }
        //[NotNull]
        public string Fhash { get; set; }
        //[NotNull]
        public string Khash { get; set; }
        [Ignore]
        public static uint? lastId { get; set; } = 0;
        public DAOFrageKategorie()
        {
            this.id = lastId+1;
        }
    }
}
