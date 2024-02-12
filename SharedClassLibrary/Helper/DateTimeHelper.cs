using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.Helper
{
    public static class DateTimeHelper
     {

        public static string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd");
        }

        public static string FormatTime(DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

    }



}
