using System.Collections.Generic;
using System.Linq;

namespace Director.Abstractions
{
	public static class DictionaryExtensions
	{
		public static bool ContainsKeys<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, params TKey[] keys) => keys.All(dictionary.ContainsKey);
	}
}
