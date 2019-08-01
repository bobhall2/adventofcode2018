using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2018.XUnitTestProject
{
	public class Day03
	{

		[Theory]
		[InlineData("#123 @ 3,2: 5x4", 123, 3, 2, 5, 4)]
		public void Day03_ParseClaim(string claimString, int id, int left, int top, int width, int height)
		{
			var claim = Claim.Parse(claimString);

			Assert.Equal(id, claim.Id);
			Assert.Equal(left, claim.Left);
			Assert.Equal(top, claim.Top);
			Assert.Equal(width, claim.Width);
			Assert.Equal(height, claim.Height);
		}

		[Fact]
		public void Day03_ParseClaims()
		{
			var claims = new[]
				{
					"#1 @ 1,3: 4x4",
					"#2 @ 3,1: 4x4",
					"#3 @ 5,5: 2x2",
				}
				.Select(s => Claim.Parse(s));

			var squares = new Dictionary<(int, int), int>();

			foreach (var key in from c in claims
								from s in c.Squares
								select s)
			{
				if (squares.ContainsKey(key)) squares[key]++;
				else squares.Add(key, 1);
			}

			var keys = squares
				.Where(kvp => kvp.Value > 1)
				.Select(kvp => kvp.Key)
				.ToList();

			Assert.Equal(4, keys.Count);
			Assert.Contains((3, 3), keys);
			Assert.Contains((3, 4), keys);
			Assert.Contains((4, 3), keys);
			Assert.Contains((4, 4), keys);
		}

		[Theory]
		[InlineData("claims.dat", 118_539)]
		public async Task Day03_ParseFile(string filename, int expected)
		{
			var path = Path.Combine("data", filename);

			var squares = new Dictionary<(int, int), int>();

			await foreach (var line in path.GetLinesAsync())
			{
				var claim = Claim.Parse(line);

				foreach (var (x, y) in claim.Squares)
				{
					var key = (x, y);

					if (squares.ContainsKey(key)) squares[key]++;
					else squares.Add(key, 1);
				}
			}

			Assert.Equal(
				expected,
				squares.Values.Count(i => i > 1));
		}

		private readonly struct Claim
		{
			private static readonly Regex _regex = new Regex(
				@"^#(?<id>\d+) @ (?<left>\d+),(?<top>\d+): (?<width>\d+)x(?<height>\d+)$",
				RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant);

			public Claim(int id, int left, int top, int width, int height)
			{
				Id = id;
				Left = left;
				Top = top;
				Width = width;
				Height = height;

				Squares = from x in Enumerable.Range(left, width)
						  from y in Enumerable.Range(top, height)
						  select (x, y);
			}

			public int Id { get; }
			public int Left { get; }
			public int Top { get; }
			public int Width { get; }
			public int Height { get; }
			public IEnumerable<(int x, int y)> Squares { get; }

			public static Claim Parse(in string s)
			{
				if (string.IsNullOrWhiteSpace(s)) throw new ArgumentNullException(s);

				var match = _regex.Match(s);

				if (!match.Success) throw new ArgumentOutOfRangeException(nameof(s), s, $"{s} doesn't match {_regex}") { Data = { [nameof(s)] = s, }, };

				int f(string key) => int.Parse(match.Groups[key].Value);

				return new Claim(f("id"), f("left"), f("top"), f("width"), f("height"));
			}
		}
	}
}
