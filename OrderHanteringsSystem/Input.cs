using System;

namespace OrderHanteringsSystem
{
    class Input
    {
        /// <summary>
        /// Acceptera värde som anges av användaren
        /// </summary>
        /// <returns></returns>
        public int UserVal()
        {
            string inputstring = Console.ReadLine();
            return Utilities.ValidateInt(inputstring);
        }
        /// <summary>
        /// Acceptera texten som anges av användaren
        /// </summary>
        /// <returns></returns>
        public string  UserInput(string msg)
        {
            Utilities.WriteLog(msg);
            return Console.ReadLine();
        }
        
        
    }
}
