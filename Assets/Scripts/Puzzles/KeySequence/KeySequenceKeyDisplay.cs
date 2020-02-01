using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Puzzles.KeySequence
{
    public class KeySequenceKeyDisplay : BaseBehaviour
    {
        private Image Arrow { get; set; }

        private void Awake()
        {
            Arrow = transform.Find("Arrow").GetComponent<Image>();
        }

        public void SetCorrect()
        {
            Arrow.color = new Color(0.35f, 0.75f, 0.31f);
        }

        public void SetNormal()
        {
            Arrow.color = new Color(1f, 1f, 1f);
        }
    }
}