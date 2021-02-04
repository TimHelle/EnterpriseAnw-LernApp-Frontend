using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Model.Database
{
    [Table("Frage")]
    class DAOFrage
    {
        [PrimaryKey, Column("id"), NotNull, AutoIncrement]
        public uint? Id { get; set; }
        public Boolean userCreated { get; set; }
        [MaxLength(255), NotNull]
        public string text { get; set; }
        public string explanation { get; set; }
        public string hash { get; set; }
        [Ignore]
        public static uint? lastId { get; set; } = 0;
        public DAOFrage()
        {
            this.Id = lastId+1;           
        }
        public DAOFrage(Boolean countUp)
        {

        }
    }
}
