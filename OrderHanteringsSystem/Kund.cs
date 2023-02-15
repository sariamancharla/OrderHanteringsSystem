using System;
using System.Collections.Generic;
using System.Text;

namespace OrderHanteringsSystem
{
    class kund
    {
        string KundCode;
        string KundNamn;
        string KundAdress;
        string KundTelefon;
        string KundFilnamn;
        
        public List<string> FieldNamn;
        List<string> FieldValue;
        public List<string> FieldNamnDisplay;

        FilHanterare filHanterare;
        Input myinput;

        public kund()
        {
            KundFilnamn = "kund.txt";
            filHanterare = new FilHanterare(Utilities.PathNamn, KundFilnamn);
            myinput = new Input();

            FieldNamn=new List<string>();
            FieldNamnDisplay = new List<string>();

            FieldNamn.Add("KundCode:");
            FieldNamnDisplay.Add("Kund Code");

            FieldNamn.Add("KundNamn:");
            FieldNamnDisplay.Add("Kund Namn");

            FieldNamn.Add("KundAdress:");
            FieldNamnDisplay.Add("Kund Adress");

            FieldNamn.Add("KundtTelefon:");
            FieldNamnDisplay.Add("Kund Telefon");

            FieldValue = new List<string>();
        }
        /// <summary>
        /// Skapa Kund
        /// </summary>
        public void AddKund()
        {
            bool addanotherkund = true;
            do
            {
                do
                {
                    Utilities.WriteLog("Ange kund code:");
                    this.KundCode = Console.ReadLine();
                    this.KundCode = this.KundCode.ToUpper();
                } while (Utilities.ValidateText(this.KundCode,4, 4) == "");//Loopa tills användaren anger rätt värde
                //validatio for already Exits                
                List<string> datalist = filHanterare.FetchAllLines();//Hämta alla kunder

                string item =Utilities.FetchItem(datalist, Utilities.BuildSearchString(FieldNamn[0], this.KundCode));
                
                if (!string.IsNullOrEmpty(item)) ////Kolla kund finns redan
                {
                    Utilities.WriteErrorLog("Kund redan finns.");
                    Utilities.ViewRow(FieldNamnDisplay, item);
                }
                else
                {
                    FieldValue.Add(this.KundCode);
                    do
                    {
                        Utilities.WriteLog("Ange kund namn:");
                        this.KundNamn = Console.ReadLine();

                    } while (Utilities.ValidateText(this.KundNamn,1,20) == "");//Loopa tills användaren anger rätt värde
                    FieldValue.Add(this.KundNamn);

                    do
                    {
                        Utilities.WriteLog("Ange kund adress:");
                        this.KundAdress = Console.ReadLine();
                    } while (string.IsNullOrEmpty(this.KundAdress));//Loopa tills användaren anger rätt värde
                    FieldValue.Add(this.KundAdress.ToString());

                    do
                    {
                        Utilities.WriteLog("Ange kund telefon:");
                        this.KundTelefon = Console.ReadLine();
                    } while (Utilities.ValidateText(this.KundTelefon, 10, 13) == "");//Loopa tills användaren anger rätt värde
                    FieldValue.Add(this.KundTelefon);

                    AppendKund(Utilities.BuildString(FieldNamn, FieldValue), "Kund tillagd framgångsrikt.");//Skriva ut fil ny kund

                    FieldValue.Clear();
                    string str = "";
                    do
                    {
                        str = myinput.UserInput("Vill du ange en annan kund J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            addanotherkund = true;
                        }
                        else if (str == "N")
                        {
                            addanotherkund = false;
                            Utilities.ConsoleClear();
                        }
                    } while ((str != "J") && (str != "N"));
                }
            } while (addanotherkund==true);//loop tills användaren vill skapa inte en annan kund

        }
        /// <summary>
        /// Ta bort Kund
        /// </summary>
        public void DeleteKund()
        {
            if (filHanterare.FileExists() == false)
            {
                Utilities.WriteErrorLog("No kund att ta bort.");
                Utilities.WriteErrorLogOchContinue();
            }
            else
            {
                bool deleteanotherkund = true;
                List<string> rowtodelete = new List<string>();
                do
                {
                    do
                    {
                        Utilities.WriteLog("Ange kund code:");
                        this.KundCode = Console.ReadLine();
                        this.KundCode = this.KundCode.ToUpper();

                    } while (Utilities.ValidateText(this.KundCode,4, 4) == "");
                    FieldValue.Add(this.KundCode);

                    if (!ProduktExistIBeordra(Utilities.BuildSearchString(this.FieldNamn[0], this.KundCode) + Utilities.DELIMETER))//kolla om kund finns int i beordra 
                    {
                        FieldValue.Add(this.KundCode);

                        rowtodelete.Add(Utilities.BuildSearchString(FieldNamn[0], this.KundCode));
                        Utilities.DeleteLineFromFile(filHanterare, rowtodelete, "Kund borttagen framgångsrikt.", " Hittades inte att ta bort.");
                    }
                    

                    FieldValue.Clear();
                    rowtodelete.Clear();
                    string str = "";
                    do
                    {
                        str = myinput.UserInput("Vill du ta bort en annan kund J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            deleteanotherkund = true;
                        }
                        else if (str == "N")
                        {
                            deleteanotherkund = false;
                            Utilities.ConsoleClear();
                        }
                    } while ((str != "J") && (str != "N"));

                } while (deleteanotherkund == true);//loop tills användaren vill ta bort inte en annan produkt
            }
        }
        /// <summary>
        /// Skriva ut till kund fil
        /// </summary>
        /// <param name="text"></param>
        /// <param name="msgtouser"></param>
        private void AppendKund(string text,string msgtouser)
        {
            filHanterare.AppendFile(text, msgtouser);
        }
        /// <summary>
        /// Ändra kund information
        /// </summary>
        public void ModifyKund()
        {

            if (filHanterare.FileExists() == false)
            {
                Utilities.WriteErrorLog("Ingen kund att ändra.");
                Utilities.WriteErrorLogOchContinue();
            }
            else
            {
                bool modifyanotherkund = true;
                bool modifiedkund = false;
                string str1 ;
                string item;                
                List<string> rowtomodify = new List<string>();
                do
                {
                    do
                    {
                        Utilities.WriteLog("Ange kund code:");
                        this.KundCode = Console.ReadLine();
                        this.KundCode = this.KundCode.ToUpper();

                    } while (Utilities.ValidateText(this.KundCode,4, 4) == "");

                    List<string> datalist = filHanterare.FetchAllLines();
                    
                    //Hämta kund data
                    item = Utilities.FetchItem(datalist, Utilities.BuildSearchString(FieldNamn[0], this.KundCode));

                    string[] itemarray = item.Split(Utilities.DELIMETER);
                    string[] fieldarray;

                    if (!string.IsNullOrEmpty(item)) //kund finns
                    {
                        FieldValue.Add(this.KundCode);

                        fieldarray = itemarray[1].Split(':');
                        this.KundNamn = fieldarray[1];

                        fieldarray = itemarray[2].Split(':');
                        this.KundAdress = fieldarray[1];

                        fieldarray = itemarray[3].Split(':');
                        this.KundTelefon=fieldarray[1];

                        //Kolla användaren om de vill ändra kundnamn

                        do
                        {
                            str1 = myinput.UserInput("Vill du ändra kundnamn J/N?");
                            str1 = Utilities.CheckUserJN(str1);
                            if (str1 == "J")
                            {
                                do
                                {
                                    Utilities.WriteLog("Ange kund namn:");
                                    this.KundNamn = Console.ReadLine();
                                    modifiedkund = true;
                                } while (Utilities.ValidateText(this.KundNamn,1,20) == "");
                            }
                            
                        } while ((str1 != "J") && (str1 != "N"));
                        FieldValue.Add(this.KundNamn);

                        //Kolla användaren om de vill ändra kundadress
                        do
                        {
                            str1 = myinput.UserInput("Vill du ändra kundadress J/N?");
                            str1 = Utilities.CheckUserJN(str1);
                            if (str1 == "J")
                            {
                                do
                                {
                                    Utilities.WriteLog("Ange kund adress:");
                                    this.KundAdress = Console.ReadLine();
                                    modifiedkund = true;
                                } while (string.IsNullOrEmpty(this.KundAdress));
                            }                            
                        } while ((str1 != "J") && (str1 != "N"));
                        FieldValue.Add(this.KundAdress);

                        //Kolla användaren om de vill ändra kundtelefon
                        do
                        {
                            str1 = myinput.UserInput("Vill du ändra kundtelefon J/N?");
                            str1 = Utilities.CheckUserJN(str1);
                            if (str1 == "J")
                            {
                                do
                                {
                                    Utilities.WriteLog("Ange kund telefon:");
                                    this.KundTelefon =Console.ReadLine();
                                    modifiedkund = true;
                                } while (Utilities.ValidateText(this.KundTelefon, 10, 13) == "");
                            }
                        } while ((str1 != "J") && (str1 != "N"));
                        FieldValue.Add(this.KundTelefon);

                        //Om användaren ändra produktnamn eller produktpris då ändra fil
                        if (modifiedkund)
                        {
                            rowtomodify.Add(Utilities.BuildSearchString(FieldNamn[0], this.KundCode));
                            Utilities.DeleteLineFromFile(filHanterare, rowtomodify, "", "");

                            AppendKund(Utilities.BuildString(FieldNamn, FieldValue), "Kund modifierades framgångsrikt.");
                        }
                        modifiedkund = false;
                    }
                    else
                    {
                        Utilities.WriteLineLog("Kunden kunde inte ändra.");
                    }
                    FieldValue.Clear();
                    rowtomodify.Clear();
                    string str = "";
                    do
                    {
                        str = myinput.UserInput("Vill du ändra en annan kund J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            modifyanotherkund = true;
                        }
                        else if (str == "N")
                        {
                            modifyanotherkund = false;
                            Utilities.ConsoleClear();
                        }
                    } while ((str != "J") && (str != "N"));
                } while (modifyanotherkund == true);
            }
        }
        /// <summary>
        /// Skriva ut kund
        /// </summary>
        public void ViewKund()
        {
            string str = "";
            bool viewanotherkund = true;
            if (filHanterare.FileExists() == false)
            {
                Utilities.WriteErrorLog("Ingen kund att se.");
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
                        Utilities.WriteLog("Ange kund code:");
                        this.KundCode = Console.ReadLine();
                        this.KundCode = this.KundCode.ToUpper();

                    } while (Utilities.ValidateText(this.KundCode,4, 4) == "");

                    List<string> datalist = filHanterare.FetchAllLines();

                    //Hämta kund data
                    item = Utilities.FetchItem(datalist, Utilities.BuildSearchString(FieldNamn[0], this.KundCode));

                    if (!string.IsNullOrEmpty(item))
                        Utilities.ViewRow(FieldNamnDisplay, item);
                    else
                        Utilities.WriteLineLog("{0} {1} {2}", FieldNamnDisplay[0], this.KundCode, "finns inte.");
                    do
                    {
                        str = myinput.UserInput("Vill du se en annan kund J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            viewanotherkund = true;
                        }
                        else if (str == "N")
                        {
                            viewanotherkund = false;
                            Utilities.ConsoleClear();
                        }
                    } while ((str != "J") && (str != "N"));
                } while (viewanotherkund == true);//loop tills användaren vill se inte en annan kund
            }
        }
        /// <summary>
        /// Skriv ut alla kund
        /// </summary>
        public void ViewAll()
        {
            if (filHanterare.FileExists() == false)
            {
                Utilities.WriteErrorLog("Ingen kund att se.");
                Utilities.WriteErrorLogOchContinue();
            }
            else
            {
                Utilities.ViewAll(FieldNamnDisplay, filHanterare.FetchAllLines());
            }
        }
        /// <summary>
        /// Skriv ut alla kund till listan
        /// </summary>
        public List<string> PopulateKund()
        {
            return filHanterare.FetchAllLines();
        }
        /// <summary>
        /// Kolla finns i beordra
        /// </summary>
        /// <param name="datacheck"></param>
        /// <returns></returns>
        private bool ProduktExistIBeordra(string datacheck)
        {
            FilHanterare beordrafilHanterare = new FilHanterare(Utilities.PathNamn, "beordra.txt");

            if (!string.IsNullOrEmpty(Utilities.FetchItem(beordrafilHanterare.FetchAllLines(), datacheck).ToString()))
            {
                Utilities.WriteErrorLog("Kund finns i beordra.");
                return true;
            }
            return false;

        }

    }
    
}
