using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2018.XUnitTestProject
{
	public class Day05
	{
		[Theory]
		[InlineData("aA", "", 0)]
		[InlineData("abBA", "", 0)]
		[InlineData("abAB", "abAB", 4)]
		[InlineData("aabAAB", "aabAAB", 6)]
		[InlineData("dabAcCaCBAcCcaDA", "dabCBAcaDA", 10)]
		public void Day05_React(string before, string expected, int expectedUnitsCount)
		{
			var actual = React(before);

			Assert.Equal(expected, actual);
			Assert.Equal(expectedUnitsCount, actual.Length);
		}

		[Theory]
		[InlineData("polymer.dat", 10_564)]
		public async Task Day05_ParseFile(string filename, int expected)
		{
			var polymer = await filename.GetContentAsync();

			polymer = polymer.Trim();

			var actual = React(polymer);

			Assert.Equal(expected, actual.Length);
		}

		private static string React(string polymer)
		{
			var chars = polymer.ToList();

			for (var a = 0; a < chars.Count - 1; a++)
			{
				var left = chars[a];
				var right = chars[a + 1];

				if (left != right
					&& char.ToLowerInvariant(left) == char.ToLowerInvariant(right))
				{
					chars.RemoveAt(a);
					chars.RemoveAt(a);

					if (a > 0) a -= 2;
				}
			}

			return new string(chars.ToArray());
		}
	}
}
