using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Model.Database
{
    [Table("Antwort")]
    class DAOAntwort
    {
        [PrimaryKey, Column("id"), NotNull, AutoIncrement]
        public uint? Id { get; set; }
        public Boolean userCreated { get; set; }
        [MaxLength(255), NotNull]
        public string text { get; set; }
        [NotNull]
        public Boolean isCorrect { get; set; }
        [Ignore]
        public static uint? lastId { get; set; } = 0;
        public DAOAntwort()
        {
            this.Id = lastId+1;
        }
    }
}
