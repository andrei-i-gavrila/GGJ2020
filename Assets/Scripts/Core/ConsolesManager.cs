using System.Collections.Generic;
using System.Linq;

namespace GGJ
{
	public class ConsolesManager
	{
		private List<Console> consoles = new List<Console>();

		public void AddConsole(Console console)
		{
			consoles.Add(console);
		}

		public int GetNumberOfConsolesWithState(ConsoleState consoleState)
		{
			return consoles.Count(console => console.ConsoleState == consoleState);
		}

		public Console GetRandomConsoleWithState(ConsoleState consoleState)
		{
			return consoles.Where(console => console.ConsoleState == consoleState).GetRandomValue();
		}
	}
}