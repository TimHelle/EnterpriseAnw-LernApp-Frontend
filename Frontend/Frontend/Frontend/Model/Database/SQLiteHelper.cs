using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text.Json;

namespace App.Model.Database
{
    class SQLiteHelper
    {
        public static readonly string dbFileName = "LearningAppData.sqlite";
        public static readonly string pathToDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbFileName);

        private static readonly string UrlToBackend = "http://51.137.215.185:9000/api/";

        public SQLiteHelper() { }
        #region stuff for settings
        public void initializeSavedSettings()
        {
            var db = new SQLiteConnection(pathToDb);
            db.CreateTable<DAOSetting>();
            if (db.Table<DAOSetting>().Count() == 0)
            {
                //adding default values of settings
                DAOSetting urlSetting = new DAOSetting("urlToBackend", UrlToBackend);
                db.Insert(urlSetting);
            }
            db.Close();
        }

        public void saveSetting(String key, String value)
        {
            var db = new SQLiteConnection(pathToDb);
            DAOSetting setting = new DAOSetting(key, value);
            db.Update(setting);
            db.Close();
        }
        #endregion
        public void initializeSQLiteDatabase()
        {
            var db = new SQLiteConnection(pathToDb);
            //create tables if not already existing
            db.CreateTable<DAOFrage>(SQLite.CreateFlags.AutoIncPK | SQLite.CreateFlags.ImplicitPK);
            db.CreateTable<DAOAntwort>(SQLite.CreateFlags.AutoIncPK | SQLite.CreateFlags.ImplicitPK);
            db.CreateTable<DAOKategorie>(SQLite.CreateFlags.AutoIncPK | SQLite.CreateFlags.ImplicitPK);
            db.CreateTable<DAOFrageAntwort>(SQLite.CreateFlags.AutoIncPK | SQLite.CreateFlags.ImplicitPK);
            db.CreateTable<DAOFrageKategorie>(SQLite.CreateFlags.AutoIncPK | SQLite.CreateFlags.ImplicitPK);
            db.Close();
            setFragenList();
        }
        public void dropAllTables()
        {
            var db = new SQLiteConnection(pathToDb);
            //dropping all tables
            db.DropTable<DAOFrage>();
            db.DropTable<DAOAntwort>();
            db.DropTable<DAOKategorie>();
            db.DropTable<DAOFrageAntwort>();
            db.DropTable<DAOFrageKategorie>();
            db.Close();
        }

        public static Collection<Fragen> getAllFragen()
        {
            Collection<Fragen> fragen = new Collection<Fragen>();
            var db = new SQLiteConnection(pathToDb);

            var tableOfFragen = db.Table<DAOFrage>();
            var tableOfAntworten = db.Table<DAOAntwort>();
            var tableOfFrageAntwort = db.Table<DAOFrageAntwort>();
            var tableOfFrageKategorie = db.Table<DAOFrageKategorie>();
            var tableOfKategorien = db.Table<DAOKategorie>();

            foreach (DAOFrage daoFrage in tableOfFragen)
            {
                Fragen newFrage = new Fragen();
                //set id erstmal ausgelassen
                newFrage.setText(daoFrage.text);
                newFrage.setErklärung(daoFrage.explanation);
                newFrage.setId(daoFrage.Id);
                //add Antwort
                foreach (DAOAntwort daoAntwort in tableOfAntworten)
                {
                    foreach (DAOFrageAntwort fa in tableOfFrageAntwort)
                    {
                        if (fa.FID == daoFrage.Id && fa.AID == daoAntwort.Id)
                        {
                            Antworten newAntwort = new Antworten(daoAntwort.text, daoAntwort.isCorrect);
                            newFrage.setAntworten(newAntwort);
                        }

                        if (fa.id > DAOFrageAntwort.lastId)
                        {
                            DAOFrageAntwort.lastId = fa.id;
                        }

                    }

                    if (daoAntwort.Id > DAOAntwort.lastId)
                    {
                        DAOAntwort.lastId = daoAntwort.Id;
                    }

                }

                //add Kategorie
                foreach (DAOKategorie daoKategorie in tableOfKategorien)
                {
                    foreach (DAOFrageKategorie fk in tableOfFrageKategorie)
                    {
                        if (fk.FID == daoFrage.Id && fk.KID == daoKategorie.Id)
                        {
                            Kategorien newKategorie = new Kategorien(daoKategorie.title, daoKategorie.description);
                            newFrage.setKategorie(newKategorie);
                        }

                        if (fk.id > DAOFrageKategorie.lastId)
                        {
                            DAOFrageKategorie.lastId = fk.id;
                        }

                    }

                    if (daoKategorie.Id > DAOKategorie.lastId)
                    {
                        DAOKategorie.lastId = daoKategorie.Id;
                    }

                }

                if (newFrage.getId() > DAOFrage.lastId)
                {
                    DAOFrage.lastId = newFrage.getId();
                }

                fragen.Add(newFrage);
            }
            return fragen;
        }

