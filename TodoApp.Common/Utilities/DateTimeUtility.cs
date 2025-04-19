using System.Globalization;

namespace TodoApp.Common.Utilities;

public static class DateTimeUtility
{
    public static string GetUtcNow(this DateOnly? date)
    {
        if (date == null) return string.Empty;

        DateTime? d = date?.ToDateTime(TimeOnly.MinValue);
        PersianCalendar pc = new PersianCalendar();
        return string.Format("{0}/{1}/{2}", pc.GetYear(d ?? DateTime.Now), pc.GetMonth(d ?? DateTime.Now),
            pc.GetDayOfMonth(d ?? DateTime.Now));
    }
}