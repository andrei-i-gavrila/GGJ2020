using System;
using System.Collections;
using System.Collections.Generic;
using GGJ.Puzzles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GGJ.Puzzles.Data
{
    public class KeySequencePuzzleData : IPuzzleData
    {
        public readonly List<KeyCode> KeySequence;
        public float CompletionTime;
        
        public int CorrectKeyCount = 0;
        public float TimePassed = 0f;

        public KeySequencePuzzleData(float difficulty)
        {
            var keyPossibilities = new KeyCode[] {KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.LeftArrow};
            KeySequence = new List<KeyCode>();

            var keyAmount = getKeyAmount(difficulty);
            CompletionTime = keyAmount * getTimePerKey(difficulty);

            for (var i = 0; i < keyAmount; i++)
            {
                KeySequence.Add(keyPossibilities[Random.Range(0, keyPossibilities.Length)]);
            }
        }

        private int getKeyAmount(float difficulty)
        {
            return (int) Mathf.Lerp(5f, 10f, difficulty / 10f);
        }

        private float getTimePerKey(float difficulty)
        {
            return Mathf.Lerp(1f, 0.3f, difficulty / 10f);
        }
    }
}