using System;
using System.Collections.Generic;
using System.Text;

namespace OrderHanteringsSystem
{
    class Produkt
    {
        string ProduktCode;
        string ProduktNamn;
        double ProduktPris;
        string ProduktFilnamn;

        public List<string> FieldNamn;
        public List<string> FieldNamnDisplay;
        List<string> FieldValue;

        FilHanterare filHanterare;
        Input myinput;
        public Produkt()
        {
            ProduktFilnamn = "produkt.txt";
            filHanterare = new FilHanterare(Utilities.PathNamn, ProduktFilnamn);
            myinput = new Input();

            FieldNamn = new List<string>();
            FieldNamnDisplay = new List<string>();

            FieldNamn.Add("ProduktCode:");
            FieldNamnDisplay.Add("Produkt Code");

            FieldNamn.Add("ProduktNamn:");
            FieldNamnDisplay.Add("Produkt Namn");

            FieldNamn.Add("ProduktPris:");
            FieldNamnDisplay.Add("Produkt Pris");

            FieldValue = new List<string>();
        }
        /// <summary>
        /// Skapa produkt
        /// </summary>

        public void AddProdukt()
        {
            bool addanotherprodukt = true;
            do
            {
                do
                {
                    Utilities.WriteLog("Ange produkt code:");
                    this.ProduktCode = Console.ReadLine();
                    this.ProduktCode = this.ProduktCode.ToUpper();

                } while (Utilities.ValidateText(this.ProduktCode,4, 4) == "");//Loopa tills användaren anger rätt värde

                List<string> datalist = filHanterare.FetchAllLines(); //Hämta alla produkter

                string item = Utilities.FetchItem(datalist, 
                                        Utilities.BuildSearchString(FieldNamn[0], this.ProduktCode));

                if (!string.IsNullOrEmpty(item))////Kolla Produkt finns redan
                {
                    Utilities.WriteErrorLog("Produkt finns redan.");
                    Utilities.ViewRow(FieldNamnDisplay, item);
                }
                else
                {
                    FieldValue.Add(this.ProduktCode);
                    do
                    {
                        Utilities.WriteLog("Ange produkt namn:");
                        this.ProduktNamn = Console.ReadLine();

                    } while (Utilities.ValidateText(this.ProduktNamn,1,20) == "");
                    FieldValue.Add(this.ProduktNamn);//Loopa tills användaren anger rätt värde

                    do
                    {
                        Utilities.WriteLog("Ange produkt pris:");
                        this.ProduktPris = Utilities.ValidateDouble(Console.ReadLine());
                    } while (this.ProduktPris == 0);
                    FieldValue.Add(this.ProduktPris.ToString());//Loopa tills användaren anger rätt värde

                    AppendProdukt(Utilities.BuildString(FieldNamn, FieldValue), "Produkt tillagd framgångsrikt.");//Skriva ut fil ny produkt
                    FieldValue.Clear();
                    string str = "";
                    
                    do
                    {
                        str = myinput.UserInput("Vill du ange en annan produkt J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            addanotherprodukt = true;
                        }
                        else if (str == "N")
                        {
                            addanotherprodukt = false;
                            Utilities.ConsoleClear();
                        }
                    } while ((str != "J") && (str != "N"));
                }
            } while (addanotherprodukt == true);//loop tills användaren vill skapa inte en annan produkt
        }
        /// <summary>
        /// Ta bort produkt
        /// </summary>
        public void DeleteProdukt()
        {
            if (filHanterare.FileExists() == false)
            {
                Utilities.WriteErrorLog("Inga produkter att ta bort.");
                Utilities.WriteErrorLogOchContinue();
            }
            else
            {
                bool deleteanotherprodukt = true;
                List<string> rowtodelete = new List<string>();
                do
                {
                    do
                    {
                        
                        Utilities.WriteLog("Ange produkt code:");
                        this.ProduktCode = Console.ReadLine();
                        this.ProduktCode = this.ProduktCode.ToUpper();
                    } while ((Utilities.ValidateText(this.ProduktCode,4, 4) == ""));

                    if (!ProduktExistIBeordra(Utilities.BuildSearchString(this.FieldNamn[0], this.ProduktCode) + Utilities.DELIMETER))//kolla om produkt finns int i beordra 
                    {
                        FieldValue.Add(this.ProduktCode);

                        rowtodelete.Add(Utilities.BuildSearchString(FieldNamn[0], this.ProduktCode));
                        Utilities.DeleteLineFromFile(filHanterare, rowtodelete, "Produkt Ta bort framgångsrikt.", " finns inte.");
                    }

                    FieldValue.Clear();
                    rowtodelete.Clear();
                    string str = "";
                    do
                    {
                        str = myinput.UserInput("Vill du ta bort en annan produkt J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            deleteanotherprodukt = true;
                        }
                        else if (str == "N")
                        {
                            deleteanotherprodukt = false;
                            Utilities.ConsoleClear();
                        }
                    } while ((str != "J") && (str != "N"));

                } while (deleteanotherprodukt == true);//loop tills användaren vill ta bort inte en annan produkt
            }
        }        
        /// <summary>
        /// Skriva ut produkt
        /// </summary>
        public void ViewProdukt()
        {
            string str = "";
            bool viewanotherprodukt = true;
            if (filHanterare.FileExists() == false)
            {
                Utilities.WriteErrorLog("Inga produkter att se.");
                Utilities.WriteErrorLogOchContinue();
            }
            else
            {
                string item;
                List<string> rowtoview = new List<string>();
                do
                {
                    do
                    {
                        Utilities.WriteLog("Ange produkt code:");
                        this.ProduktCode = Console.ReadLine();
                        this.ProduktCode = this.ProduktCode.ToUpper();
                    } while (Utilities.ValidateText(this.ProduktCode,4, 4) == "");

                    List<string> datalist = filHanterare.FetchAllLines();
                    item = Utilities.FetchItem(datalist, Utilities.BuildSearchString(FieldNamn[0], this.ProduktCode));

                    if (!string.IsNullOrEmpty(item)) ////Kolla om produkt finns
                        Utilities.ViewRow(FieldNamnDisplay, item);
                    else
                        Utilities.WriteLineLog("{0} {1} {2}", FieldNamnDisplay[0], this.ProduktCode, "finns inte.");

                    do
                    {
                        str = myinput.UserInput("Vill du se en annan produkt J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            viewanotherprodukt = true;
                        }
                        else if (str == "N")
                        {
                            viewanotherprodukt = false;
                            Utilities.ConsoleClear();
                        }
                    } while ((str != "J") && (str != "N"));
                } while (viewanotherprodukt == true);//loop tills användaren vill se inte en annan produkt
            }
        }
        /// <summary>
        /// Skriva ut till produkt fil
        /// </summary>
        /// <param name="text"></param>
        /// <param name="msgtouser"></param>
        private void AppendProdukt(string text, string msgtouser)
        {
            filHanterare.AppendFile(text, msgtouser);
        }
        /// <summary>
        /// Ändra produkt information
        /// </summary>
        public void ModifyProdukt()
        {
            bool modifiedprodukt = false;

            if (filHanterare.FileExists() == false)
            {
                Utilities.WriteErrorLog("Inga produkter att ändra.");
                Utilities.WriteErrorLogOchContinue();
            }
            else
            {
                bool modifyanotherprodukt = true;
                string str1;
                string item;
                List<string> rowtomodify = new List<string>();
                do
                {
                    do
                    {
                        Utilities.WriteLog("Ange produkt code:");
                        this.ProduktCode = Console.ReadLine();
                        this.ProduktCode = this.ProduktCode.ToUpper();
                    } while (Utilities.ValidateText(this.ProduktCode,4, 4) == "");

                    List<string> datalist = filHanterare.FetchAllLines();
                    item = Utilities.FetchItem(datalist, Utilities.BuildSearchString(FieldNamn[0], this.ProduktCode));

                    string[] itemarray = item.Split(Utilities.DELIMETER);
                    string[] fieldarray;

                    if (!string.IsNullOrEmpty(item)) ////Kolla produkt finns -hämta data
                    {
                        FieldValue.Add(this.ProduktCode);

                        fieldarray = itemarray[1].Split(':');//produkt Name:proukt 1;
                        this.ProduktNamn = fieldarray[1];

                        fieldarray = itemarray[2].Split(':');//produkt pris:1;
                        double.TryParse(fieldarray[1], out this.ProduktPris);

                        //Kolla användaren om de vill ändra produktnamn
                        do
                        {
                            str1 = myinput.UserInput("Vill du ändra produktnamnet J/N?");
                            str1 = Utilities.CheckUserJN(str1);
                            if (str1 == "J")
                            {
                                do
                                {
                                    Utilities.WriteLog("Ange produkt namn:");
                                    this.ProduktNamn = Console.ReadLine();
                                    modifiedprodukt = true;
                                } while (Utilities.ValidateText(this.ProduktNamn,1,20) == "");
                            }

                        } while ((str1 != "J") && (str1 != "N"));
                        FieldValue.Add(this.ProduktNamn);

                        //Kolla användaren om de vill ändra produktpris
                        do
                        {
                            str1 = myinput.UserInput("Vill du ändra produkt pris J/N?");
                            str1 = Utilities.CheckUserJN(str1);
                            if (str1 == "J")
                            {
                                do
                                {
                                    Utilities.WriteLog("Ange produkt pris:");
                                    this.ProduktPris = Utilities.ValidateDouble(Console.ReadLine());
                                    modifiedprodukt = true;
                                } while (this.ProduktPris == 0);
                            }
                        } while ((str1 != "J") && (str1 != "N"));
                        FieldValue.Add(this.ProduktPris.ToString());

                        //Om användaren ändra produktnamn eller produktpris då ändra fil

                        if (modifiedprodukt)
                        {
                            rowtomodify.Add(Utilities.BuildSearchString(FieldNamn[0], this.ProduktCode));
                            Utilities.DeleteLineFromFile(filHanterare, rowtomodify, "", "");

                            AppendProdukt(Utilities.BuildString(FieldNamn, FieldValue), "Produkten modifierades framgångsrikt.");
                        }
                        modifiedprodukt = false;
                    }
                    else
                    {
                        Utilities.WriteLineLog("Produkten finns inte.");
                    }
                    FieldValue.Clear();
                    rowtomodify.Clear();
                    string str = "";
                    do
                    {
                        str = myinput.UserInput("Vill du ändra en annan produkt J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            modifyanotherprodukt = true;
                        }
                        else if (str == "N")
                        {
                            modifyanotherprodukt = false;
                            Utilities.ConsoleClear();
                        }
                    } while ((str != "J") && (str != "N"));
                } while (modifyanotherprodukt == true);
            }
        }
        /// <summary>
        /// Skriv ut alla produkt
        /// </summary>
        public void ViewAll()
        {
            if (filHanterare.FileExists() == false)
            {
                Utilities.WriteErrorLog("Inga produkter att se.");
                Utilities.WriteErrorLogOchContinue();
            }
            else
            {
                Utilities.ViewAll(FieldNamnDisplay, filHanterare.FetchAllLines());
            }
        }
        /// <summary>
        /// Skriv ut alla produkt till listan
        /// </summary>
        public List<string> PopulateProduct()
        {
            if (filHanterare.FileExists())
                return filHanterare.FetchAllLines();

            return new List<string>();
        }
        /// <summary>
        /// Hämta pris för produkten
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public double GetPrice(string item)
        {
            string[] itemarray;
            string[] fieldarray;
            itemarray = item.Split(Utilities.DELIMETER);

            fieldarray = itemarray[2].Split(':');

            double.TryParse(fieldarray[1], out double value);
            return value;
        }
        /// <summary>
        /// Hämta namn för produkten
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GetProductName(string item)
        {
            string[] itemarray;
            string[] fieldarray;
            itemarray = item.Split(Utilities.DELIMETER);

            fieldarray = itemarray[1].Split(':');

            return fieldarray[1];
        }
        /// <summary>
        /// Kolla finns i beordra
        /// </summary>
        /// <param name="datacheck"></param>
        /// <returns></returns>
        private bool ProduktExistIBeordra(string datacheck)
        {
            FilHanterare beordrafilHanterare = new FilHanterare(Utilities.PathNamn, "beordra.txt");
           
            if (!string.IsNullOrEmpty (Utilities.FetchItem(beordrafilHanterare.FetchAllLines(), datacheck).ToString()))
            {
                Utilities.WriteErrorLog("Produkt finns i beordra.");
                    return true;
            }
            return false;

        }
    }
}