        public Collection<Kategorien> getAllKategorien()
        {
            Collection<Kategorien> kategorien = new Collection<Kategorien>();
            var db = new SQLiteConnection(pathToDb);

            var tableOfKategorien = db.Table<DAOKategorie>();

            foreach (DAOKategorie daoKategorie in tableOfKategorien)
            {
                Kategorien newKategorie = new Kategorien(daoKategorie.title, daoKategorie.description);
                newKategorie.id = daoKategorie.Id;
                kategorien.Add(newKategorie);
            }

            return kategorien;
        }

        public List<BenutzerFrage> getAllUserCreated()
        {
            List<BenutzerFrage> fragen = new List<BenutzerFrage>();

            var db = new SQLiteConnection(pathToDb);

            var tableOfFragen = db.Table<DAOFrage>();
            var tableOfAntworten = db.Table<DAOAntwort>();
            var tableOfFrageAntwort = db.Table<DAOFrageAntwort>();
            var tableOfFrageKategorie = db.Table<DAOFrageKategorie>();
            var tableOfKategorien = db.Table<DAOKategorie>();

            foreach (DAOFrage daoFrage in tableOfFragen)
            {
                if(daoFrage.userCreated)
                {
                    BenutzerFrage benutzerFrage = new BenutzerFrage();
                    benutzerFrage.text = daoFrage.text;
                    benutzerFrage.explanation = daoFrage.explanation;
                    benutzerFrage.hash = daoFrage.hash;


                    //add Antwort
                    foreach (DAOAntwort daoAntwort in tableOfAntworten)
                    {
                        foreach (DAOFrageAntwort fa in tableOfFrageAntwort)
                        {
                            if (fa.FID == daoFrage.Id && fa.AID == daoAntwort.Id)
                            {
                                Antworten newAntwort = new Antworten(daoAntwort.text, daoAntwort.isCorrect);
                                benutzerFrage.addAntwort(newAntwort);
                            }

                            if (fa.id > DAOFrageAntwort.lastId)
                            {
                                DAOFrageAntwort.lastId = fa.id;
                            }

                        }

                        if (daoAntwort.Id > DAOAntwort.lastId)
                        {
                            DAOAntwort.lastId = daoAntwort.Id;
                        }

                    }

                    //add Kategorie
                    foreach (DAOKategorie daoKategorie in tableOfKategorien)
                    {
                        foreach (DAOFrageKategorie fk in tableOfFrageKategorie)
                        {
                            if (fk.FID == daoFrage.Id && fk.KID == daoKategorie.Id)
                            {
                                Kategorien newKategorie = new Kategorien(daoKategorie.title, daoKategorie.description);
                                benutzerFrage.setKategorie(newKategorie);
                            }

                            if (fk.id > DAOFrageKategorie.lastId)
                            {
                                DAOFrageKategorie.lastId = fk.id;
                            }

                        }

                        if (daoKategorie.Id > DAOKategorie.lastId)
                        {
                            DAOKategorie.lastId = daoKategorie.Id;
                        }

                    }

                    //add here
                    fragen.Add(benutzerFrage);
                }

            }
            return fragen;   
        }

