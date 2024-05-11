using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp4.Data;

namespace ConsoleApp4
{
    public class SplineData
    {
        public V1DataArray V1DataArray { get; set; }
        public int numOfNodes { get; set; } // число узлов новой равномерной сетки
        public double bcSpline1 { get; set; } // массив граничных значений для 1-го сплайна
        public double bcSpline2 { get; set; }// массив граничных значений для 2-го сплайна
        public List<SplineDataItem> data { get; set; } // Список с результатами сплайн интерполяции

        public SplineData(V1DataArray V1DataArray,int numOfNodes, double bcSpline1, double bcSpline2)
        {
            this.V1DataArray = V1DataArray;
            this.bcSpline1 = bcSpline1;
            this.bcSpline2 = bcSpline2;
            this.numOfNodes = numOfNodes;
            data = new List<SplineDataItem>();
        }

        public int CreateSpline()
        {
            int nx = V1DataArray.NumOfNodes;
            double[] x = Array.Empty<double>();
            if (V1DataArray.UniformityCheck_)
                x = new double[2] { V1DataArray.Left, V1DataArray.Right };
            else
                x = V1DataArray.xArray;
            int ny = 1;
            double[] y = V1DataArray.yArray[0];
            bool isUniform = V1DataArray.UniformityCheck_;
            double[] scoeff = new double[ny * 4 * (nx - 1)];
            double[] bc = new double[2] { bcSpline1, bcSpline2 };

            int nsite = numOfNodes;
            double[] site = new double[2] { V1DataArray.Left, V1DataArray.Right };
            int ndorder = 3;
            int[] dorder = new int[3] { 1, 1, 1 };
            int nder = 3;
            double[] resultDeriv = new double[ny * nder * nsite];

            int nlim = 1;
            double[] llim = new double[1] { V1DataArray.Left };
            double[] rlim = new double[1] { V1DataArray.Right };
            double[] resultInteg = new double[ny * nlim];

            int ret = -1;
            try
            {
                CubicSplineInterpoalte(nx, x, ny, y, isUniform, bc, scoeff, nsite, site, ndorder, dorder, resultDeriv, nlim, llim, rlim, resultInteg, ref ret);

                if (ret != 0)
                {
                    return ret;
                }
                double[] newGrid = new double[nsite];
                double step = (V1DataArray.Right - V1DataArray.Left) / (numOfNodes - 1);
                int k = 0;
                for (double i = V1DataArray.Left; k < numOfNodes - 1; i += step, k++)
                {
                    newGrid[k] = i;
                }
                newGrid[k] = V1DataArray.Right;
                for (int i = 0, ind = 0; i < nder * nsite - 2; i += 3, ind++)
                {
                    double value = resultDeriv[i];
                    double firstDeriv = resultDeriv[i + 1];
                    double secondDeriv = resultDeriv[i + 2];
                    SplineDataItem tmpSplineDataItem = new(newGrid[ind], value, firstDeriv, secondDeriv);
                    data.Add(tmpSplineDataItem);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [DllImport("..\\..\\..\\..\\x64\\Debug\\lab3dll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CubicSplineInterpoalte(int nx, double[] x, int ny, double[] y, bool isUniform, double[] bc, double[] scoeff, int nsite, double[] site,
                int ndorder, int[] dorder, double[] resultDeriv, int nlim, double[] llim, double[] rlim, double[] resultInteg, ref int ret);
    }

}
