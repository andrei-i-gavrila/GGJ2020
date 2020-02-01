using System.Collections.Generic;

namespace GGJ.Rooms
{
	public class Room : BaseBehaviour
	{
		public List<Entrance> Entrances { get; private set; } = new List<Entrance>();
	}
}
