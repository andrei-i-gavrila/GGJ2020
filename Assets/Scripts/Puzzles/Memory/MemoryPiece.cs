using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GGJ.Puzzles.Memory
{
    public class MemoryPiece : BaseBehaviour, IPointerClickHandler
    {
        public Image FaceImage;
        public Pair<string, Color> config;
        public RectTransform faceImageRectTransform;
        public Action OnClicked { get; set; }

        private void Awake()
        {
            Utils.GetComponentInChild(transform, "Face", out FaceImage);
            faceImageRectTransform = FaceImage.GetComponent<RectTransform>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }
    }
}