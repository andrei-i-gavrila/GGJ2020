using System.Collections.Generic;

namespace GGJ
{
	public class ConsolesManager
	{
		private List<Console> consoles = new List<Console>();

		public void AddConsole(Console console)
		{
			consoles.Add(console);
		}


	}
}