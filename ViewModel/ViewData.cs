using ConsoleApp4;
using ConsoleApp4.Data;
using Microsoft.Win32;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp2;
using System.Windows.Markup;
using System.Windows.Media.Media3D;

namespace ViewModel
{
    public interface IErrorSender
    {
        void SendError(string message);
    }

    public interface IFileDialog
    {
        public string OpenFileDialog();
        public string SaveFileDialog();
    }

    public class ViewData : ViewModelBase, IDataErrorInfo
    {
        public double Left { get; set; } = 0;
        public double Right { get; set; } = 10;
        public int NumOfNodes { get; set; } = 3;
        public bool UniformityCheck { get; set; } = true;
        public FValuesEnum Func { get; set; } = FValuesEnum.MyFunction1;
        public double LeftDer { get; set; } = 0;
        public double RightDer { get; set; } = 0;
        public int SplineNodes { get; set; } = 4;
        public V1DataArray V1DataArray { get; set; }
        public SplineData SplineData { get; set; }
        public PlotModel? splinePlot { get; set; }

        private readonly IErrorSender errorSender;
        private readonly IFileDialog fileDialog;

        public ICommand SaveFileCommand { get; private set; }
        public ICommand LoadFromControlsCommand { get; private set; }
        public ICommand LoadFromFileCommand { get; private set; }
        public ICommand ComputeSplineCommand { get; private set; }

        public ViewData(IErrorSender errorSender, IFileDialog fileDialog)
        {
            this.errorSender = errorSender;
            this.fileDialog = fileDialog;
            V1DataArray = new V1DataArray("DataArray", DateTime.Now ,0, 1, 2, true, MyFunctions.MyFunction1);
            SplineData = null;
            SaveFileCommand = new RelayCommand(o => { SaveFileCommandHandler(); }, o => CanSaveFileCommandHandler());
            LoadFromControlsCommand = new RelayCommand(o => { LoadFromControlsCommandHandler(); }, o => CanLoadFromControlsCommandHandler());
            LoadFromFileCommand = new RelayCommand(o => { LoadFromFileCommandHandler(); }, o => CanLoadFromFileCommandHandler());
            ComputeSplineCommand = new RelayCommand(o => { ComputeSplineCommandHandler(); }, o => CanComputeSplineCommandHandler());
        }

        private void SaveFileCommandHandler()
        {
            try
            {
                LoadFromControls();
                string filename = fileDialog.SaveFileDialog();
                if (!string.IsNullOrEmpty(filename))
                {
                    if (V1DataArray != null)
                        V1DataArray.Save(filename);
                }
            }
            catch (Exception ex)
            {
                errorSender.SendError("Ошибка:" + ex.Message);
            }
        }

        private bool CanSaveFileCommandHandler()
        {
            return string.IsNullOrEmpty(this["Left"]) && string.IsNullOrEmpty(this["Right"]) && string.IsNullOrEmpty(this["NumOfNodes"]);
        }

        private void LoadFromControlsCommandHandler()
        {
            try
            {
                LoadFromControls();
                int result = ComputeSpline();
                DrawSpline();
                RaisePropertyChanged("V1DataArray");
                RaisePropertyChanged("SplineData");
                RaisePropertyChanged("splinePlot");
            }
            catch (Exception ex)
            {
                errorSender.SendError("Ошибка:" + ex.Message);
            }
        }
        
        private bool CanLoadFromControlsCommandHandler()
        {
            return string.IsNullOrEmpty(this["Left"]) && string.IsNullOrEmpty(this["Right"]) && string.IsNullOrEmpty(this["NumOfNodes"]) && string.IsNullOrEmpty(this["SplineNodes"]);
        }

