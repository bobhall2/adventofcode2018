using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2018.XUnitTestProject
{
	public class Day01
	{
		[Theory]
		[InlineData("+0", 0)]
		[InlineData("+1", 1)]
		[InlineData("+2", 2)]
		[InlineData("-0", 0)]
		[InlineData("-1", -1)]
		[InlineData("-2", -2)]
		public void Day01_ParseString(string before, int expected)
		{
			Assert.Equal(
				expected,
				int.Parse(before));
		}

		[Theory]
		[InlineData(new[] { 1, -2, 3, 1, }, 3)]
		[InlineData(new[] { 1, 1, 1, }, 3)]
		[InlineData(new[] { 1, 1, -2, }, 0)]
		[InlineData(new[] { -1, -2, -3, }, -6)]
		public void Day01_SumValues(IEnumerable<int> values, int expected)
		{
			Assert.Equal(
				expected,
				values.Sum());
		}

		[Theory]
		[InlineData("frequencies.dat", 442)]
		public async Task Day01_ParseAndSum(string filename, int expected)
		{
			var path = Path.Combine("data", filename);

			var actual = 0;

			await foreach (var line in path.GetLinesAsync())
			{
				actual += int.Parse(line);
			}

			Assert.Equal(expected, actual);
		}
	}
}
