using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2018.XUnitTestProject
{
	public class Day06
	{
		[Theory]
		[InlineData(1, 1, 1, 6, 8, 3, 3, 4, 5, 5, 8, 9)]
		public void Day06_ParsePoints(params int[] coordinates)
		{
			// Act
			var points = GetPoints(coordinates).ToList();

			// Assert
			Assert.Equal(coordinates.Length / 2, points.Count);
			Assert.All(points, p => Assert.InRange(p.X, 1, int.MaxValue));
			Assert.All(points, p => Assert.InRange(p.Y, 1, int.MaxValue));
			Assert.Equal(coordinates[0], points[0].X);
			Assert.Equal(coordinates[1], points[0].Y);
			Assert.Equal(coordinates[2], points[1].X);
			Assert.Equal(coordinates[3], points[1].Y);
			Assert.Equal(coordinates[4], points[2].X);
			Assert.Equal(coordinates[5], points[2].Y);
			Assert.Equal(coordinates[6], points[3].X);
			Assert.Equal(coordinates[7], points[3].Y);
			Assert.Equal(coordinates[8], points[4].X);
			Assert.Equal(coordinates[9], points[4].Y);
			Assert.Equal(coordinates[10], points[5].X);
			Assert.Equal(coordinates[11], points[5].Y);
		}

		[Fact]
		public void Day06_PopulateGrid()
		{
			var coordinates = new[] { 1, 1, 1, 6, 8, 3, 3, 4, 5, 5, 8, 9, };

			var points = GetPoints(coordinates).ToList();

			var grid = GetGrid(points);

			Assert.Equal(9, grid.GetLength(0));
			Assert.Equal(10, grid.GetLength(1));
			Assert.Equal('A', grid[1, 1]);
			Assert.Equal('B', grid[1, 6]);
			Assert.Equal('C', grid[8, 3]);
			Assert.Equal('D', grid[3, 4]);
			Assert.Equal('E', grid[5, 5]);
			Assert.Equal('F', grid[8, 9]);
		}

		private static IEnumerable<Point> GetPoints(IEnumerable<int> coordinates)
		{
			using var enumerator = coordinates.GetEnumerator();

			while (enumerator.MoveNext())
			{
				var x = enumerator.Current;
				enumerator.MoveNext();
				var y = enumerator.Current;
				yield return new Point(x, y);
			}
		}

		private static char[,] GetGrid(ICollection<Point> points)
		{
			var width = points.Max(p => p.X) + 1;
			var height = points.Max(p => p.Y) + 1;

			var array = new char[width, height];

			var @char = 'A';

			foreach (var point in points)
			{
				array[point.X, point.Y] = @char++;
			}

			return array;
		}

		[Theory]
		[InlineData(5, 5, 0, 5, 5)]
		[InlineData(5, 5, 1, 5, 5, 5, 4, 5, 6, 4, 5, 6, 5)]
		public void Day07_GetPointsFromPoint(int x, int y, int manhattanDistance, params int[] expectedCoordsList)
		{
			var point = new Point(x, y);
			var actuals = GetPointsFromPoint(point, manhattanDistance).ToList();

			var expecteds = expectedCoordsList.GetPairs().Select(t => new Point(t.first, t.second)).ToList();

			Assert.Equal(expecteds.Count, actuals.Count);

			Assert.Equal(expecteds, actuals);
		}

		private static readonly Size _up = new Size(0, -1);
		private static readonly Size _down = new Size(0, 1);
		private static readonly Size _left = new Size(-1, 0);
		private static readonly Size _right = new Size(1, 0);

		private static readonly IEnumerable<Size> _directions = new[] { _up, _down, _left, _right, };

		private static IEnumerable<Point> GetPointsFromPoint(Point point, int manhattanDistance)
		{
			yield return point;

			if (manhattanDistance == 0)
			{
				yield break;
			}

			foreach (var direction in _directions)
			{
				foreach (var subPoint in GetPointsFromPoint(point + direction, manhattanDistance - 1))
				{
					yield return subPoint;
				}
			}
		}
	}
}
