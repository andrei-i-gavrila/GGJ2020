using System;
using GGJ.Puzzles;
using GGJ.Puzzles.Data;

namespace Puzzles
{
    public class KeySequencePuzzle: IPuzzle<KeySequencePuzzleData>
    {
        public void StartPuzzle(KeySequencePuzzleData data)
        {
            
        }

        public Action<KeySequencePuzzleData> OnPuzzleStarted { get; set; }

        public Action<bool> OnPuzzleEnd { get; set; }
        public KeySequencePuzzleData CreateData(float difficulty)
        {
            return new KeySequencePuzzleData(difficulty);
        }
    }
}