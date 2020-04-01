using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Helpers
{
    public static class JalaliDate
    {
        public static string GetJalaliDate(this DateTime date, JalaliDateFormat format)
        {
            PersianCalendar jc = new PersianCalendar();
            int year = jc.GetYear(date);
            int month = jc.GetMonth(date);
            int day = jc.GetDayOfMonth(date);
            string dayName = jc.GetDayOfWeek(date).GetJalaliName();
            string monthName = GetJalaliMonthName(month);
            string str;
            switch (format)
            {
                case JalaliDateFormat.SimpleDate:
                    str = year + "/" + month + "/" + day;
                    break;
                case JalaliDateFormat.Zarvan:
                    str = year + "/" + (month < 10 ? ("0" + month) : month.ToString()) + "/" + (day < 10 ? ("0" + day) : day.ToString());
                    break;
                case JalaliDateFormat.SimpleDateTime:
                    str = year + "/" + month + "/" + day + ", " + date.ToShortTimeString();
                    break;
                case JalaliDateFormat.LongDate:
                    str = dayName + " " + day + " " + monthName + " " + year;
                    break;
                case JalaliDateFormat.LongDateTime:
                    str = dayName + " " + day + " " + monthName + " " + year + ", ساعت " + date.ToLongTimeString();
                    break;
                case JalaliDateFormat.Elpased:
                    var span = (DateTime.Now - date);
                    double value = span.TotalSeconds;
                    if (value < 60)
                        str = " لحظاتی پیش";
                    else if (value < 3600)
                        str = Math.Round(span.TotalMinutes) + " دقیقه پیش";
                    else if (value < 86400)
                        str = Math.Round(span.TotalHours) + " ساعت پیش";
                    else
                        str = Math.Round(span.TotalDays) + " روز پیش";
                    break;
                case JalaliDateFormat.Day:
                    str = day.ToString();
                    break;
                default:
                    str = year + "/" + month + "/" + day;
                    break;
            }
            return str;
        }
        public static string GetJalaliName(this DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Friday => "جمعه",
                DayOfWeek.Monday => "دوشنبه",
                DayOfWeek.Saturday => "شنبه",
                DayOfWeek.Sunday => "یک‌شنبه",
                DayOfWeek.Thursday => "پنج‌شنبه",
                DayOfWeek.Tuesday => "سه‌شنبه",
                DayOfWeek.Wednesday => "چهارشنبه",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        public static string GetJalaliMonthName(int month)
        {
            return month switch
            {
                1 => "فروردین",
                2 => "اردیبهشت",
                3 => "خرداد",
                4 => "تیر",
                5 => "مرداد",
                6 => "شهریور",
                7 => "مهر",
                8 => "آبان",
                9 => "آذر",
                10 => "دی",
                11 => "بهمن",
                12 => "اسفند",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    public enum JalaliDateFormat
    {
        SimpleDate,
        SimpleDateTime,
        LongDate,
        LongDateTime,
        Elpased,
        Zarvan,
        Day
    }
}
