using System;
using System.Globalization;

namespace Netricity.Common
{
   /// <summary>
   /// System.DateTime extension methods.
   /// </summary>
   public static class DateTimeExtensions
   {
      /// <summary>
      /// d MMM yyyy
      /// </summary>
      public const string DateFormatShort = "d MMM yyyy";

      /// <summary>
      /// d MM yy (jquery renders this as d MMMM yyyy)
      /// </summary>
      public const string DateFormatShortJquery = "d MM yy";

      /// <summary>
      /// HH:mm
      /// </summary>
      public const string TimeFormatJquery = "HH:mm";

      /// <summary>
      /// d MMMM yyyy
      /// </summary>
      public const string DateFormatMedium = "d MMMM yyyy";

      /// <summary>
      /// dddd, d MMMM yyyy
      /// </summary>
      public const string DateFormatLong = "dddd, d MMMM yyyy";

      /// <summary>
      /// yyyy-MM-dd
      /// </summary>
      public const string DateFormatHtml5 = "yyyy-MM-dd";

      /// <summary>
      /// d MMM yyyy HH:mm tt
      /// </summary>
      public const string DateTimeFormat = "d MMM yyyy HH:mm tt";

      /// <summary>
      /// HH:mm tt
      /// </summary>
      public const string TimeFormat = "HH:mm tt";

      /// <summary>
      /// HH:mm
      /// </summary>
      public const string TimeFormatShort = "HH:mm";

      /// <summary>
      /// d MMM yyyy HH:mm
      /// </summary>
      public const string DateTimeFormatShort = DateFormatShort + " " + TimeFormatShort;

      /// <summary>
      /// Returns a string of JavaScript code that constructs a Date object.
      /// e.g. a .Net DateTime of '31st Dec 2014, 23:59:59.999' would return 'new Date(2014, 11, 31, 23, 59, 59, 999)'
      /// </summary>
      /// <param name="date">The date.</param>
      public static string ToJsDateCtor(this DateTime date)
      {
         // Date(2014, 0, 1, 12, 30, 0, 0)
         var strDate = string.Format(
            "new Date({0}, {1}, {2}, {3}, {4}, {5}, {6})",
            date.Year,
            date.Month - 1,
            date.Day,
            date.Hour,
            date.Minute,
            date.Second,
            date.Millisecond);

         return strDate;
      }

      /// <summary>
      /// Calculates the age at the current date from the given birth date.
      /// </summary>
      /// <param name="dob">Date of birth</param>
      public static int AgeNow(this DateTime dob)
      {
         return AgeAt(dob, SystemTime.Now);
      }

      /// <summary>
      /// Calculates the age from the birth date at the given date.
      /// </summary>
      /// <param name="dob">Date of birth</param>
      /// <param name="atDate">The date at which to calculate the age</param>
      public static int AgeAt(this DateTime dob, DateTime atDate)
      {
         int age = atDate.Year - dob.Year;

         if (atDate.Month < dob.Month || (atDate.Month == dob.Month && atDate.Day < dob.Day))
            age--;

         return age;
      }

      /// <summary>
      /// Gets an approximate DOB based on the given age at today's date.
      /// </summary>
      /// <param name="age">e.g. 21</param>
      public static DateTime DobFromAge(int age)
      {
         return DobFromAge(age, SystemTime.Today);
      }

      /// <summary>
      /// Gets an approximate DOB based on the given age at the given date.
      /// </summary>
      /// <param name="age">e.g. 21</param>
      /// <param name="atDate">e.g. today</param>
      public static DateTime DobFromAge(int age, DateTime atDate)
      {
         int approxYob = atDate.AddYears(-(age)).Year;

         return new DateTime(approxYob, 1, 1);
      }

      public static int? ApproxAgeFromYob(int? yob)
      {
         var today = SystemTime.Today;

         if (!yob.HasValue || yob <= 0 || yob > today.Year)
            return null;

         if (yob == today.Year)
            return 0;

         return today.AddYears(-((int)yob)).Year;
      }

      public static int? ApproxYobFromAge(int? approxAge)
      {
         var today = SystemTime.Today;

         if (!approxAge.HasValue || approxAge < 0 || approxAge > today.Year)
            return null;

         if (approxAge == today.Year)
            return today.Year;


         return today.AddYears(-((int)approxAge)).Year;
      }

      /// <summary>
      /// Gets the suffix for the date's day, e.g. 'st' if date is 1st of January.
      /// </summary>
      /// <param name="date">The date.</param>
      public static string GetDayOfMonthSuffix(this DateTime date)
      {
         switch (date.Day)
         {
            case 1:
            case 21:
            case 31:
               return "st";

            case 2:
            case 22:
               return "nd";

            case 3:
            case 23:
               return "rd";

            default:
               return "th";
         }
      }

      /// <summary>
      /// Gets the extended DateTime truncated to whole minutes.
      /// e.g. 1 May 2014 12:34:56 returns 1 May 2014 12:34:00
      /// </summary>
      /// <param name="dateTime">The date time.</param>
      public static DateTime TruncateToWholeMinutes(this DateTime dateTime)
      {
         dateTime = dateTime.Truncate(TimeSpan.FromMinutes(1)); // Truncate to whole minute

         return dateTime;
      }

      private static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
      {
         if (timeSpan == TimeSpan.Zero)
            return dateTime; // Or could throw an ArgumentException

         return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
      }

      public static bool DateTimeTryParseExact(string str, string dateFormat, ref DateTime result)
      {
         return DateTime.TryParseExact(str, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
      }


      public static bool DateTimeTryParseExact(string str, string[] dateFormats, ref DateTime result)
      {
         return DateTime.TryParseExact(str, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
      }

      public static string ToJavascriptDate(this DateTime dateTime)
      {
         return dateTime
               .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
               .TotalMilliseconds
               .ToString();
      }
   }
}
