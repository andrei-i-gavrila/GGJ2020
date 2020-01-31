using System;

namespace GGJ.Puzzles
{
	public interface IPuzzle
	{
		void StartPuzzle();
		float Dificulty { get; set; }
		Action<bool> OnPuzzleEnd { get; set; }
	}
}
