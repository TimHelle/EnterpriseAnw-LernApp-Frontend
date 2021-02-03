using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace App
{
    public class Kategorien
    {
        public uint? id { get; set; }
        public string titel { get; set; }
        public string hash { get; set; }
        public bool userCreated { get; set; }
        public string beschreibung { get; set; }
        public bool selected { get; set; }
        public static Collection<Kategorien> auswahl { get; set; } = new Collection<Kategorien>();
        public static Collection<Kategorien> kategorien { get; set; } = new Collection<Kategorien>();
        public Kategorien () { }
        public Kategorien(string titel, string beschreibung)
        {
            this.titel = titel;
            this.beschreibung = beschreibung;
        }
    }
}
