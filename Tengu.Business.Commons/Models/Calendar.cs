using Tengu.Business.Commons.Objects;

namespace Tengu.Business.Commons.Models
{
    public class Calendar
    {

        public Dictionary<TenguWeekDays, List<CalendarEntryModel>> DaysDictionary { get; } = new Dictionary<TenguWeekDays, List<CalendarEntryModel>>()
        {
            { TenguWeekDays.Monday, new List<CalendarEntryModel>() },
            { TenguWeekDays.Tuesday, new List<CalendarEntryModel>() },
            { TenguWeekDays.Wednesday, new List<CalendarEntryModel>() },
            { TenguWeekDays.Thursday, new List<CalendarEntryModel>() },
            { TenguWeekDays.Friday, new List<CalendarEntryModel>() },
            { TenguWeekDays.Saturday, new List<CalendarEntryModel>() },
            { TenguWeekDays.Sunday, new List<CalendarEntryModel>() },
            { TenguWeekDays.None, new List<CalendarEntryModel>() },
        };

        public TenguHosts Host { get; set; } = TenguHosts.None;

    }
}
