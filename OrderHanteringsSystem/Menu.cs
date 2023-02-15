using System;

namespace OrderHanteringsSystem
{
    class Menu
    {
        public void MainMenuText()
        {
            Console.WriteLine("\n");
            Console.WriteLine("             ****************************************************************");
            Console.WriteLine("             *                                                              *");
            Console.WriteLine("             *                    ORDERHANTERINGSSYSTEM                     *");
            Console.WriteLine("             *                                                              *");
            Console.WriteLine("             ****************************************************************");
            Console.WriteLine("                         PRODUKT                               KUNDER");
            Console.WriteLine("                         -------                             ----------");
            Console.WriteLine("                     1: Skapa produkt.                  6 : Skapa kund.");
            Console.WriteLine("                     2: Ändra produkt.                  7 : Ändra kund."); ;
            Console.WriteLine("                     3: Ta bort produkt.                8 : Ta bort kund.");
            Console.WriteLine("                     4: Se produkt.                     9 : Se kund.");
            Console.WriteLine("                     5: Skriva ut alla.                 10: Skriva ut alla.");

            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("                         BEORDRA                              ALLMÄN");
            Console.WriteLine("                         -------                              ------");
            Console.WriteLine("                     11: Skapa beordra                    14: Ta bort hela.");
            Console.WriteLine("                     12: Ta bort beordra.                 15: Avslut program.");             
            Console.WriteLine("                     13: Se beordra.                     ");            
            Console.WriteLine("\n");
            Console.WriteLine("    *********************************************************************************");
            Console.WriteLine("    *   Lärare     : Andres Bendeck Berrios                                         *");
            Console.WriteLine("    *   Projekt av : Saritha Lakshmi A                                              *");
            Console.WriteLine("    *   För        : C3L - Programmering 1, SFX-IT,Tyresö.                          *");
            Console.WriteLine("    *********************************************************************************");
            Console.WriteLine("\n");
            Console.Write("Ange ditt val:");
        }
    }
}
