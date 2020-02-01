using System;
using GGJ.Puzzles.Data;

namespace GGJ.Puzzles
{
    public class PuzzleManager
    {
        public void StartPuzzle(PuzzleType type, float difficulty)
        {
            var puzzleData = CreatePuzzleData(type, difficulty);
            OnPuzzleStarted?.Invoke(puzzleData);
        }

        private IPuzzleData CreatePuzzleData(PuzzleType type, float difficulty)
        {
            if (type == PuzzleType.KeySequence)
                return new KeySequencePuzzleData(difficulty);


            throw new Exception("wtf");
        }

        public Action<IPuzzleData> OnPuzzleStarted { get; set; }

        public Action OnPuzzleFailed { get; set; }

        public void PuzzleFailed()
        {
            OnPuzzleFailed?.Invoke();
        }
    }

    public enum PuzzleType
    {
        KeySequence
    }
}