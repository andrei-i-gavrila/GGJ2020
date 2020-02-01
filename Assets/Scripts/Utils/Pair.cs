﻿using System;

namespace GGJ
{
	public class Pair<T1, T2>
	{
		public Pair()
		{
			Item1 = default(T1);
			Item2 = default(T2);
		}

		public Pair(T1 item1, T2 item2)
		{
			Item1 = item1;
			Item2 = item2;
		}

		public T1 Item1 { get; set; }
		public T2 Item2 { get; set; }
	}
}
