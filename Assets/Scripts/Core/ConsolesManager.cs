using System.Collections.Generic;
using System.Linq;

namespace GGJ
{
	public class ConsolesManager
	{
		private List<Console> consoles = new List<Console>();
		private List<int> consoleIds = new List<int>();

		public ConsolesManager()
		{
			consoleIds = GetListWithNumbers(0, 300);
		}

		public void AddConsole(Console console)
		{
			consoles.Add(console);
			var id = consoleIds.GetRandomValue();
			console.SetConsoleId(id.ToString());
			consoleIds.Remove(id);
		}

		public int GetNumberOfConsolesWithState(ConsoleState consoleState)
		{
			return consoles.Count(console => console.ConsoleState == consoleState);
		}

		public Console GetRandomConsoleWithState(ConsoleState consoleState)
		{
			return consoles.Where(console => console.ConsoleState == consoleState).GetRandomValue();
		}

		private List<int> GetListWithNumbers(int from, int to)
		{
			if (from > to)
			{
				var temp = to;
				to = from;
				from = temp;
			}

			var result = new List<int>();
			for (int i = from; i <= to; i++)
			{
				result.Add(i);
			}
			return result;
		}
	}
}