        private void LoadFromFileCommandHandler()
        {
            try
            {
                string filename = fileDialog.OpenFileDialog();
                if (!string.IsNullOrEmpty(filename))
                {
                    LoadFromFile(filename);
                    RaisePropertyChanged("Left");
                    RaisePropertyChanged("Right");
                    RaisePropertyChanged("NumOfNodes");
                    RaisePropertyChanged("UniformityCheck");
                    RaisePropertyChanged("Func");
                    RaisePropertyChanged("V1DataArray");
                }
                if (ComputeSplineCommand.CanExecute(this))
                    ComputeSplineCommand.Execute(this);
            }
            catch (Exception ex)
            {
                errorSender.SendError("Ошибка:" + ex.Message);
            }
        }
        private bool CanLoadFromFileCommandHandler()
        {
            return string.IsNullOrEmpty(this["SplineNodes"]);
        }

        private void ComputeSplineCommandHandler()
        {
            int result = ComputeSpline();
            DrawSpline();
            RaisePropertyChanged("V1DataArray");
            RaisePropertyChanged("SplineData");
            RaisePropertyChanged("splinePlot");
        }
        private bool CanComputeSplineCommandHandler()
        {
            return (string.IsNullOrEmpty(this["Left"]) && string.IsNullOrEmpty(this["Right"]) && string.IsNullOrEmpty(this["NumOfNodes"]) && string.IsNullOrEmpty(this["SplineNodes"])) && (V1DataArray != null);
        }

        private void DrawSpline()
        {
            try
            {
                var oxyPlotMod = new OxyPlotModel(SplineData, V1DataArray);
                this.splinePlot = oxyPlotMod.plotModel;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка отрисовки сплайна\n" + ex.Message);
            }
        }

        public void LoadFromControls()
        {
            try
            {
                FValues func = GetFuncFromEnum(Func);
                V1DataArray = new V1DataArray("Data", DateTime.Now, NumOfNodes, Left, Right , UniformityCheck, func);
            }
            catch (Exception)
            {
                throw new Exception("Неправильный формат ввода!"); ;
            }
        }

        public void LoadFromFile(string filename)
        {
            try
            {
                V1DataArray = new V1DataArray("Data", DateTime.Now,filename);
                Left = V1DataArray.Left;
                Right = V1DataArray.Right;
                NumOfNodes = V1DataArray.NumOfNodes;
                UniformityCheck = V1DataArray.UniformityCheck_;
                Func = GetEnumFromFunc(V1DataArray.F_);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ComputeSpline()
        {
            try
            {
                SplineData = new SplineData(V1DataArray, SplineNodes, LeftDer, RightDer);
                return SplineData.CreateSpline();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static FValues GetFuncFromEnum(FValuesEnum fValuesEnum)
        {
            FValues? fValues = fValuesEnum switch
            {
                FValuesEnum.MyFunction1 => MyFunctions.MyFunction1,
                FValuesEnum.MyFunction2 => MyFunctions.MyFunction2,
                FValuesEnum.MyFunction3 => MyFunctions.MyFunction3,
                _ => MyFunctions.MyFunction3,
            };
            return fValues;
        }

        public static FValuesEnum GetEnumFromFunc(FValues func)
        {
            string funcName = func.Method.Name;
            FValuesEnum fValuesEnum = funcName switch
            {
                "MyFunction1" => FValuesEnum.MyFunction1,
                "MyFunction2" => FValuesEnum.MyFunction2,
                "MyFunction3" => FValuesEnum.MyFunction2,
                _ => FValuesEnum.MyFunction1
            };
            return fValuesEnum;
        }

        public string Error
        {
            get { return "Входные данные некорректны!"; }
        }

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;
                switch (columnName)
                {
                    case "Left":
                        if (Left > Right)
                            result = "Левая граница должна быть меньше правой";
                        break;
                    case "Right":
                        if (Right < Right)
                            result = "Правая граница должна быть больше левой";
                        break;
                    case "NumOfNodes":
                        if (NumOfNodes <= 2)
                            result = "Число узлов сетки, на которой заданы дискретные значения функции, больше или равно 3!";
                        break;
                    case "SplineNodes":
                        if (SplineNodes <= 3)
                            result = "Число узлов равномерной сетки, на которой вычисляются значения сплайна, больше 3!";
                        break;
                }
                return result;
            }
        }
    }
}
