using ConsoleApp4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class main
    {
        static void Main() {
            V1DataArray V1DataArray = new V1DataArray("DataArray", DateTime.Now, 5, 1, 5, true, MyFunctions.MyFunction1);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(V1DataArray.xArray[i]);
                Console.WriteLine(1 + (6 - 1) * i / (5 - 1));
            }
            Console.ReadLine();
        }
    }
}
