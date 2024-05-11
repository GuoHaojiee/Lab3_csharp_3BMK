using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4.Data
{
    public static class MyFunctions
    {
        public static void MyFunction1(double x, ref double y1, ref double y2)
        {
            y1 = x * 2;
            y2 = x * 3;
        }
        public static void MyFunction2(double x, ref double y1, ref double y2)
        {
            y1 = x;
            y2 = x;
        }
        public static void MyFunction3(double x, ref double y1, ref double y2)
        {
            y1 = x * x;
            y2 = x * x * x;
        }
    }
}
