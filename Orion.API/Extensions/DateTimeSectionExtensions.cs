using Orion.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Orion.API.Extensions
{
	/// <summary>日期時間區段 的 Extension</summary>
	public static class DateTimeSectionExtensions
	{

		/// <summary>取得重疊日期時間區段</summary>
		public static DateTimeSection Overlap(this DateTimeSection sectionA, DateTimeSection sectionB) 
		{
			return new DateTimeSection
			{
				Start = OrionUtils.Max(sectionA.Start, sectionB.Start),
				End = OrionUtils.Min(sectionA.End, sectionB.End),
			};
		}



		/// <summary>將日期時間區段清單反轉成空缺區段</summary>
		public static IEnumerable<DateTimeSection> ComplementSection(this IEnumerable<DateTimeSection> source)
		{
			source = source.Where(x => x != null).OrderBy(x => x.Start);
			var cursor = new DateTimeSection { Start = DateTime.MinValue };

			foreach (var item in source)
			{
				cursor.End = item.Start;

				yield return cursor;
				cursor = new DateTimeSection { Start = item.End };
			}

			cursor.End = DateTime.MaxValue;
			yield return cursor;
		}



		/// <summary>取得日期時間區段清單的交集</summary>
		public static IEnumerable<DateTimeSection> IntersectSection(this IEnumerable<DateTimeSection> source, IEnumerable<DateTimeSection> target)
		{
			source = source.Where(x => x != null && x.Start < x.End).OrderBy(x => x.End);
			target = target.Where(x => x != null && x.Start < x.End).OrderBy(x => x.End);

			using (IEnumerator<DateTimeSection> sIter = source.GetEnumerator(), tIter = target.GetEnumerator())
			{
				bool hasS = sIter.MoveNext();
				bool hasT = tIter.MoveNext();

				while (hasS && hasT)
				{
					DateTimeSection overlap = Overlap(sIter.Current, tIter.Current);
					if (overlap.End == sIter.Current.End) { hasS = sIter.MoveNext(); }
					if (overlap.End == tIter.Current.End) { hasT = tIter.MoveNext(); }

					if (overlap.Start < overlap.End) { yield return overlap; }
				}
			}
		}




		/// <summary>取得日期時間區段清單的聯集</summary>
		public static IEnumerable<DateTimeSection> UnionSection(this IEnumerable<DateTimeSection> source, IEnumerable<DateTimeSection> target)
		{
			source = source.Concat(target).Where(x => x != null).OrderBy(x => x.Start);

			using (IEnumerator<DateTimeSection> iter = source.GetEnumerator())
			{
				if (!iter.MoveNext()) { yield break; }
				var cursor = new DateTimeSection(iter.Current);

				while (iter.MoveNext())
				{
					if (iter.Current.Start <= cursor.End)
					{
						cursor.End = OrionUtils.Max(iter.Current.End, cursor.End);
						continue;
					}

					yield return cursor;
					cursor = new DateTimeSection(iter.Current);
				}

				yield return cursor;
			}
		}



		/// <summary>取得日期時間區段清單的差集</summary>
		public static IEnumerable<DateTimeSection> ExceptSection(this IEnumerable<DateTimeSection> first, IEnumerable<DateTimeSection> second)
		{
			return first.IntersectSection(second.ComplementSection());
		}



		/// <summary>統計日期時間區段長度</summary>
		public static TimeSpan SumDuration(this IEnumerable<DateTimeSection> source)
		{
			if (!source.Any()) { return TimeSpan.Zero; }
			return source.Select(x => x.Duration).Aggregate((sum, next) => sum + next);
		}



		/// <summary>檢查日期時間區段是否重疊</summary>
		public static void CheckOverlap(this IEnumerable<DateTimeSection> source)
		{
			source = source.Where(x => x != null).OrderBy(x => x.Start);

			using (IEnumerator<DateTimeSection> iter = source.GetEnumerator())
			{
				if (!iter.MoveNext()) { return; }
				DateTimeSection prev = iter.Current;

				while (iter.MoveNext())
				{
					if(prev.End <= iter.Current.Start) { continue; }
					throw new DataException($"Section overlap {prev} and {iter.Current}");
				}
			}
		}






	}
}
