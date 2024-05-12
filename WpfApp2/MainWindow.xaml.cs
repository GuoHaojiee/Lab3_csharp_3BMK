using ConsoleApp4;
using ConsoleApp4.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ViewModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            ViewData viewData = new ViewData(new MessageBoxErrorSender(), new SaveAndLoadFileDialog());
            this.DataContext = viewData;
        }

        public class MessageBoxErrorSender : IErrorSender
        {
            public void SendError(string message) => MessageBox.Show(message);
        }

        public class SaveAndLoadFileDialog : IFileDialog
        {
            public string OpenFileDialog()
            {
                try
                {
                    string resultFileName = "";
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == true)
                        resultFileName = openFileDialog.FileName;
                    return resultFileName;
                }
                catch (Exception ex)
                {
                    throw new Exception("Не удалось открыть файл: " + ex.Message);
                }
            }
            public string SaveFileDialog()
            {
                try
                {
                    string resultFileName = "";
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (saveFileDialog.ShowDialog() == true)
                        resultFileName = saveFileDialog.FileName;
                    return resultFileName;
                }
                catch (Exception ex)
                {
                    throw new Exception("Не удалось сохранить файл: " + ex.Message);
                }
            }
        }

    }

    public class BoundsTextBoxConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string result = values[0].ToString() + ";" + values[1].ToString();
                return result;
            }
            catch
            {
                MessageBox.Show("Неправильный ввод!");
                return values;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            try
            {
                string[] splitValues = ((string)value).Split(';');
                double[] bounds = { System.Convert.ToDouble(splitValues[0]), System.Convert.ToDouble(splitValues[1]) };
                return new object[] { bounds[0], bounds[1] };
            }
            catch
            {
                return new object[] { 0, 1 };
            }
        }
    }

    public class IntegerTextBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? converted = value.ToString();
            if (converted != null)
                return converted;
            else
            {
                MessageBox.Show("Неправильный ввод!");
                return "0";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = value as string;
            int output;
            if (int.TryParse(input, out output))
                return output;
            else
                return DependencyProperty.UnsetValue;
        }
    }

    public class RadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }

    public enum FValuesEnum
    {
        MyFunction1, MyFunction2, MyFunction3,
    };

    public class DoubleTextBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? converted = value.ToString();
            if (converted != null)
                return converted;
            else
            {
                MessageBox.Show("Неправильный ввод!");
                return "0";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = value as string;
            double output;
            if (double.TryParse(input, out output))
                return output;
            else
                return DependencyProperty.UnsetValue;
        }
    }
    
}