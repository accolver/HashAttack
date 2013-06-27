using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Dynamic;

namespace HashAttack
{
	class HashGenerator
	{
		readonly HashChecker _hashChecker;
		readonly SHA1 _sha1;
		readonly int _maxHashLength;
		Tuple<string, string, string, int> _collision;
		Random _random = new Random((int)DateTime.Now.Ticks);
		int _counter;
		const int COUNTER_RESET = 40000;


		public HashGenerator(int maxHashLength, int estimatedHashesNeeded = -1)
		{
			_hashChecker = new HashChecker(_maxHashLength = maxHashLength, estimatedHashesNeeded);
			_sha1 = new SHA1CryptoServiceProvider();
			_collision = null;
		}

		public Tuple<string, string, string, int> GenerateUntilCollision(int maxTextLength, Encoding enc)
		{
			_counter = 0;
			do
			{
				_collision = _hashChecker.CheckAndAdd(RandomString(_random.Next(maxTextLength) + 20), enc);
				//_counter++;
			} while (_collision == null);

			return _collision;
		}

		public Tuple<string, string, string, int> SecondPreImage(string m1, int maxTextLength)
		{ 
			var enc = new ASCIIEncoding();
			var hash1 = BitConverter.ToString(_sha1.ComputeHash(enc.GetBytes(m1))).Replace("-", "").Substring(0, _maxHashLength);

			var count = 0;
			var m2 = "";
			var hash2 = "";
			do
			{
				if (count > COUNTER_RESET)
				{
					_random = new Random((int)DateTime.Now.Ticks);
					count = 0;
				}
				m2 = RandomString(maxTextLength);
				hash2 = _hashChecker.ComputeHash(m2, enc);
				count++;
			} while (hash1 != hash2);
			return new Tuple<string, string, string, int>(m1, m2, hash2, count);
		}

		private string RandomString(int size)
		{
			//if(_counter % COUNTER_RESET == 0)
			//	_random = new Random((int)DateTime.Now.Ticks);
			StringBuilder builder = new StringBuilder();
			char ch;
			for (int i = 0; i < size; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65)));
				builder.Append(ch);
			}

			return builder.ToString();
		}
	}
}
