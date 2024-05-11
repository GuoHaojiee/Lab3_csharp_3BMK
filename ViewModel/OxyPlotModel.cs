using ConsoleApp4;
using ConsoleApp4.Data;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfApp2
{
    class OxyPlotModel
    {
        public PlotModel plotModel { get; private set; }
        SplineData data;
        V1DataArray v1DataArray;
        public OxyPlotModel(SplineData data, V1DataArray v1DataArray)
        {
            this.data = data;
            this.v1DataArray = v1DataArray;
            this.plotModel = new PlotModel { Title = "Spline Interpolation result" };
            AddSeries();
        }
        public void AddSeries()
        {
            this.plotModel.Series.Clear();
            Legend legend = new Legend();
            LineSeries lineSeries = new LineSeries();
            if (data != null)
            {
                this.plotModel.Series.Clear();
                for (int js = 0; js < data.numOfNodes; js++)
                {
                    OxyColor color = (js == 0) ? OxyColors.Green : OxyColors.Blue;
                    lineSeries.Points.Add(new DataPoint(data.data[js].Coord, data.data[js].Spline));
                    lineSeries.Color = color;
                    lineSeries.Title = "Cubic spline interpolation";
                }
            }
            plotModel.Legends.Add(legend);
            this.plotModel.Series.Add(lineSeries);
            Legend legend_rd = new Legend();
            LineSeries lineSeries_rd = new LineSeries();
            for (int js = 0; js < v1DataArray.NumOfNodes; js++)
            {
                OxyColor color = (js == 0) ? OxyColors.Red : OxyColors.Black;
                lineSeries_rd.Points.Add(new DataPoint(v1DataArray.xArray[js], v1DataArray.yArray[0][js]));
                lineSeries_rd.Color = color;

                lineSeries_rd.MarkerType = MarkerType.Circle;
                lineSeries_rd.MarkerSize = 4;
                lineSeries_rd.MarkerStroke = color;
                lineSeries_rd.MarkerFill = color;
                lineSeries_rd.Title = "Original function";
            }
            plotModel.Legends.Add(legend_rd);
            this.plotModel.Series.Add(lineSeries_rd);
        }
    }
}
