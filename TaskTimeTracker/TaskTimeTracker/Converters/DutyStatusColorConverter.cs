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
                    return Colors.Red;
                case DutyStatus.Paused:
                    return Colors.Orange;
                case DutyStatus.Completed:
                    return Colors.Green;
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
