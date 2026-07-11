using System;
using TamilDesktopWidget.Data;
using TamilDesktopWidget.Models;

namespace TamilDesktopWidget.Services
{
    public class TamilCalendarService
    {
        public TamilDateModel GetTamilDate(DateTime date)
        {
            TamilDateModel model = new TamilDateModel();

            int tamilMonthIndex = GetTamilMonthIndex(date);

            var start = TamilMonthRules.StartDates[tamilMonthIndex];

            DateTime startDate = new DateTime(date.Year, start.Month, start.Day);

            // If today's date is before this month's start,
            // move to the previous Tamil month.
            if (date < startDate)
            {
                tamilMonthIndex--;

                if (tamilMonthIndex < 0)
                {
                    tamilMonthIndex = 11;
                    start = TamilMonthRules.StartDates[tamilMonthIndex];
                    startDate = new DateTime(date.Year - 1, start.Month, start.Day);
                }
                else
                {
                    start = TamilMonthRules.StartDates[tamilMonthIndex];

                    int year = start.Month >= 4 ? date.Year : date.Year - 1;

                    startDate = new DateTime(year, start.Month, start.Day);
                }
            }

            model.TamilDay = (date - startDate).Days + 1;

            model.TamilMonth = TamilMonths.Names[tamilMonthIndex];

            model.TamilWeekDay = GetTamilWeekDay(date.DayOfWeek);

            model.TamilYear = GetTamilYear(date);

            return model;
        }

        private int GetTamilMonthIndex(DateTime date)
        {
            for (int i = TamilMonthRules.StartDates.Length - 1; i >= 0; i--)
            {
                var rule = TamilMonthRules.StartDates[i];

                int year = rule.Month >= 4 ? date.Year : date.Year + 1;

                DateTime start = new DateTime(year, rule.Month, rule.Day);

                if (date >= start)
                    return i;
            }

            return 11;
        }

        private string GetTamilWeekDay(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Sunday => "ஞாயிறு",
                DayOfWeek.Monday => "திங்கள்",
                DayOfWeek.Tuesday => "செவ்வாய்",
                DayOfWeek.Wednesday => "புதன்",
                DayOfWeek.Thursday => "வியாழன்",
                DayOfWeek.Friday => "வெள்ளி",
                DayOfWeek.Saturday => "சனி",
                _ => ""
            };
        }

        private string GetTamilYear(DateTime date)
        {
            int tamilYear = date.Year - 1987;

            while (tamilYear < 0)
                tamilYear += 60;

            tamilYear %= 60;

            return TamilYears.Names[tamilYear];
        }
    }
}