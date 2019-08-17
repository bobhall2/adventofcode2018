using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;

namespace AdventOfCode2018.XUnitTestProject
{
	public class ExtensionMethodsTests
	{
		[Theory]
		[InlineData(new[] { 1, 2, }, new[] { 1, })]
		[InlineData(new[] { 1, 3, }, new[] { 1, 2, })]
		[InlineData(new[] { 2, 5, }, new[] { 2, 3, 4, })]
		[InlineData(new[] { 2, 5, 10, 11, }, new[] { 2, 3, 4, 10, })]
		public void ExtensionMethodsTests_GetInbetweenValues(ICollection<int> inputChanges, ICollection<int> expected)
		{
			// Act
			var actual = inputChanges.GetInbetweenValues();

			// Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(1, 2)]
		[InlineData(2, 3, 3, 4)]
		public void ExtensionMethodsTests_GetPairs_GetPoints(params int[] values)
		{
			var points = values.GetPairs<int, Point>((x, y) => new Point(x, y)).ToList();

			Assert.NotEmpty(points);
			Assert.Equal(values.Length / 2, points.Count);

			for (int a = 0, b = 0; a < values.Length && b < points.Count; a++, b += 2)
			{
				Assert.Equal(values[a], points[b].X);
				Assert.Equal(values[a + 1], points[b].Y);
			}
		}
	}
}
