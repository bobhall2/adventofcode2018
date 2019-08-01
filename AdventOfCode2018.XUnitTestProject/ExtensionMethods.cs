using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2018.XUnitTestProject
{
	public static class ExtensionMethods
	{
		public async static IAsyncEnumerable<string> GetLinesAsync(this string path)
		{
			using var reader = new StreamReader(path);

			string? line;

			do
			{
				line = await reader.ReadLineAsync();

				if (!string.IsNullOrWhiteSpace(line))
				{
					yield return line;
				}
			}
			while (line != default);
		}
	}
}
