using System;
using System.Collections.Generic;
using System.Text;

namespace OrderHanteringsSystem
{
    class TextHanterar
    {
        string FilData;
        FilHanterare filHanterare;
        public TextHanterar(FilHanterare filHanterare)
        {
            this.filHanterare = filHanterare;
            if (filHanterare.FileExists())
            {
                this.FilData= this.filHanterare.ReadFileText();                
                Utilities.WriteLineLog(FilData);
            }
            else
            {
                Utilities.WriteLineLog("\nFil {0} finns inte.\n", filHanterare.FilNamn);

            }
        }
        private void ConvertToUpper()
        {
            Utilities.WriteLineLog("Stora bokstaver");
            Utilities.BreakLine('-', 16);
            Utilities.WriteLineLog(FilData.ToUpper());
        }
        private void ConvertToLower()
        {
            Utilities.WriteLineLog("Lilla bokstäver");
            Utilities.BreakLine('-', 16);
            Utilities.WriteLineLog(FilData.ToLower());
        }
        private void SplitText()
        {
            Utilities.WriteLineLog("Beskär vid mellanslag");
            Utilities.BreakLine('-', 16);
            string[] ordArray = FilData.Split(' '); // Crop at space
            foreach (string ord in ordArray)
            {
                Utilities.WriteLineLog(ord);
            }
        }
    }
}
