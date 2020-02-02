using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GGJ.Puzzles.Memory
{
    public class MemoryPiece : BaseBehaviour, IPointerClickHandler
    {
        public Image FaceImage
        {
            get
            {
                if (FaceImageCached == null)
                {
                    Utils.GetComponentInChild(transform, "Face", out FaceImageCached);
                    faceImageRectTransform = FaceImage.GetComponent<RectTransform>();
                }

                return FaceImageCached;
            }
        }

        public Image FaceImageCached;
        public Pair<string, Color> config;
        public RectTransform faceImageRectTransform;
        public Action OnClicked { get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }
    }
}