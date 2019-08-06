using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2018.XUnitTestProject
{
	public static class ExtensionMethods
	{
		public async static IAsyncEnumerable<string> GetLinesAsync(this string filename)
		{
			var path = Path.Combine("Data", filename);

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

		public static IEnumerable<int> GetInbetweenValues(this IEnumerable<int> stateChanges)
		{
			using var enumerator = stateChanges.GetEnumerator();

			while (enumerator.MoveNext())
			{
				var start = enumerator.Current;
				enumerator.MoveNext();
				var end = enumerator.Current;

				for (var a = start; a < end; a++)
				{
					yield return a;
				}
			}
		}
	}
}
