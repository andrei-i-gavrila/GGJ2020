using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Puzzles.SimonSays
{
    public class SimonSaysKeyDisplay : BaseBehaviour
    {
        private Image Arrow { get; set; }

        private void Awake()
        {
            Arrow = transform.Find("Arrow").GetComponent<Image>();
        }

        public void SetHint()
        {
            Arrow.color = new Color(0.75f, 0.72f, 0.05f);
        }

        public void SetPressed()
        {
            Arrow.color = new Color(0.35f, 0.75f, 0.31f);
        }

        public void SetNormal()
        {
            Arrow.color = Color.white;
        }
    }
}