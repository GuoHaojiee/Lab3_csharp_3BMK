using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp4.Data;

namespace ConsoleApp4.Data
{
    public abstract class V1Data : IEnumerable<DataItem>
    {
        public string Key { get; set; }
        public DateTime Date { get; set; }

        public V1Data(string key, DateTime date)
        {
            Key = key;
            Date = date;
        }

        public abstract double Y1Average { get; }
        public abstract double MaxDistance { get; }
        public abstract string ToLongString(string format);

        public override string ToString()
        {
            string result = $"Key: {Key}; Time: {Date}";
            foreach (var item in GetDataItems())
            {
                result += $"\n{item}";
            }
            return result;
        }

        public IEnumerator<DataItem> GetEnumerator()
        {
            return GetDataItems().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract IEnumerable<DataItem> GetDataItems();
    }
}
