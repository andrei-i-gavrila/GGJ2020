using System;

namespace GGJ.Puzzles
{
    public interface IPuzzle<TPuzzleData> where TPuzzleData : IPuzzleData
    {
        void StartPuzzle(TPuzzleData data);

        Action<bool> OnPuzzleEnd { get; set; }
    }

    public interface IPuzzleData
    {
    }
}