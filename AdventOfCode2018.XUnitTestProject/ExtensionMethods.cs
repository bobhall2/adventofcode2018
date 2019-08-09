using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

		public async static Task<string> GetContentAsync(this string filename)
		{
			var path = Path.Combine("Data", filename);

			using var reader = new StreamReader(path);

			return await reader.ReadToEndAsync();
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

		public static IEnumerable<(T first, T second)> GetPairs<T>(this IEnumerable<T> collection)
		{
			using var enumerator = collection.GetEnumerator();

			while (enumerator.MoveNext())
			{
				var first = enumerator.Current;

				var secdond = enumerator.MoveNext()
					? enumerator.Current
					: default;

				yield return (first, secdond);
			}
		}
	}
}
