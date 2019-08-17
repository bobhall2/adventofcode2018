using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2018.XUnitTestProject
{
	public class Day04
	{
		[Theory]
		[InlineData("[1518-03-18 23:57] Guard #857 begins shift", "1518-03-18T23:57:00Z", "1518-03-19", default, 857)]
		[InlineData("[1518-11-01 00:00] Guard #10 begins shift", "1518-11-01T00:00:00Z", "1518-11-01", default, 10)]
		[InlineData("[1518-11-01 00:05] falls asleep", "1518-11-01T00:05:00Z", "1518-11-01", 5, default)]
		[InlineData("[1518-11-01 00:25] wakes up", "1518-11-01T00:25:00Z", "1518-11-01", 25, default)]
		public void Day04_ParseLine(string line, string expectedDateTimeString, string expectedDateString, int? expectedMinute, int? expectedGuardId)
		{
			var logEntry = LogEntry.Parse(line);

			Assert.Equal(
				DateTime.Parse(expectedDateTimeString, styles: DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal),
				logEntry.DateTime);
			Assert.Equal(DateTime.Parse(expectedDateString), logEntry.Date);
			Assert.Equal(expectedMinute, logEntry.Minute);
			Assert.Equal(expectedGuardId, logEntry.GuardId);
		}

		[Fact]
		public void Day04_Group()
		{
			var lines = new[]
			{
				"[1518-11-01 00:00] Guard #10 begins shift",
				"[1518-11-01 00:05] falls asleep",
				"[1518-11-01 00:25] wakes up",
				"[1518-11-01 00:30] falls asleep",
				"[1518-11-01 00:55] wakes up",
				"[1518-11-01 23:58] Guard #99 begins shift",
				"[1518-11-02 00:40] falls asleep",
				"[1518-11-02 00:50] wakes up",
				"[1518-11-03 00:05] Guard #10 begins shift",
				"[1518-11-03 00:24] falls asleep",
				"[1518-11-03 00:29] wakes up",
				"[1518-11-04 00:02] Guard #99 begins shift",
				"[1518-11-04 00:36] falls asleep",
				"[1518-11-04 00:46] wakes up",
				"[1518-11-05 00:03] Guard #99 begins shift",
				"[1518-11-05 00:45] falls asleep",
				"[1518-11-05 00:55] wakes up",
			};

			var dictionary = new Dictionary<(int, int), int>();

			var logEntries = lines.Select(LogEntry.Parse);

			var shifts = Shift.Parse(logEntries);

			var asleep = from s in shifts
						 group s by s.GuardId into gg
						 let guardId = gg.Key
						 let totalMinutesAsleep = gg.Sum(s => s.TotalMinutesAsleep)
						 orderby totalMinutesAsleep descending
						 select (guardId, totalMinutesAsleep);

			var sleepiest = asleep.First();

			Assert.Equal(10, sleepiest.guardId);
			Assert.Equal(50, sleepiest.totalMinutesAsleep);

			var query = from shift in shifts
						where shift.GuardId == sleepiest.guardId
						from minute in shift.AsleepMinutes
						group minute by minute into gg
						let minute = gg.Key
						let count = gg.Count()
						orderby count descending
						select (minute, count);

			var (sleepiestMinute, sleepiestMinuteCount) = query.First();

			Assert.Equal(24, sleepiestMinute);
			Assert.Equal(2, sleepiestMinuteCount);
		}

		[Theory]
		[InlineData("sleeplog.dat", 39_422)]
		public async Task Day04_ParseFile(string filename, int expected)
		{
			var logEntries = new List<LogEntry>();

			await foreach (var line in filename.GetLinesAsync())
			{
				logEntries.Add(LogEntry.Parse(line));
			}

			var shifts = Shift.Parse(logEntries).ToList();

			var asleep = from s in shifts
						 group s by s.GuardId into gg
						 let guardId = gg.Key
						 let totalMinutesAsleep = gg.Sum(s => s.TotalMinutesAsleep)
						 orderby totalMinutesAsleep descending
						 select (guardId, totalMinutesAsleep);

			var sleepiest = asleep.First();

			var query = from shift in shifts
						where shift.GuardId == sleepiest.guardId
						from minute in shift.AsleepMinutes
						group minute by minute into gg
						let minute = gg.Key
						let count = gg.Count()
						orderby count descending
						select (minute, count);

			var (sleepiestMinute, sleepiestMinuteCount) = query.First();

			Assert.Equal(expected, sleepiest.guardId * sleepiestMinute);
		}

		[Theory]
		[InlineData(
			new[] { 5, 10, 20, 30 },
			new[] { 5, 6, 7, 8, 9, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, })]
		public void Day04_GetInbetweenValues(IEnumerable<int> before, IEnumerable<int> expected)
		{
			Assert.Equal(
				expected,
				before.GetInbetweenValues());
		}

		[Fact]
		public void Day04_Shift_1()
		{
			var lines = new[]
			{
				"[1518-11-01 23:58] Guard #99 begins shift",
				"[1518-11-02 00:40] falls asleep",
				"[1518-11-02 00:50] wakes up",
			};

			var logEntries = lines.Select(LogEntry.Parse).ToList();

			Assert.Equal(3, logEntries.Count);

			var shifts = Shift.Parse(logEntries).ToList();

			Assert.Single(shifts);

			var shift = shifts.Single();

			Assert.Equal(
				DateTime.Parse("1518-11-02", styles: DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal),
				shift.Date);

			Assert.Equal(99, shift.GuardId);
			Assert.Equal(new[] { 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, }, shift.AsleepMinutes);
			Assert.Equal(10, shift.TotalMinutesAsleep);
		}

		[Fact]
		public void Day04_Shift_2()
		{
			var lines = new[]
			{
				"[1518-11-01 00:00] Guard #10 begins shift",
				"[1518-11-01 00:05] falls asleep",
				"[1518-11-01 00:25] wakes up",
				"[1518-11-01 00:30] falls asleep",
				"[1518-11-01 00:55] wakes up",
			};

			var logEntries = lines.Select(LogEntry.Parse).ToList();

			Assert.Equal(5, logEntries.Count);

			var shifts = Shift.Parse(logEntries).ToList();

			Assert.Single(shifts);

			var shift = shifts.Single();

			Assert.Equal(
				DateTime.Parse("1518-11-01", styles: DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal),
				shift.Date);

			Assert.Equal(10, shift.GuardId);
			Assert.Equal(new[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, }, shift.AsleepMinutes);
			Assert.Equal(45, shift.TotalMinutesAsleep);
		}

		public readonly struct Shift
		{
			private Shift(in DateTime date, in short guardId, in IEnumerable<int> asleepMinutes)
			{
				Date = date;
				GuardId = guardId;
				AsleepMinutes = new List<int>(asleepMinutes);
			}

			public DateTime Date { get; }
			public short GuardId { get; }
			public IReadOnlyCollection<int> AsleepMinutes { get; }
			public int TotalMinutesAsleep => AsleepMinutes.Count;

			public static IEnumerable<Shift> Parse(IEnumerable<LogEntry> logEntries)
			{
				foreach (var (date, subset) in from l in logEntries
											   group l by l.Date into gg
											   select (gg.Key, gg))
				{
					var guardId = subset
						.Single(l => l.Type == LogEntry.Types.BeginsShift)
						.GuardId ?? throw new ArgumentException();

					var minutes = (from l in subset
								   where (l.Type & (LogEntry.Types.FallsAsleep | LogEntry.Types.WakesUp)) != 0
								   let minute = l.Minute ?? throw new ArgumentException()
								   orderby minute
								   select minute
								  ).GetInbetweenValues();

					yield return new Shift(date, guardId, minutes);
				}
			}
		}

		public readonly struct LogEntry
		{
			[Flags]
			public enum Types : byte
			{
				None = 0,
				BeginsShift = 1,
				FallsAsleep = 2,
				WakesUp = 4,
			}

			private const DateTimeStyles _dateTimeStyles = DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal;
			private static readonly Regex _regex = new Regex(
					@"^\[(?<dateTime>\d+-\d+-\d+ \d+:\d+)\] (?<message>falls asleep|Guard #(?<guardId>\d+) begins shift|wakes up)$",
					RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Multiline);

			private LogEntry(in DateTime dateTime, in Types type, in short? guardId)
			{
				DateTime = dateTime;
				Type = type;
				GuardId = guardId;
			}

			public DateTime Date => DateTime.Hour > 12 ? DateTime.Date.AddDays(1) : DateTime.Date;
			public short? GuardId { get; }
			public DateTime DateTime { get; }
			public int? Minute => (Type & (Types.FallsAsleep | Types.WakesUp)) != 0 ? DateTime.Minute : default(int?);
			public Types Type { get; }

			public static LogEntry Parse(string line)
			{
				var match = _regex.Match(line);

				var dateTime = DateTime.Parse(match.Groups["dateTime"].Value, styles: _dateTimeStyles);

				var type = match.Groups["message"].Value switch
				{
					"falls asleep" => Types.FallsAsleep,
					"wakes up" => Types.WakesUp,
					_ => Types.BeginsShift,
				};

				var group = match.Groups["guardId"];

				var guardId = group.Success
					? short.Parse(group.Value)
					: default(short?);

				return new LogEntry(dateTime, type, guardId);
			}
		}
	}
}
