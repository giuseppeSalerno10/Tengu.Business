using Tengu.Business.Commons.Objects;

namespace Tengu.Business.Commons.Models
{
    public class Calendar
    {

        public Dictionary<WeekDays, List<CalendarEntryModel>> DaysDictionary { get; } = new Dictionary<WeekDays, List<CalendarEntryModel>>()
        {
            { WeekDays.Monday, new List<CalendarEntryModel>() },
            { WeekDays.Tuesday, new List<CalendarEntryModel>() },
            { WeekDays.Wednesday, new List<CalendarEntryModel>() },
            { WeekDays.Thursday, new List<CalendarEntryModel>() },
            { WeekDays.Friday, new List<CalendarEntryModel>() },
            { WeekDays.Saturday, new List<CalendarEntryModel>() },
            { WeekDays.Sunday, new List<CalendarEntryModel>() },
            { WeekDays.None, new List<CalendarEntryModel>() },
        };

        public Hosts Host { get; set; } = Hosts.None;

    }
}
