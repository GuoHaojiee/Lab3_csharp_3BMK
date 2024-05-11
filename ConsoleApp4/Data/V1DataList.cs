using ConsoleApp4;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp4.Data;

namespace ConsoleApp4.Data
{
    public class V1DataList : V1Data
    {
        public List<DataItem> DataItems { get; set; }

        public V1DataList(string key, DateTime date) : base(key, date)
        {
            DataItems = new List<DataItem>();
        }

        public V1DataList(string key, DateTime time, double[] X, FDI F) : base(key, time)
        {
            DataItems = new List<DataItem>();
            foreach (double xelem in X)
            {
                if (!DataItems.Exists(sample => sample.X == xelem))
                {
                    DataItems.Add(F(xelem));
                }
            }
            DataItems.Sort((X, Y) => X.X.CompareTo(Y.X));
        }

        public override double Y1Average
        {
            get
            {
                if (DataItems.Count < 2)
                    return 0.0;

                double average = 0.0;
                for (int i = 0; i < DataItems.Count; i++)
                {
                    average += DataItems[i].X;
                }
                return average / DataItems.Count * 2;
            }
        }

        public override double MaxDistance
        {
            get
            {
                if (DataItems.Count < 2)
                    return 0.0;

                double maxDistance = 0.0;
                for (int i = 0; i < DataItems.Count; i++)
                {
                    for (int j = i + 1; j < DataItems.Count; j++)
                    {
                        double distance = Math.Abs(DataItems[i].X - DataItems[j].X);
                        if (distance > maxDistance)
                            maxDistance = distance;
                    }
                }
                return maxDistance;
            }
        }

        public static explicit operator V1DataArray(V1DataList source)
        {
            source.DataItems.Sort((X, Y) => X.X.CompareTo(Y.X));
            V1DataArray dataarray = new V1DataArray(source.Key, source.Date);
            double[] xArray = new double[source.DataItems.Count];
            double[][] yArray = new double[2][];
            yArray[0] = new double[source.DataItems.Count];
            yArray[1] = new double[source.DataItems.Count];
            for (int i = 0; i < source.DataItems.Count; i++)
            {
                xArray[i] = source.DataItems[i].X;
                yArray[0][i] = source.DataItems[i].Y1;
                yArray[1][i] = source.DataItems[i].Y2;
            }
            dataarray.xArray = xArray;
            dataarray.yArray = yArray;
            return dataarray;
        }

        public override string ToString()
        {
            string result = $"{Key} {Date}\n";
            foreach (var item in DataItems)
            {
                result += $"{item}\n";
            }
            return result;
        }

        public override string ToLongString(string format)
        {
            string result = ToString();
            foreach (var item in DataItems)
            {
                result += $"\n{item.ToLongString(format)}";
            }
            return result;
        }

        public override IEnumerable<DataItem> GetDataItems()
        {
            return DataItems;
        }
    }
}
