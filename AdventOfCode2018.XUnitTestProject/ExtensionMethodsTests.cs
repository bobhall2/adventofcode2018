using System.Collections.Generic;
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
	}
}
