//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Data;

//namespace PL
//{
//    public class ConvertLongetitude : IValueConverter
//    {
//        ConvertDecimalToDegMinSec minSec;
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            minSec = new();
//            if (value == null)
//                return "";
//            double location = (double)value;
//            return minSec.ConvertDecimalToDegMinSecFunction(location) + "E";
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//    public class ConertLattedue : IValueConverter
//    {
//        ConvertDecimalToDegMinSec minSec;

//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            minSec = new();
//            if (value == null)
//                return "";
//            double location = (double)value;
//            return minSec.ConvertDecimalToDegMinSecFunction(location) + "N";
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//    public class ConvertDecimalToDegMinSec
//    {
//        /// <summary>
//        /// This function converts coordinate from decimal display to display using minute and second degrees.
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></string>
//        public string ConvertDecimalToDegMinSecFunction(double value)
//        {
//            int deg = (int)value;
//            value = Math.Abs(value - deg);
//            int min = (int)(value * 60);
//            value = value - (double)min / 60;
//            int sec = (int)(value * 3600);
//            value = value - (double)sec / 3600;
//            return deg.ToString() + '°' + min.ToString() + "'" + sec.ToString() + "''";
//        }
//    }
//}
