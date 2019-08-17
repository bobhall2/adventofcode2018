using System;
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
			foreach (var (start, end) in stateChanges.GetPairs())
			{
				for (var a = start; a < end; a++)
				{
					yield return a;
				}
			}
		}

		public static IEnumerable<(T first, T second)> GetPairs<T>(this IEnumerable<T> collection) =>
			collection.GetPairs<T, ValueTuple<T, T>>((first, second) => (first, second));

		public static IEnumerable<TResult> GetPairs<T, TResult>(this IEnumerable<T> collection, Func<T, T, TResult> func)
		{
			using var enumerator = collection.GetEnumerator();

			while (enumerator.MoveNext())
			{
				var first = enumerator.Current;

				var second = enumerator.MoveNext()
					? enumerator.Current
					: default;

				yield return func(first, second);
			}
		}
	}
}
