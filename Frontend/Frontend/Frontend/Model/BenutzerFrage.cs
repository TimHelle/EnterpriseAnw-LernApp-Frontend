using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace App
{
    class BenutzerFrage
    {
        private class smallKategorie
        {
            public String titel { get; set; }
            public String beschreibung { get; set; }
            public String hash { get; set; }
        }

        private class smallAntwort
        {
            public String text { get; set; }
            public Boolean isCorrect { get; set; }
        }

        private String text { get; set; }
        private String erklärung { get; set; }

        private Collection<smallAntwort> antworten = new Collection<smallAntwort>();
        private smallKategorie kategorie;

        private String hash { get; set; }

        public void addAntwort(Antworten antwort)
        {
            smallAntwort nAntwort = new smallAntwort();
            nAntwort.text = antwort.getText();
            nAntwort.isCorrect = antwort.getStatus();
            antworten.Add(nAntwort);
        }

        public void setKategorie(Kategorien kat)
        {
            smallKategorie nKat = new smallKategorie();

            nKat.titel = kat.titel;
            nKat.beschreibung = kat.beschreibung;
            //TODO : 
            //nKat.hash = kat.hash

            kategorie = nKat;
        }
    }
}
