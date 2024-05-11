using ConsoleApp4.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp4.Data
{
    public class V1DataArray : V1Data
    {
        public double[] xArray { get; set; }
        public double[][] yArray { get; set; }
        public double Left { get; set; }
        public double Right { get; set; }
        public int NumOfNodes { get; set; }
        public bool UniformityCheck_ { get; set; }
        public FValues F_ { get; set; }

        public V1DataArray(string key, DateTime date) : base(key, date)
        {
            Left = 0;
            Right = 0;
            NumOfNodes = 0;
            UniformityCheck_ = false;
            F_ = (double x, ref double y1, ref double y2) => { y1 = x; y2 = 2 * x; };
            xArray = new double[0];
            yArray = new double[2][];
            yArray[0] = new double[0];
            yArray[1] = new double[0];
        }

        public V1DataArray(string key, DateTime time, double[] x, FValues F) : base(key, time)
        {
            Left = 0;
            Right = 0;
            NumOfNodes = 0;
            UniformityCheck_ = false;
            xArray = x.OrderBy(x => x).Distinct().ToArray();
            yArray = new double[2][];
            yArray[0] = new double[xArray.Length];
            yArray[1] = new double[xArray.Length];
            for (int i = 0; i < xArray.Length; i++)
            {
                F(xArray[i], ref yArray[0][i], ref yArray[1][i]);
            }
        }

        public V1DataArray(string key, DateTime date, int nX, double xL, double xR, bool b, FValues F) : base(key, date)
        {
            Left = xL;
            Right = xR;
            NumOfNodes = nX;
            UniformityCheck_ = b;
            F_ = F;
            xArray = new double[nX];
            yArray = new double[2][];
            yArray[0] = new double[nX];
            yArray[1] = new double[nX];

            for (int i = 0; i < nX; i++)
            {
                xArray[i] = xL + (xR - xL) * i / (nX - 1);
                F(xArray[i], ref yArray[0][i], ref yArray[1][i]);
            }
        }

        public V1DataArray(string key, DateTime date, string filename) : base(key, date)
        {
            try
            {
                Load(filename, out V1DataArray Data);
                Left = Data.Left;
                Right = Data.Right;
                NumOfNodes = Data.NumOfNodes;
                UniformityCheck_ = Data.UniformityCheck_;
                F_ = Data.F_;
                xArray = Data.xArray;
                yArray = Data.yArray;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public double[] this[int index]
        {
            get
            {
                if (index == 0 || index == 1)
                {
                    return yArray[index];
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid index. Only 0 or 1 is allowed.");
                }
            }
        }

        public V1DataList List
        {
            get
            {
                V1DataList list = new V1DataList(Key, Date);

                for (int i = 0; i < xArray.Length; i++)
                {
                    DataItem item = new DataItem(xArray[i], yArray[0][i], yArray[1][i]);
                    list.DataItems.Add(item);
                }
                return list;
            }
        }

        public override double Y1Average
        {
            get
            {
                double average = 0.0;
                for (int i = 0; i < yArray.Length; i++)
                {
                    average += xArray[i];
                }
                return average / yArray.Length * 2 + 1;
            }
        }

        public override double MaxDistance
        {
            get
            {
                double maxDist = 0.0;
                for (int i = 0; i < xArray.Length; i++)
                {
                    for (int j = i + 1; j < xArray.Length; j++)
                    {
                        double dist = Math.Abs(xArray[i] - xArray[j]);
                        if (dist > maxDist)
                        {
                            maxDist = dist;
                        }
                    }
                }
                return maxDist;
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Data Count: {xArray.Length}";
        }

        public override string ToLongString(string format)
        {
            string longString = ToString() + "\n";

            for (int i = 0; i < xArray.Length; i++)
            {
                longString += $"x = {xArray[i].ToString(format)}, y1 = {yArray[0][i].ToString(format)}, y2 = {yArray[1][i].ToString(format)}\n";
            }
            return longString;
        }

        public override IEnumerable<DataItem> GetDataItems()
        {
            List<DataItem> dataItems = new List<DataItem>();
            for (int i = 0; i < xArray.Length; i++)
            {
                dataItems.Add(new DataItem(xArray[i], yArray[0][i], yArray[1][i]));
            }
            return dataItems;
        }

        public void Save(string filename)
        {
            FileStream? fs = null;
            StreamWriter? writer = null;
            try
            {
                fs = File.Create(filename);
                writer = new StreamWriter(fs);
                writer.WriteLine(Left.ToString());
                writer.WriteLine(Right.ToString());
                writer.WriteLine(NumOfNodes.ToString());
                writer.WriteLine(UniformityCheck_.ToString());

                for (int i = 0; i < NumOfNodes; ++i)
                {
                    writer.WriteLine(xArray[i].ToString());
                }
                for (int i = 0; i < NumOfNodes; ++i)
                {
                    writer.WriteLine(yArray[0][i].ToString());
                }
                return;
            }
            catch (Exception x)
            {
                Console.WriteLine($"ERROR SAVING FILE ! ! !: {x}");
                throw;
            }
            finally
            {
                writer?.Dispose();
                fs?.Close();
            }
        }

        public static void Load(string filename, out V1DataArray Data)
        {
            FileStream? fs = null;
            StreamReader? reader = null;
            try
            {
                double left, right;
                int node_cnt;
                bool is_uniform;

                fs = File.OpenRead(filename);
                reader = new StreamReader(fs);
                left = double.Parse(reader.ReadLine());
                right = double.Parse(reader.ReadLine());
                node_cnt = Convert.ToInt32(reader.ReadLine());
                is_uniform = Convert.ToBoolean(reader.ReadLine());
                Data = new V1DataArray("LoadData", DateTime.Now, node_cnt, left, right,is_uniform, MyFunctions.MyFunction1);
                for (int i = 0; i < node_cnt; ++i)
                {
                    Data.xArray[i] = double.Parse(reader.ReadLine());
                }
                for (int i = 0; i < node_cnt; ++i)
                {
                    Data.yArray[0][i] = double.Parse(reader.ReadLine());
                }
                return;
            }
            catch (Exception x)
            {
                Console.WriteLine($"ERROR LOADING FILE!!!: {x}");
                throw;
            }
            finally
            {
                reader?.Dispose();
                fs?.Close();
            }
        }
    }
}