        public void wipeDataAndFillFromRemote()
        {
            //get all user created
            List<BenutzerFrage> benutzerFragen = getAllUserCreated();
            //
            var db = new SQLiteConnection(pathToDb);
            this.dropAllTables();
            this.initializeSQLiteDatabase();

            string url = UrlToBackend;

            var tableSettings = db.Table<DAOSetting>();
            foreach (DAOSetting s in tableSettings)
            {
                if (s.key.Equals("urlToBackend"))
                {
                    url = s.value;
                }
            }

            String jsonFragen = "[]";
            String jsonKategorien = "[]";

            try
            {
                jsonFragen = (new WebClient()).DownloadString(url + "questions");
                jsonKategorien = (new WebClient()).DownloadString(url + "categories");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            JsonDocument jsonDocFragen = JsonDocument.Parse(jsonFragen);
            JsonDocument jsonDocKategorien = JsonDocument.Parse(jsonKategorien);

            foreach (JsonElement element in jsonDocFragen.RootElement.EnumerateArray())
            {
                DAOFrage nFrage = new DAOFrage();
                nFrage.explanation = element.GetProperty("explanation").GetString();
                //nFrage.explanation = "not yet implemented";
                nFrage.Id = element.GetProperty("id").GetUInt32();
                nFrage.text = element.GetProperty("text").GetString();
                try
                {
                    //var kategorien = element.GetProperty("category").EnumerateArray();
                    //foreach (JsonElement katEl in kategorien)
                    //{
                    //    DAOFrageKategorie daoKat = new DAOFrageKategorie();
                    //    daoKat.FID = nFrage.Id;
                    //    //daoKat.KID = katEl.GetUInt32();
                    //    daoKat.KID = katEl.GetProperty("id").GetUInt32();
                    //    db.Insert(daoKat);
                    //}

                    var kategorie = element.GetProperty("category");
                    var kid = kategorie.GetProperty("id").GetUInt32();
                    DAOFrageKategorie daoKat = new DAOFrageKategorie();
                    daoKat.FID = nFrage.Id;
                    daoKat.KID = kid;
                    db.Insert(daoKat);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                var antworten = element.GetProperty("answers").EnumerateArray();
                foreach (JsonElement antEl in antworten)
                {
                    DAOAntwort antwort = new DAOAntwort();
                    antwort.Id = antEl.GetProperty("id").GetUInt32();
                    antwort.text = antEl.GetProperty("text").GetString();
                    antwort.isCorrect = antEl.GetProperty("isCorrect").GetBoolean();
                    db.Insert(antwort);

                    DAOFrageAntwort frAnt = new DAOFrageAntwort();
                    frAnt.FID = nFrage.Id;
                    frAnt.AID = antwort.Id;
                    db.Insert(frAnt);
                }
                db.Insert(nFrage);
            }

            foreach (JsonElement element in jsonDocKategorien.RootElement.EnumerateArray())
            {
                DAOKategorie nKategorie = new DAOKategorie();
                nKategorie.Id = element.GetProperty("id").GetUInt32();
                nKategorie.title = element.GetProperty("title").GetString();
                nKategorie.description = element.GetProperty("description").GetString();

                try {
                db.Insert(nKategorie);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            db.Close();

            //now add old user questions and stuff
            foreach(BenutzerFrage benFrage in benutzerFragen)
            {
                Boolean toBeAdded = true;
                foreach(Fragen frage in getAllFragen())
                {
                    if(frage.hash == benFrage.hash)
                    {
                        toBeAdded = false;
                    }
                }
                if (toBeAdded)
                {
                    AddFrageToDatabase(benFrage.toFrage());
                }
            }

            setFragenList();
        }

        public void AddCategoryToDatabase(Kategorien kategorien)
        {
            DAOKategorie dAOKategorie = new DAOKategorie();
            var db = new SQLiteConnection(pathToDb);
            dAOKategorie.title = kategorien.titel;
            dAOKategorie.userCreated = kategorien.userCreated;
            dAOKategorie.hash = kategorien.hash;
            dAOKategorie.description = kategorien.beschreibung;
            db.Insert(dAOKategorie);
        }
        public void AddFrageToDatabase(Fragen frage)
        {
            DAOFrage dAOFrage = new DAOFrage();
            var db = new SQLiteConnection(pathToDb);
            dAOFrage.userCreated = frage.userCreated;
            dAOFrage.hash = frage.hash;
            dAOFrage.text = frage.getText();
            foreach (Antworten antwort in frage.getAntwort())
            {
                DAOAntwort dAOAntwort = new DAOAntwort();
                dAOAntwort.text = antwort.getText();
                dAOAntwort.isCorrect = antwort.getStatus();
                db.Insert(dAOAntwort);
                DAOFrageAntwort dAOFrageAntwort = new DAOFrageAntwort();
                dAOFrageAntwort.AID = dAOAntwort.Id;
                dAOFrageAntwort.FID = dAOFrage.Id;
                db.Insert(dAOFrageAntwort);
            }
            foreach (Kategorien kategorie in frage.getKategorie())
            {
                bool check = false;
                uint? i = 0;
                foreach (Kategorien dbKategorie in this.getAllKategorien())
                {
                    if (dbKategorie.titel.Equals(kategorie.titel))
                    {
                        check = true;
                        i = dbKategorie.id;
                    }
                }
                if (check == false)
                {
                    DAOKategorie dAOKategorie = new DAOKategorie();
                    dAOKategorie.title = kategorie.titel;
                    dAOKategorie.description = kategorie.beschreibung;
                    db.Insert(dAOKategorie);
                    i = dAOKategorie.Id;
                }
                DAOFrageKategorie dAOFrageKategorie = new DAOFrageKategorie();
                dAOFrageKategorie.KID = i;
                dAOFrageKategorie.FID = dAOFrage.Id;
                db.Insert(dAOFrageKategorie);
            }
            db.Insert(dAOFrage);
            setFragenList();
        }

        public void setFragenList()
        {
            Fragen.fragen.Clear();
            Fragen.fragen = getAllFragen();
            Kategorien.kategorien.Clear();
            Kategorien.kategorien = this.getAllKategorien();
        }

        public static uint? getLastUsedId(String tablename)
        {
            Collection<Fragen> fragen = getAllFragen();
            uint? temp = 0;
            foreach (Fragen frage in fragen)
            {
                if (frage.getId() > temp)
                {
                    temp = frage.getId();
                }
            }
            return temp;
            //var db = new SQLiteConnection(pathToDb);
            //SQLiteCommand cmd = new SQLiteCommand(db);
            //cmd.CommandText = "select last_insert_rowid() as id from " + tablename;
            //var temp = cmd.ExecuteScalar<long>();
            //db.Close();
            //return Convert.ToUInt32(temp);
        }
        //Anlegen von Daten
        //Löschen von Daten
        //mit Daten aus remote db füllen // an remote db senden // synchronisieren

    }
}
