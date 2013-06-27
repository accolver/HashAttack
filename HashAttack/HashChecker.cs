using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace HashAttack
{
	public class HashChecker
	{
		public SortedList<string, string> Hashes { get { return _hashes; } }
		readonly SortedList<string, string> _hashes;

		readonly SHA1 _sha1;
		readonly int _maxHashLength;

		public HashChecker(int maxHashLength, int initialSize = -1)
		{
			_maxHashLength = maxHashLength;
			_hashes = initialSize < 1 ? new SortedList<string, string>() : new SortedList<string, string>(initialSize);
			_sha1 = new SHA1CryptoServiceProvider();
		}

		public Tuple<string, string, string, int> CheckAndAdd(string text, Encoding enc)
		{
			var buffer = enc.GetBytes(text);
			var hash = BitConverter.ToString(_sha1.ComputeHash(buffer)).Replace("-", "").Substring(0, _maxHashLength);

			if (_hashes.ContainsKey(hash))
				if (_hashes[hash] != text)
					return new Tuple<string, string, string, int>(_hashes[hash], text, hash, _hashes.Count);
				else
					return null;

			_hashes.Add(hash, text);
			return null;
		}

		public string ComputeHash(string m2, Encoding enc)
		{
			return BitConverter.ToString(_sha1.ComputeHash(enc.GetBytes(m2))).Replace("-", "").Substring(0, _maxHashLength);
		}

	}
}
