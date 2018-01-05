using System;
using System.Collections.Generic;
using System.Linq;

namespace Orion.API.Models
{
	/// <summary>日曆參數</summary>
	public class CalendarParams
	{
		private string[] _weekName = new []
		{ 
			"星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" 
		};


		private int _year = DateTime.Today.Year;
		private int _month = DateTime.Today.Month;

		private DateTime _firstDate;
		private DateTime _lastDate;

		private DayOfWeek _weekDayFirst = DayOfWeek.Sunday;
		private DayOfWeek _weekDayLast = DayOfWeek.Saturday;


		/// <summary></summary>
		public CalendarParams()
		{
			updateStartEnd();
		}


		private void updateStartEnd()
		{
			_firstDate = new DateTime(_year, _month, 1);
			_lastDate = _firstDate.AddMonths(1).AddDays(-1);
		}


		/// <summary>年</summary>
		public int Year {
			get { return _year; }
			set { _year = value; updateStartEnd(); }
		}

		/// <summary>月</summary>
		public int Month {
			get { return _month; }
			set { _month = value; updateStartEnd(); }
		}



		/// <summary>每周第一天</summary>
		public DayOfWeek WeekDayFirst 
		{ 
			get { return _weekDayFirst; }
			set 
			{ 
				_weekDayFirst = value; 
				_weekDayLast = (DayOfWeek)((int)(value + 6) % 7);
			} 
		}

		/// <summary>每周最後一天</summary>
		public DayOfWeek WeekDayLast { get { return _weekDayLast; } }

		/// <summary>當月第一天</summary>
		public DateTime FirstDate { get { return _firstDate; } }

		/// <summary>當月最後一天</summary>
		public DateTime LastDate { get { return _lastDate; } }


		/// <summary>補齊行事曆第一天</summary>
		public DateTime PadFirstDate
		{ 
			get {
				DateTime firstPad = _firstDate;
				while (firstPad.DayOfWeek != _weekDayFirst) { firstPad = firstPad.AddDays(-1); }
				return firstPad;
			} 
		}

		/// <summary>補齊行事曆最後一天</summary>
		public DateTime PadLastDate 
		{ 
			get {
				DateTime lastPad = _lastDate;
				while (lastPad.DayOfWeek != _weekDayLast) { lastPad = lastPad.AddDays(1); }
				return lastPad;
			}
		}

		/// <summary>上個月</summary>
		public DateTime PreviousMonth { get { return _firstDate.AddMonths(-1); } }

		/// <summary>下個月</summary>
		public DateTime NextMonth { get { return _firstDate.AddMonths(1); } }


		/// <summary>以指定起始星期為基礎，產生本周星期清單</summary>
		public string[] WeekNameItems
		{
			get
			{
				int weekDay = (int)_weekDayFirst;
				return _weekName.Skip(weekDay).Concat(_weekName.Take(weekDay)).ToArray();
			}
		}

		
		/// <summary>取得從 (目前年份-10) 開始，往後取(目前年份 + 9)數量 的陣列</summary>
		public int[] GetYearItems(int count = 20)
		{
			return Enumerable.Range(_year - (count / 2), count).ToArray();
		}

		/// <summary>取得月份的陣列</summary>
		public int[] GetMonthItems()
		{
			return Enumerable.Range(1, 12).ToArray();
		}

		/// <summary>是否為今天</summary>
		public bool IsToday(DateTime date) 
		{
			return date == DateTime.Today; 
		}

		/// <summary>是否為周末</summary>
		public bool IsWeekend(DateTime date) 
		{ 			
			return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
		}

		/// <summary>是否於當月區間</summary>
		public bool IsInMonth(DateTime date) 
		{ 
			return _firstDate <= date && date <= _lastDate;
		}


		/// <summary>是否為每周第一天</summary>
		public bool IsWeekDayFirst(DateTime date)
		{
			return WeekDayFirst == date.DayOfWeek;
		}

		/// <summary>是否為每周最後一天</summary>
		public bool IsWeekDayLast(DateTime date)
		{
			return WeekDayLast == date.DayOfWeek;
		}


		/// <summary>列舉行事曆的每一天</summary>
		public IEnumerable<DateTime> EnumerateDates()
		{
			for (var date = FirstDate; date <= LastDate; date = date.AddDays(1)) 
			{
				yield return date;
			}
		}


		/// <summary>列舉補齊行事曆的每一天</summary>
		public IEnumerable<DateTime> EnumeratePadDates()
		{
			for (var date = PadFirstDate; date <= PadLastDate; date = date.AddDays(1))
			{
				yield return date;
			}
		}
		
	}
}
