using System;

namespace Orion.API.Models
{
	/// <summary>日期時間區段</summary>
	public class DateTimeSection
	{
		/// <summary>開始日期時間</summary>
		public DateTime Start { get; set; }

		/// <summary>結束日期時間</summary>
		public DateTime End { get; set; }

		/// <summary>日期時間長度</summary>
		public TimeSpan Duration { get { return End - Start; } }


		/// <summary></summary>
		public DateTimeSection() { }

		/// <summary></summary>
		public DateTimeSection(DateTimeSection from)
		{
			Start = from.Start;
			End = from.End;
		}

		/// <summary></summary>
		public DateTimeSection(DateTime start, DateTime end)
		{
			Start = start;
			End = end;
		}

		/// <summary></summary>
		public DateTimeSection(string start, string end)
		{
			Start = DateTime.Parse(start);
			End = DateTime.Parse(end);
		}


		/// <summary></summary>
		public override bool Equals(object obj)
		{
			var target = obj as DateTimeSection;
			if(target == null) { return false; }

			return Start == target.Start && End == target.End;
		}

		/// <summary></summary>
		public override int GetHashCode()
		{
			return Start.GetHashCode() ^ End.GetHashCode();
		}

		/// <summary></summary>
		public override string ToString()
		{
			return $"{Start:yyyy-MM-dd HH:mm:ss.fff} ~ {End:yyyy-MM-dd HH:mm:ss.fff}";
		}


	}
}
