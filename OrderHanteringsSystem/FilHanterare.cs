using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OrderHanteringsSystem
{
    class FilHanterare
    {
        public string FilNamn;
        string FilPath;
        public FilHanterare() { }
        /// <summary>
        /// konstruktör med path och filnamn
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filnamn"></param>
        public FilHanterare(string path,string filnamn)
        {
            this.FilPath = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()) + path;
            
            DirCreate();////Skapar katalog
            this.FilNamn=this.FilPath + "\\"+ filnamn;
        }
        /// <summary>
        /// konstruktör med hela path och filnamn
        /// </summary>
        /// <param name="pathfilnamn"></param>
        public FilHanterare(string pathfilnamn)
        {
            this.FilPath = Path.GetDirectoryName(pathfilnamn);// Returnerar sökvägen

            DirCreate();////Skapar katalog
            this.FilNamn = pathfilnamn;
        }
        /// <summary>
        /// Returnerar bool om fil finns
        /// </summary>
        /// <returns></returns>
        public bool FileExists()
        {            
            return File.Exists(FilNamn);
        }
        /// <summary>
        /// Skapar fil och skriva till textfil eller skriva över textfil
        /// </summary>
        public void UpdateFile(string text)
        {
            if (!FileExists())
            {
                DirCreate();                
            }
            using (StreamWriter streamwriter = File.CreateText(this.FilNamn))
            {
                streamwriter.WriteLine(text);//skriva till file
            };
        }
        /// <summary>
        /// Ta bort alla rader som finns och returnera listan
        /// </summary>
        public List<string> DeleteAllMatchingRecords( string deleteText, string message, string failuremessage = "")
        {
            string textinfile = string.Empty;
            List<string> Excludedlist = new List<string>();
            bool recordexists = false;

            if (FileExists())
            {
                using (StreamReader streamreader = File.OpenText(FilNamn))
                {
                    //loopar igenom fil
                    while ((textinfile = streamreader.ReadLine()) != null)
                    {
                        if (!textinfile.Contains(deleteText))
                        {
                            Excludedlist.Add(textinfile);
                        }
                        else
                        {
                            recordexists = true;
                        }
                    }
                }
                if ((recordexists == false) && (!string.IsNullOrEmpty(failuremessage)))
                {
                    Utilities.WriteLineLog(failuremessage);
                }
                else if (recordexists==true)
                {
                    Utilities.WriteLineLog(message);
                }
            }
            return Excludedlist;
        }
        /// <summary>
        /// läser hela fil och returnerar textar
        /// </summary>
        public string ReadFileText()
        {
            if (!FileExists())
            {
                
                return string.Empty;
            }
            else
            {                
                return File.ReadAllText(this.FilNamn);
            }
        }
        /// <summary>
        /// skriver in en ström av data till en fil
        /// </summary>
        /// <param name="text"></param>
        public void AppendFile(string text,string msgtouser="")
        {
            if (!FileExists())
            {
                DirCreate();////Skapar katalog 
            }
            using (StreamWriter streamwriter = File.AppendText(FilNamn))
            {
                streamwriter.WriteLine(text);//skriva till file
            }
            Utilities.WriteLineLog(msgtouser);
        }
        /// <summary>
        /// Tar bort fil
        /// </summary>
        public void DeleteFile()
        {
            if (!FileExists())
            {
                Utilities.WriteLineLog("\nFil {0} finns inte.\n", this.FilNamn);
            }
            else
            {
                File.Delete(this.FilNamn);
                Utilities.WriteLineLog("\nFil {0} har tagits bort.\n", this.FilNamn);
            }
        }
        /// <summary>
        /// Skapa katalog om finns inte
        /// </summary>
        private void DirCreate()
        {
            if (!Directory.Exists(this.FilPath))
            {
                Directory.CreateDirectory(this.FilPath);
            }
        }
        /// <summary>
        /// Fil data to listan
        /// </summary>
        /// <returns></returns>
        public List<string> FetchAllLines()
        {
            if (FileExists())
                return File.ReadAllLines(this.FilNamn).ToList();

            return new List<string>();
        }
        /// <summary>
        /// Array data to fil
        /// </summary>
        /// <param name="datalist"></param>
        public void WriteArrayDataToFile(List<string> datalist)
        {
            File.WriteAllLines(this.FilNamn, datalist.ToArray());
        }

        /// <summary>
        /// TaBort fil produkt.txt,kund.txt,beordra.txt,lastbeordraid.txt
        /// </summary>
        public void TaBortAllaFiler()
        {
            FilHanterare produktfil = new FilHanterare(Utilities.PathNamn, "produkt.txt");
            FilHanterare kundfil = new FilHanterare(Utilities.PathNamn, "kund.txt");
            FilHanterare beordrafil = new FilHanterare(Utilities.PathNamn, "beordra.txt");
            FilHanterare lastbeordraid = new FilHanterare(Utilities.PathNamn, "lastbeordraid.txt");
            produktfil.DeleteFile();
            kundfil.DeleteFile();
            beordrafil.DeleteFile();
            lastbeordraid.DeleteFile();
        }
    }
}
