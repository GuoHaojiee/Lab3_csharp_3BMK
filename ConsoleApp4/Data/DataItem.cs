using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp4.Data;

namespace ConsoleApp4.Data
{
    public struct DataItem
    {
        public double X { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }

        public DataItem(double x, double y1, double y2)
        {
            X = x;
            Y1 = y1;
            Y2 = y2;
        }

        public string ToLongString(string format)
        {
            return $"X: {X.ToString(format)}, Y1: {Y1.ToString(format)}, Y2: {Y2.ToString(format)}";
        }

        public override string ToString()
        {
            return $"X: {X}, Y1: {Y1}, Y2: {Y2}";
        }
    }
}
