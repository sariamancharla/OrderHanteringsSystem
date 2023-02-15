using System;

namespace OrderHanteringsSystem
{
    class Program
    {
        static bool programQuit = false;
        static Input input;
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            input = new Input();

            while (programQuit == false)
            {
                menu.MainMenuText();
                UserChoice(input.UserVal());
            }
        }
        /// <summary>
        /// Utför metoden baserat på användarinmatning
        /// </summary>
        /// <param name="userInput"></param>
        static void UserChoice(int userInput)
        {
            string str;
            Produkt produkt = new Produkt();
            kund kund = new kund();
            Beordra beordra;
            FilHanterare filHanterare ;
            switch (userInput)
            {
                case 1://Skapa produkt
                    produkt.AddProdukt();
                    break;
                case 2://Ändra produkt
                    produkt.ModifyProdukt();
                    break;
                case 3://Ta bort produkt
                    produkt.DeleteProdukt();
                    break;
                case 4://Se produkt 
                    produkt.ViewProdukt();
                    break;
                case 5://Skriva ut alla produkt
                    produkt.ViewAll();
                    break;

                case 6://Skapa kund
                    kund.AddKund();
                    break;
                case 7://Ändra kund.
                    kund.ModifyKund();
                    break;
                case 8://Ta bort kund
                    kund.DeleteKund();
                    break;
                case 9://Se kund. 
                    kund.ViewKund();
                    break;
                case 10://Skriva ut alla kund.
                    kund.ViewAll();
                    break;

                case 11://Skapa beordra
                    beordra = new Beordra();
                    beordra.AddBeordra();
                    break;
                case 12://Ta bort beordra
                    beordra = new Beordra();
                    beordra.DeleteBeordra();
                    break;
                case 13://Se beordra. 
                    beordra = new Beordra();
                    beordra.ViewBeordra();
                    break;                

                case 14://Ta bort hela produkt,kund,beordra
                    filHanterare = new FilHanterare();
                    do
                    {
                        str = input.UserInput("Är du säker på att du vill ta bort alla J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            filHanterare.TaBortAllaFiler();//Ta Bort Alla Filer
                            Utilities.WriteLineLog("Tryck på valfri tangent för att fortsätta......");
                            Console.ReadLine();
                            Utilities.ConsoleClear();
                        }
                        else if (str == "N")
                        {
                            Utilities.ConsoleClear();
                        }
                    } while ((str != "J") && (str != "N"));
                    break;
                case 15://Avslut program
                    do
                    {
                        str = input.UserInput("Är du säker på att du vill avsluta J/N?");
                        str = Utilities.CheckUserJN(str);
                        if (str == "J")
                        {
                            programQuit = true;
                        }
                        else if (str == "N")
                        {
                            Utilities.ConsoleClear();
                        }
                    } while ((str != "J") && (str!="N"));
                    break;
                default:
                    Utilities.WriteLineLog("Ditt val är ej giltigt, prova igen. Ange värdet 1-15.\n");
                    Utilities.WriteErrorLogOchContinue();
                    break;
            }
        }                    
    }
}
