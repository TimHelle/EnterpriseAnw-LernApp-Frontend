using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace App
{
    class BenutzerFrage
    {
        public class smallKategorie
        {
            public String title { get; set; }
            public String description { get; set; }
            public String hash { get; set; }
        }

        public class smallAntwort
        {
            public String text { get; set; }
            public Boolean isCorrect { get; set; }
        }

        public BenutzerFrage()
        {
            answers = new List<smallAntwort>();
        }

        public String text { get; set; }
        public String explanation { get; set; }

        //public List<smallAntwort> answers = new List<smallAntwort>();
        public List<smallAntwort> answers { get; }
        public smallKategorie category { get; set; }

        public String hash { get; set; }

        public void addAntwort(Antworten antwort)
        {
            smallAntwort nAntwort = new smallAntwort();
            nAntwort.text = antwort.getText();
            nAntwort.isCorrect = antwort.getStatus();
            answers.Add(nAntwort);
        }

        public void setKategorie(Kategorien kat)
        {
            smallKategorie nKat = new smallKategorie();

            nKat.title = kat.titel;
            nKat.description = kat.beschreibung;
            //TODO : 
            //nKat.hash = kat.hash

            category = nKat;
        }

        public Kategorien getKategorie()
        {
            Kategorien nKategorie = new Kategorien();
            nKategorie.titel = this.category.title;
            nKategorie.userCreated = true;
            nKategorie.hash = this.category.hash;
            nKategorie.beschreibung = this.category.description;
            return nKategorie;
        }

        public Fragen toFrage()
        {
            Fragen nFrage = new Fragen();
            nFrage.setText(this.text);
            nFrage.setErklärung(this.explanation);
            nFrage.setKategorie(this.getKategorie());
            nFrage.userCreated = true;
            nFrage.hash = this.hash;

            foreach(smallAntwort smAnt in answers)
            {
                Antworten antwort = new Antworten();
                antwort.setText(smAnt.text);
                antwort.setStatus(smAnt.isCorrect);
                nFrage.setAntworten(antwort);
            }

            return nFrage;
        }

    }
}
