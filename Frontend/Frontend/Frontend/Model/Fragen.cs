using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace App
{
    public class Fragen
    {
        private uint? id;
        public String text { get; set; }
        private String erklärung;
        private Collection<Antworten> antwort = new Collection<Antworten>();
        private Collection<Kategorien> kategorie = new Collection<Kategorien>();

        public string hash { get; set; }
        public bool userCreated { get; set; }

        public static Collection<Fragen> fragen { get; set; } = new Collection<Fragen>();

        public uint? getId()
        {
            return this.id;
        }
        public void setId(uint? id)
        {
            this.id = id;
        }
        public String getText()
        {
            return this.text;
        }
        public void setText(String text)
        {
            this.text = text;
        }
        public String getErklärung()
        {
            return this.erklärung;
        }
        public void setErklärung(String erklärung)
        {
            this.erklärung = erklärung;
        }
        public Collection<Antworten> getAntwort()
        {
            return this.antwort;
        }
        public void setAntworten(Antworten a)
        {
            this.antwort.Add(a);
        }
        public Collection<Kategorien> getKategorie()
        {
            return this.kategorie;
        }
        public void setKategorie(Kategorien k)
        {
            this.kategorie.Add(k);
        }
    }
}
