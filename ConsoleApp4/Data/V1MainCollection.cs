using ConsoleApp4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4.Data
{
    public class V1MainCollection : System.Collections.ObjectModel.ObservableCollection<V1Data>
    {
        public V1MainCollection()
        {
            double[] sampleData1 = new double[] { 1.0, 2.0, 3.0 };
            V1DataArray dataArray = new V1DataArray("Array1", DateTime.Now, sampleData1, (double x, ref double y1, ref double y2) => { y1 = x; y2 = 2 * x; });
            double[] sampleData2 = new double[] { 1.0, 2.0, 4.0 };
            V1DataList dataList = new V1DataList("List1", DateTime.Now, sampleData2, x => new DataItem(x, x, 2 * x));
            Add(dataArray);
            Add(dataList);
        }

        public V1MainCollection(int nV1DataArray, int nV1DataList)
        {
            for (int i = 0; i < nV1DataArray; i++)
            {
                double[] xArray = { 0, 0.2, 0.4, 0.7, 0.8, 1 };
                V1DataArray dataArray = new V1DataArray($"Array_{i}", DateTime.Now, xArray, FValuesMethod);

                Add(dataArray);
            }
            for (int i = 0; i < nV1DataList; i++)
            {
                double[] xArray = { 0, 0.2, 0.4, 0.7, 0.8, 1 };
                V1DataList dataList = new V1DataList($"List_{i}", DateTime.Now, xArray, FDIMethod);
                Add(dataList);
            }
        }

        public static void FValuesMethod(double x, ref double y1, ref double y2)
        {
            y1 = x;
            y2 = 2 * x;
        }

        public static DataItem FDIMethod(double x)
        {
            // Реализация FDI
            return new DataItem(x, x, 2 * x);
        }

        public bool Contains(string key)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Key == key)
                    return true;
            }
            return false;
        }

        public new bool Add(V1Data v1Data)
        {
            if (Contains(v1Data.Key))
            {
                return false;
            }
            base.Add(v1Data);
            return true;
        }

        public override string ToString()
        {
            return "V1MainCollection";
        }

        public string ToLongString(string format)
        {
            string longString = ToString() + ":\n";
            foreach (V1Data elem in this)
            {
                longString += elem.ToLongString(format) + "\n";
            }
            return longString;
        }

        public double AverageModulus
        {
            get
            {
                var query = from data in this
                            from item in data
                            select Math.Sqrt(Math.Pow(item.Y1, 2) + Math.Pow(item.Y2, 2));

                return query.Any() ? query.Average() : double.NaN;
            }
        }

        public DataItem MaxModulusDifferenceItem
        {
            get
            {
                var query = from data in this
                            from item in data
                            let modulus = Math.Sqrt(Math.Pow(item.Y1, 2) + Math.Pow(item.Y2, 2))
                            orderby Math.Abs(modulus - AverageModulus) descending
                            select item;

                return query.FirstOrDefault();
            }
        }

        public IEnumerable<double> CommonXValues
        {
            get
            {
                var query = from data in this
                            from item in data
                            group item by item.X into grouped
                            where grouped.Count() >= 2
                            orderby grouped.Key ascending
                            select grouped.Key;

                return query.Distinct();
            }
        }

        public IEnumerable<double> XValuesInArraysNotInLists
        {
            get
            {
                var arrayXValues = this.OfType<V1DataArray>().SelectMany(data => data.xArray);
                var listXValues = this.OfType<V1DataList>().SelectMany(data => data.Select(item => item.X));

                var result = arrayXValues.Except(listXValues);

                return result;
            }
        }
    }
}
