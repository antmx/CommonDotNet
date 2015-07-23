using System;

namespace Netricity.Common
{
	/// <summary>
	/// Represents a DateTime that can be independant of the system clock.
	/// </summary>
	public static class SystemTime
	{
		private static readonly Func<DateTime> _defaultNowFunction = () => DateTime.Now;
		private static Func<DateTime> _overidingNowFunction;

		public static Func<DateTime> NowFunction
		{
			get { return _overidingNowFunction ?? _defaultNowFunction; }
			set
			{
				if (value == null)
					throw new ArgumentNullException(nameof(value));

				_overidingNowFunction = value;
			}
		}

		public static DateTime Now
		{
			get { return NowFunction(); }
		}

		public static DateTime Today
		{
			get { return NowFunction().Date; }
		}

		public static void ResetNowFuncton()
		{
			_overidingNowFunction = null;
		}
	}
}