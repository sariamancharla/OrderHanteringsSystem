using System;
using System.Collections.Generic;

namespace OrderHanteringsSystem
{
    internal static class Utilities 
    {
        internal static string PathNamn = @"\Data";
        internal const string DELIMETER = ";";//Separator för kolumnvärden i rad i en textfil

        /// <summary>
        /// Trycka värdet för n gånger 
        /// </summary>
        /// <param name="breakLineType"></param>
        /// <param name="length"></param>        
        ///
        internal static void BreakLine(char breakLineType, int length)
        {

            string breakLine = new string(breakLineType, length);
            WriteLineLog(breakLine);
        }
        /// <summary>
        /// Skriv ut meddelandet och skapa en ny rad
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="param"></param>
        internal static void WriteLineLog(string txt, params string[] param)
        {
            Console.WriteLine(txt, param);
        }
        /// <summary>
        /// Skriv ut meddelandet
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="param"></param>
        internal static void WriteLog(string txt, params string[] param)
        {
            Console.Write(txt + " ", param);
        }
        /// <summary>
        /// Skriv ut fel meddelandet
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="param"></param>
        internal static void WriteErrorLog(string txt, params string[] param)
        {
            Console.Write("\n" + txt + "\n", param);
        }
        internal static void WriteErrorLogOchContinue()
        {
            WriteLineLog("Tryck på valfri tangent för att fortsätta......");
            Console.ReadLine();
            ConsoleClear();
        }
        /// <summary>
        /// Validate bara Ja eller Nej (J/N)
        /// </summary>
        /// <param name="userinput"></param>
        /// <returns></returns>
        internal static string CheckUserJN(string userinput)
        {
            if (Utilities.ValidateChar(userinput, "J", "N") == "J")
            {
                return "J";
            }
            else
            {
                return userinput.ToUpper();
            }
        }
        /// <summary>
        /// Rensa console
        /// </summary>
        internal static void ConsoleClear()
        {
            Console.Clear();
        }
        /// <summary>
        /// konvertera string till Int och inputNum=0, om Ogiltig data 
        /// </summary>
        /// <param name="inputstring"></param>
        /// <returns></returns>
        internal static int ValidateInt(string inputstring)
        {            
            Int32.TryParse(inputstring, out int inputNum);
            if (inputNum == 0) Utilities.WriteLineLog("Ange ett nummer.");
            return inputNum;
        }
        /// <summary>
        /// Validera användarvärden t.ex "J" och "N" (Ja eller Nej)
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        internal static string ValidateChar(string txt, params string[] param)
        {
            string val = "";
            for (int i = 0; i < param.Length; i++)
            {
                val += param[i];
                val += ',';
                if (txt.ToUpper() == param[i])
                {
                    return param[i];

                }
            }
            WriteLineLog("Ange värdena {0}",val.Substring(0, val.Length - 1));
            return string.Empty;
        }
        /// <summary>
        /// Textlängden ska vara mellan minlength och maxlength.
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static string ValidateText(string txt, int minlength = 0,int maxlength=0)
        {
            if((minlength==maxlength)&&(txt.Length != maxlength) && (maxlength != 0)) //t.ex-bara 4 längd
            {
                WriteLineLog("Längden ska vara {0}", maxlength.ToString());
                return string.Empty;
            }
            else if (!((txt.Length >= minlength) && (txt.Length <= maxlength))) //t.ex-mellan 1 och 20
            {
                WriteLineLog("Längden ska vara mellan {0} och {1}.", minlength.ToString(), maxlength.ToString());
                return string.Empty;
            }
            else
            {
                return txt;
            }
        }
        /// <summary>
        /// konvertera string till double och inputNum=0, om Ogiltig data 
        /// </summary>
        /// <param name="inputstring"></param>
        /// <returns></returns>
        internal static double ValidateDouble(string inputstring)
        {
            double.TryParse(inputstring, out double inputNum);
            if (inputNum == 0) Utilities.WriteLineLog("Ange ett nummer.");
            return Math.Round(inputNum,2);
        }
        /// <summary>
        /// Bygg sträng med raddata FieldNamn1:FieldValue1,FieldNamn2:FieldValue2..
        /// </summary>
        /// <param name="FieldNamn"></param>
        /// <param name="FieldValue"></param>
        /// <returns></returns>
        internal static string BuildString(List<string> FieldNamn, List<string> FieldValue)
        {
            string str = "";
            for (int i = 0; i < FieldNamn.Count; i++)
            {
                str += FieldNamn[i] + FieldValue[i] + DELIMETER;
            }
            return str.Substring(0, str.Length - 1);
        }
        /// <summary>
        /// bygg sträng med raddata för sökning FieldNamn:FieldValue
        /// </summary>
        /// <param name="FieldNamn"></param>
        /// <param name="FieldValue"></param>
        /// <returns></returns>
        internal static string BuildSearchString(string FieldNamn, string FieldValue)
        {
            return FieldNamn + FieldValue;
        }
        /// <summary>
        /// Dela och visa en rad
        /// </summary>
        /// <param name="FieldNamnDisplay"></param>
        /// <param name="row"></param>
        internal static void ViewRow(List<string> FieldNamnDisplay, string row)
        {
            string[] itemarray;
            string[] fieldarray;
            itemarray = row.Split(Utilities.DELIMETER);
            for (int i = 0; i < FieldNamnDisplay.Count; i++)
            {
                fieldarray = itemarray[i].Split(':');//produkt Name:proukt 1;
                WriteLineLog(FieldNamnDisplay[i] + " : " + fieldarray[1]);
            }
        }
        /// <summary>
        /// Visa mer än en rad
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dataList"></param>
        public static void ViewAll(List<string> columnName, List<string> dataList)
        {
            foreach (var rowValue in dataList)
            {
                ViewRow(columnName, rowValue);
                Console.WriteLine();
            }
            Utilities.WriteErrorLogOchContinue();
        }
        /// <summary>
        /// Hämta data om det finns i listan annars returnera tom
        /// </summary>
        /// <param name="datalist"></param>
        /// <param name="rowtofetch"></param>
        /// <returns></returns>
        public static string FetchItem(List<string> datalist, string rowtofetch)
        {
            foreach (var item in datalist)
            {
                if (item.Contains(rowtofetch)) //Om finns
                {
                    return item;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Hämta data om det finns
        /// </summary>
        /// <param name="datalist"></param>
        /// <param name="rowtofetch"></param>
        /// <returns></returns>
        public static List<string> FetchItemList(List<string> datalist, string rowtofetch)
        {
            List<string> list = new List<string>();
            foreach (var item in datalist)
            {
                if (item.Contains(rowtofetch)) //Om finns
                {
                    list.Add(item);
                }
            }
            return list;
        }
        /// <summary>
        /// Skapa nästa id
        /// </summary>
        /// <param name="filHanterare"></param>
        /// <returns></returns>
        public static int NextID(FilHanterare filHanterare)
        {
            int.TryParse(filHanterare.ReadFileText(), out int maxID);
            return maxID + 1;
        }
        /// <summary>
        /// Hämta data från filen till listan
        ///  Ta bort från listan om raden finns
        ///  Skriv återstående listdata till filen
        /// </summary>
        /// <param name="filHanterare"></param>
        /// <param name="rowtodelete"></param>
        /// <param name="message"></param>
        /// <param name="failuremessage"></param>
        public static void DeleteLineFromFile(FilHanterare filHanterare,List<string> rowtodelete, string message, string failuremessage = "")
        {
            bool recordexists = false;
            bool isFileUdpate = false;
            string item = string.Empty;
            if (filHanterare.FileExists() == true)
            {
                List<string> datalist = filHanterare.FetchAllLines();
                for (int i = 0; i < rowtodelete.Count; i++)
                {
                    item = FetchItem(datalist, rowtodelete[i]);//Hämta data
                    if (!string.IsNullOrEmpty(item)) //Om finns - ta bort
                    {
                        isFileUdpate = true;
                        recordexists = true;
                        datalist.Remove(item);
                        Utilities.WriteLineLog(message);
                    }
                    if ((recordexists == false) && (!string.IsNullOrEmpty(failuremessage)))
                    {
                        Utilities.WriteLineLog(rowtodelete[i] + failuremessage);
                    }
                    recordexists = false;
                }
                if (isFileUdpate == true)
                {
                    filHanterare.WriteArrayDataToFile(datalist);
                }
            }
        }
        /// <summary>
        /// Dela och hämta värde från <code>:<value>
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetCodeValue(string item,int index)
        {
            string[] itemarray;
            string[] fieldarray;
            itemarray = item.Split(Utilities.DELIMETER);

            fieldarray = itemarray[index].Split(':');

            return fieldarray[1];
        }
        /// <summary>
        /// Pad tomt utrymme till höger
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxlength"></param>
        /// <returns></returns>
        public static string StringPadRight(string str,int maxlength)
        {
            str = str.PadRight((maxlength - str.Length)+str.Length, ' ');
            return str;
        }
        /// <summary>
        /// Pad tomt utrymme till vänster
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxlength"></param>
        /// <returns></returns>
        public static string StringPadLeft(string str, int maxlength)
        {
            str = str.PadLeft((maxlength - str.Length) + str.Length, ' ');
            return str;
        }
        /// <summary>
        /// Pad tomt utrymme till höger och vänster
        /// </summary>
        /// <param name="str"></param>
        /// <param name="padleftlength"></param>
        /// <param name="padrightlength"></param>
        /// <returns></returns>
        public static string StringPadRightLeft(string str, int padleftlength, int padrightlength)
        {
            str = str.PadRight(padrightlength, ' ').PadLeft(padleftlength,' ');
            return str;
        }        
    }

}
