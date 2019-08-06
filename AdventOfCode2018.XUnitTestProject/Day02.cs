using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2018.XUnitTestProject
{
	public class Day02
	{
		[Flags]
		public enum Groupings : byte
		{
			None = 0,
			Two = 1,
			Three = 2,
			Both = Two | Three
		}

		[Theory]
		[InlineData("abcdef", Groupings.None)]
		[InlineData("bababc", Groupings.Both)]
		[InlineData("abbcde", Groupings.Two)]
		[InlineData("abcccd", Groupings.Three)]
		[InlineData("aabcdd", Groupings.Two)]
		[InlineData("abcdee", Groupings.Two)]
		[InlineData("ababab", Groupings.Three)]
		public void Day02_IdParse(string before, Groupings expected)
		{
			Assert.Equal(
				expected,
				GetGroupings(before));
		}

		[Theory]
		[InlineData(12, "abcdef", "bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab")]
		public void Day02_BatchChecksum(int expected, params string[] batch)
		{
			var groupings = batch.Select(GetGroupings).ToList();

			var actual = GetChecksum(groupings);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("ids.dat", 6_200)]
		public async Task Day02_ParseAndChecksum(string filename, int expected)
		{
			var groupings = new List<Groupings>();

			await foreach (var line in filename.GetLinesAsync())
			{
				var grouping = GetGroupings(line);

				groupings.Add(grouping);
			}

			var actual = GetChecksum(groupings);

			Assert.Equal(expected, actual);
		}

		private static Groupings GetGroupings(string id)
		{
			var groupings = Groupings.None;

			foreach (var grouping in from c in id
									 group c by c into gg
									 select gg.Count() switch
									 {
										 2 => Groupings.Two,
										 3 => Groupings.Three,
										 _ => Groupings.None,
									 })
			{
				groupings |= grouping;
			}

			return groupings;
		}

		private static int GetChecksum(ICollection<Groupings> groupings)
		{
			var twos = groupings.Count(g => (g & Groupings.Two) != 0);
			var threes = groupings.Count(g => (g & Groupings.Three) != 0);

			return twos * threes;
		}
	}
}
