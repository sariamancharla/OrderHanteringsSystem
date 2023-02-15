using System;
using System.Collections.Generic;
using System.Text;

namespace OrderHanteringsSystem
{
    class Beordra
    {
        int BeordraId;
        DateTime BeordraDt;
        string ProduktCode;
        string KundCode;
        double ProduktQty;
        double ProduktPris;
        string BeordraFilnamn;
        string BeordraIdFilnamn;

        List<string> FieldNamn;
        List<string> FieldNamnDisplay;
        List<string> FieldValue;

        FilHanterare filHanterare;
        FilHanterare beordraidfilHanterare;
        Input myinput;

        Produkt produkt;
        kund kund;
        List<string> ProduktList;
        List<string> KundList;

        public Beordra()
        {
            BeordraIdFilnamn= "lastbeordraid.txt";
            beordraidfilHanterare = new FilHanterare(Utilities.PathNamn, BeordraIdFilnamn);

            BeordraFilnamn = "beordra.txt";
            filHanterare = new FilHanterare(Utilities.PathNamn, BeordraFilnamn);
            myinput = new Input();

            FieldNamn=new List<string>();
            FieldNamnDisplay = new List<string>();

            FieldNamn.Add("BeordraId:");
            FieldNamnDisplay.Add("Order Id");

            FieldNamn.Add("BeordraDatum:");
            FieldNamnDisplay.Add("Beordra Datum");

            FieldNamn.Add("KundCode:");
            FieldNamnDisplay.Add("Kund Code");

            FieldNamn.Add("ProduktCode:");
            FieldNamnDisplay.Add("Produkt Code");

            FieldNamn.Add("ProduktPris:");
            FieldNamnDisplay.Add("Produkt Pris");

            FieldNamn.Add("ProduktAntal:");
            FieldNamnDisplay.Add("Produkt Antal");           

            FieldValue = new List<string>();

            produkt = new Produkt();
            kund = new kund();
            ProduktList = produkt.PopulateProduct();
            KundList = kund.PopulateKund();
        }
        /// <summary>
        /// Skapa Beordra
        /// </summary>
        public void AddBeordra()
        {
            bool addanotherbeordra = true;
            bool addanotherprodukt = true;
            string item = string.Empty;
            string str = string.Empty;
            List<string> orderlist = new List<string>();
            List<string> viewitem= new List<string>();
            if (ProduktList.Count == 0 || KundList.Count == 0)//Tillåt att skapa order endast om produkt och kund finns
            {
                Utilities.WriteErrorLog("Produkt eller Kund finns inte.");
                Utilities.WriteErrorLogOchContinue();
            }
            else
            {
                do
                {
                    this.BeordraId = Utilities.NextID(beordraidfilHanterare); //Skapa nästa ID
                    this.BeordraDt = DateTime.Now.Date;

                    //Loopa tills användaren anger rätt kund
                    do
                    {
                        Utilities.WriteLog("Ange kund code:");
                        this.KundCode = Console.ReadLine();
                        this.KundCode = this.KundCode.ToUpper();
                        //Kolla om kund finns
                        item = Utilities.FetchItem(KundList, Utilities.BuildSearchString(kund.FieldNamn[0], this.KundCode) + Utilities.DELIMETER);
                        if (string.IsNullOrEmpty(item))
                        {
                            Utilities.WriteErrorLog("Kund finns inte.");
                        }
                        else
                        {
                            Utilities.BreakLine('-', 25);
                            Utilities.ViewRow(kund.FieldNamnDisplay, item);
                            Utilities.BreakLine('-', 25);
                        }
                    } while (string.IsNullOrEmpty(item));

                    //loop tills användaren vill skapa inte en annan produkt
                    do
                    {   
                        //Loopa tills användaren anger rätt produkt
                        do
                        {
                            Utilities.WriteLog("Ange produkt code:");
                            this.ProduktCode = Console.ReadLine();
                            this.ProduktCode = this.ProduktCode.ToUpper();

                            //Kolla om produkt finns
                            item = Utilities.FetchItem(ProduktList, Utilities.BuildSearchString(produkt.FieldNamn[0], this.ProduktCode) + Utilities.DELIMETER);
                            if (string.IsNullOrEmpty(item))
                            {
                                Utilities.WriteErrorLog("Produkt finns inte.");
                            }
                            else
                            {
                                string orderkundprod = string.Format("{0};{1};{2};{3}",
                                                Utilities.BuildSearchString(FieldNamn[0], this.BeordraId.ToString()),
                                                Utilities.BuildSearchString(FieldNamn[1], this.BeordraDt.ToShortDateString()),
                                                Utilities.BuildSearchString(FieldNamn[2], this.KundCode),
                                                Utilities.BuildSearchString(FieldNamn[3], this.ProduktCode));

                                string isProductExist = Utilities.FetchItem(orderlist, orderkundprod);
                                if (!string.IsNullOrEmpty(isProductExist)) //Kolla om beordra id+datum+kund+Produkt finns redan
                                {
                                    item = null;
                                    Utilities.WriteErrorLog("Denna produkt har redan lagts till i denna beordra.");
                                }
                                else
                                {

                                    Utilities.BreakLine('-', 25);
                                    Utilities.ViewRow(produkt.FieldNamnDisplay, item);//skriva ut produkt info
                                    Utilities.BreakLine('-', 25);
                                }
                            }
                        } while (string.IsNullOrEmpty(item));

                        FieldValue.Add(this.BeordraId.ToString());
                        FieldValue.Add(this.BeordraDt.ToShortDateString());
                        FieldValue.Add(this.KundCode);
                        FieldValue.Add(this.ProduktCode);

                        //Produkt pris
                        this.ProduktPris = produkt.GetPrice(item);
                        FieldValue.Add(this.ProduktPris.ToString());

                        //Produkt Antal
                        do
                        {
                            Utilities.WriteLog("Ange produkt antal:");
                            this.ProduktQty = Utilities.ValidateInt(Console.ReadLine());
                        } while (this.ProduktQty == 0);
                        FieldValue.Add(this.ProduktQty.ToString());

                        //Skapa beordra till fil
                        AppendBeordra(Utilities.BuildString(FieldNamn, FieldValue), "Produkt tillagd framgångsrikt till beordra. Beordra Id : "+ this.BeordraId.ToString());

                        orderlist.Add(Utilities.BuildString(FieldNamn, FieldValue));
                        FieldValue.Clear();
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
                            }
                        } while ((str != "J") && (str != "N"));
                    } while (addanotherprodukt == true);

                    UpdateMaxBeordraId(this.BeordraId.ToString());//Updatera id fil

                    //Skriva ut beordra
                    List<string> datalist = filHanterare.FetchAllLines();
                    viewitem = Utilities.FetchItemList(datalist, Utilities.BuildSearchString(FieldNamn[0], this.BeordraId.ToString()));
                    if (viewitem.Count > 0)
                    {
                        DisplayBeordra(viewitem);
                    }
                    
                    do
                    {
                        str = myinput.UserInput("Vill du ange en annan beordra J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            Utilities.ConsoleClear();
                            addanotherbeordra = true;
                        }
                        else if (str == "N")
                        {
                            addanotherbeordra = false;
                            Utilities.ConsoleClear();
                        }
                    } while ((str != "J") && (str != "N"));
                } while (addanotherbeordra == true);//loop tills användaren vill skapa inte en annan beordra
            }
        }
        /// <summary>
        /// Ta bort beordra
        /// </summary>
        public void DeleteBeordra()
        {
            if (!filHanterare.FileExists())
            {
                Utilities.WriteErrorLog("Ingen beordra att ta bort.");
                Utilities.WriteErrorLogOchContinue();
            }
            else
            {
                bool deleteanotherBeordra = true;
                do
                {
                    do
                    {
                        Utilities.WriteLog("Ange beordra id:");
                        this.BeordraId = Utilities.ValidateInt(Console.ReadLine()); ;

                    } while (this.BeordraId == 0);
                    FieldValue.Add(this.BeordraId.ToString());
                                        
                    List<string> DeleteSave =  filHanterare.DeleteAllMatchingRecords(
                                                Utilities.BuildSearchString(FieldNamn[0], this.BeordraId.ToString())+Utilities.DELIMETER,
                                                "Beordra ta bort.", "Beordra hittades inte.");
                    
                    if (DeleteSave.Count > 0)
                    {
                        filHanterare.WriteArrayDataToFile(DeleteSave);
                    }
                    FieldValue.Clear();
                    
                    string str = "";
                    do
                    {
                        str = myinput.UserInput("Vill du ta bort en annan beordra J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            deleteanotherBeordra = true;
                        }
                        else if (str == "N")
                        {
                            Console.Clear();
                            deleteanotherBeordra = false;
                        }
                    } while ((str != "J") && (str != "N"));

                } while (deleteanotherBeordra == true);//loop tills användaren vill ta bort inte en annan beordra
            }
        }
        /// <summary>
        /// Skriva ut Beordra
        /// </summary>
        public void ViewBeordra(int beordraId=0)
        {
            string str = "";
            bool viewanotherprodukt = true;
            if (filHanterare.FileExists() == false)
            {
                Utilities.WriteErrorLog("Ingen beordra att se.");
                Utilities.WriteErrorLogOchContinue();
            }
            else
            {               
                List<string>viewItem;                
                List<string> rowtoview = new List<string>();
                do
                {
                    if (beordraId == 0)
                    {
                        do
                        {
                            Utilities.WriteLog("Ange beordra id:");
                            this.BeordraId = Utilities.ValidateInt(Console.ReadLine());

                        } while (this.BeordraId == 0);
                    }
                    else
                    {
                        this.BeordraId = beordraId;
                    }
                    List<string> datalist = filHanterare.FetchAllLines();
                    viewItem = Utilities.FetchItemList(datalist, Utilities.BuildSearchString(FieldNamn[0], this.BeordraId.ToString()));
                    if (viewItem.Count > 0)
                    {
                        DisplayBeordra(viewItem);
                    }
                    else
                        Utilities.WriteLineLog("{0} {1} {2}", FieldNamnDisplay[0], this.BeordraId.ToString(), "finns inte.");
                    
                    do
                    {
                        str = myinput.UserInput("Vill du se en annan beordra J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            viewanotherprodukt = true;
                        }
                        else if (str == "N")
                        {
                            Utilities.ConsoleClear();
                            viewanotherprodukt = false;
                        }
                    } while ((str != "J") && (str != "N"));
                } while (viewanotherprodukt == true) ;
            }
        }
        /// <summary>
        /// Skriva ut till beordra fil
        /// </summary>
        /// <param name="text"></param>
        /// <param name="msgtouser"></param>
        private void AppendBeordra(string text,string msgtouser)
        {
            filHanterare.AppendFile(text, msgtouser);
        }
        /// <summary>
        /// Öka id med 1
        /// </summary>
        /// <param name="text"></param>
        private void UpdateMaxBeordraId(string text)
        {            
            beordraidfilHanterare.UpdateFile(text);
        }
        /// <summary>
        /// Skriva ut Beordra
        /// </summary>
        /// <param name="orderlist"></param>
        private void DisplayBeordra(List<string> orderlist)
        {

            string[] itemarray;
            string[] fieldarray;
            string beordraid;
            string orderdate;
            string kundName;
            string kundcode;
            string produktcode;
            string productName ;
            int ProductAntal; 
            double productpris;
            double Total=0, TotalValue=0;
            int i = 1;
            string row;
            
            StringBuilder formatoutput = new StringBuilder();
            Utilities.ConsoleClear();
            foreach (var item in orderlist)
            {
                itemarray = item.Split(Utilities.DELIMETER);

                if (i == 1)
                {
                    Utilities.BreakLine('-', 100);
                    
                    fieldarray = itemarray[0].Split(':');
                    beordraid = fieldarray[1];

                    fieldarray = itemarray[1].Split(':');
                    orderdate = fieldarray[1];

                    fieldarray = itemarray[2].Split(':');
                    row = Utilities.FetchItem(KundList, itemarray[2] + Utilities.DELIMETER);
                    kundcode = fieldarray[1];
                    kundName = Utilities.GetCodeValue(row, 1);

                    //Huvud av beordra

                    formatoutput.AppendLine(Utilities.StringPadRightLeft("BEORDRA", 45, 0));
                    formatoutput.AppendLine(Utilities.StringPadRightLeft("-------", 45, 0));
                    formatoutput.AppendLine(Utilities.StringPadRight("Beordra Id",20) + beordraid);
                    formatoutput.AppendLine(Utilities.StringPadRight("Beordra Datum", 20) + orderdate);
                    formatoutput.AppendLine(Utilities.StringPadRight("Kund Code", 20) + kundcode);
                    formatoutput.AppendLine(Utilities.StringPadRight("Kund Name", 20) + kundName);
                    Utilities.WriteLineLog(formatoutput.ToString());
                    Utilities.BreakLine('-', 100);

                    //Kolumnrubrik fär Detalj av beordra
                    formatoutput.Clear();
                    formatoutput.Append(Utilities.StringPadRight("Sl.No", 10));
                    formatoutput.Append(Utilities.StringPadRight("Produkt Kode", 20));
                    formatoutput.Append(Utilities.StringPadRight("Produkt Namn",30));
                    formatoutput.Append(Utilities.StringPadLeft("Antal", 10));
                    formatoutput.Append(Utilities.StringPadLeft("Pris", 10));
                    formatoutput.Append(Utilities.StringPadLeft("Totalt", 15));
                    Utilities.WriteLineLog(formatoutput.ToString());
                    Utilities.BreakLine('-', 100);
                }
                
                fieldarray = itemarray[3].Split(':');//produktcode
                row = Utilities.FetchItem(ProduktList, itemarray[3] + Utilities.DELIMETER);

                produktcode = fieldarray[1];
                productName = Utilities.GetCodeValue(row,1);

                fieldarray = itemarray[4].Split(':');//pris
                productpris = double.Parse(fieldarray[1]);

                fieldarray = itemarray[5].Split(':');//qty

                ProductAntal = int.Parse(fieldarray[1]);

                Total = Math.Round (ProductAntal * productpris,2);

                formatoutput.Clear();

                //Detalj av beordra
                formatoutput.Append(Utilities.StringPadRight(i.ToString(), 10));
                formatoutput.Append(Utilities.StringPadRight(produktcode, 20));
                formatoutput.Append(Utilities.StringPadRight(productName, 30));
                formatoutput.Append(Utilities.StringPadLeft(ProductAntal.ToString(), 10));
                formatoutput.Append(Utilities.StringPadLeft(productpris.ToString(), 10));
                formatoutput.Append(Utilities.StringPadLeft(Total.ToString(), 15));
                Utilities.WriteLineLog(formatoutput.ToString());
                TotalValue += Total;
                i += 1;    
            }
            Utilities.BreakLine('-', 100);
            Console.WriteLine(Utilities.StringPadRightLeft("Totala värdet : "+ TotalValue, 95, 1));
            Utilities.BreakLine('-', 100);
        }

    }
    
}
