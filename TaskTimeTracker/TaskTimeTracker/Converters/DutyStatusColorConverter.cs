using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using TaskTimeTracker.Common.Model;

namespace TaskTimeTracker.Converters
{
    public class DutyStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DutyStatus status = (DutyStatus)value;

            switch (status)
            {
                case DutyStatus.Ongoing:
                    return ColorConverter.ConvertFromString("#BB4D4D");
                case DutyStatus.Paused:
                    return ColorConverter.ConvertFromString("#CFC739");
                case DutyStatus.Completed:
                    return ColorConverter.ConvertFromString("#49BF4B");
                default:
                    return Colors.Pink;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
