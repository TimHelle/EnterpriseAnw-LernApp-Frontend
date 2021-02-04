using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Model.Database
{
    [Table("FrageAntwort")]
    class DAOFrageAntwort
    {
        [PrimaryKey, Column("id"), AutoIncrement]
        public uint? id { get; set; }
        [NotNull]
        public uint? FID { get; set; }
        [NotNull]
        public uint? AID { get; set; }
        //[NotNull]
        //public string Fhash { get; set; }
        //[NotNull]
        //public string Ahash { get; set; }
        [Ignore]
        public static uint? lastId { get; set; } = 0;
        public DAOFrageAntwort()
        {
            this.id = lastId+1;
        }
    }
}
