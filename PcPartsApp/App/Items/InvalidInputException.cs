using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Items
{
   public class InvalidInputException : Exception
   {
        public void PrintError()
        {
            Console.WriteLine("INVALID INPUT!!! try again(Y/N)?");
        }

   }
}
