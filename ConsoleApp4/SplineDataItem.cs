using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public struct SplineDataItem
    {
        public double Coord { get; set; }
        public double Spline { get; set; }
        public double FirstDer { get; set; }
        public double SecondDer { get; set; }
        public SplineDataItem(double x, double val, double firstDer, double secondDer)
        {
            Coord = x;
            Spline = val;
            FirstDer = firstDer;
            SecondDer = secondDer;
        }
        public string ToString(string format)
        {
            return $"X: {String.Format(format, Coord)};\n" +
                   $"Value: {String.Format(format, Spline)}\n" +
                   $"First Derivative: {String.Format(format, FirstDer)}\n" +
                   $"Second Derivative {String.Format(format, SecondDer)}";

        }

        public override string ToString()
        {
            return $"X: {Coord}\n" +
                   $"Value: {Spline}\n" +
                   $"First Derivative: {FirstDer}\n" +
                   $"Second Derivative {SecondDer}";
        }
    }
}
