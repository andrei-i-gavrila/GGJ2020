using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GGJ.Puzzles.Jigsaw
{
    public class JigsawPuzzleController : BasePuzzleController
    {
        protected override Component puzzleContainer => jigsawRoot;
        public override string PuzzleId => Constants.JIGSAW_ID;

        private const int puzzleWidth = 800;
        private const int puzzleHeight = 400;

        private List<JigsawPiece> pieces;
        private RectTransform jigsawRoot;

        
        
        protected void Awake()
        {
            Utils.GetComponentInChild(transform, "StartText", out startText);
            Utils.GetComponentInChild(transform, "JigsawRoot", out jigsawRoot);
        }

        private void Update()
        {
            if (!opened) return;

            if (Input.GetKeyUp(KeyCode.Space) && !started) StartPuzzle();

            if (!started) return;

            if (pieces.All(p => p.correct))
            {
                completed();
            }
        }

        protected override void StartPuzzle()
        {
            base.StartPuzzle();
            jigsawRoot.gameObject.SetActive(true);
            var xScale = jigsawRoot.rect.width / puzzleWidth;
            var yScale = jigsawRoot.rect.height / puzzleHeight;
            foreach (var jigsawPiece in pieces)
            {
                jigsawPiece.scaleBy(xScale, yScale);
            }
        }


        protected override void generatePuzzleData()
        {
            pieces?.ForEach(o => Destroy(o.gameObject));
            pieces = new List<JigsawPiece>();
            var pieceCount = Game.Instance.DificultyManager.GetNumberOfJigsawPieces();
            
            var thresholdDistance = puzzleHeight * puzzleWidth / (pieceCount * Mathf.PI);

            var centers = new List<Vector2Int>();

            while (centers.Count < pieceCount)
            {
                var nextCenter = new Vector2Int(Random.Range(0, puzzleWidth), Random.Range(0, puzzleHeight));

                var good = centers.All(center => (center - nextCenter).sqrMagnitude > thresholdDistance);
                if (good) centers.Add(nextCenter);
            }

            var pixelsPerCenter = centers.Select(center => new List<Vector2Int>()).ToList();

            int minIndex;
            int minDistance;

            for (var x = 0; x < puzzleWidth; x++)
            {
                for (var y = 0; y < puzzleHeight; y++)
                {
                    var pos = new Vector2Int(x, y);
                    minIndex = -1;
                    minDistance = int.MaxValue;

                    for (var i = 0; i < centers.Count; i++)
                    {
                        var center = centers[i];
                        var sqrDistance = Mathf.Abs(center.x - pos.x) + Mathf.Abs(center.y - pos.y);
                        if (sqrDistance >= minDistance) continue;
                        minDistance = sqrDistance;
                        minIndex = i;
                    }

                    pixelsPerCenter[minIndex].Add(pos - centers[minIndex]);
                }
            }

            var centerOffset = new Vector2Int(puzzleWidth / 2, puzzleHeight / 2);
            for (var i = 0; i < centers.Count; i++)
            {
                var center = centers[i];
                var go = new GameObject($"JigsawPiece{i}");

                var piece = go.AddComponent<JigsawPiece>();
                piece.transform.parent = jigsawRoot;
                piece.Init(center - centerOffset, new Vector2Int(Random.Range(-puzzleWidth / 3, puzzleWidth / 3), Random.Range(-puzzleHeight / 3, puzzleHeight / 3)), pixelsPerCenter[i]);
                pieces.Add(piece);
            }
        }


        protected override void resetPuzzle()
        {
            base.resetPuzzle();
            
            jigsawRoot.gameObject.SetActive(false);
        }
    }

    internal class JigsawPiece : BaseBehaviour, IEndDragHandler, IDragHandler
    {
        public Vector2 CorrectCenter;
        private RectTransform _rectTransform;
        public bool correct;
        private Texture2D _texture;
        private float allowedError = 40;

        public void Init(Vector2Int correctCenter, Vector2Int center, List<Vector2Int> pixelsOffsets)
        {
            CorrectCenter = correctCenter;

            createSprite(pixelsOffsets);
            _rectTransform.anchoredPosition = center;
        }

        public void scaleBy(float xScale, float yScale)
        {
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x * xScale, _rectTransform.sizeDelta.y * yScale);
            CorrectCenter = new Vector2(CorrectCenter.x * xScale, CorrectCenter.y * yScale);
            allowedError *= (xScale + yScale) / 2;
        }

        private void createSprite(List<Vector2Int> pixelOffsets)
        {
            var minX = pixelOffsets.Select(p => p.x).Min();
            var maxX = pixelOffsets.Select(p => p.x).Max();

            var minY = pixelOffsets.Select(p => p.y).Min();
            var maxY = pixelOffsets.Select(p => p.y).Max();


            var image = gameObject.AddComponent<Image>();

            var width = maxX - minX + 1;
            var height = maxY - minY + 1;
            _texture = new Texture2D(width, height);
            var colors = new Color[width * height];
            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.clear;
            }

            var minCorner = new Vector2Int(minX, minY);
            foreach (var offset in pixelOffsets)
            {
                var pixel = offset - minCorner;
                colors[pixel.y * width + pixel.x] = Color.white;
            }

            _texture.SetPixels(colors);
            _texture.Apply();

            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.sizeDelta = new Vector2(width, height);
            _rectTransform.pivot = new Vector2(1f * -minX / width, 1f * -minY / height);
            image.sprite = Sprite.Create(_texture, new Rect(0, 0, width, height), new Vector2(-minX, -minY));
            image.alphaHitTestMinimumThreshold = 1f;
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            if (!((CorrectCenter - _rectTransform.anchoredPosition).sqrMagnitude <= allowedError*allowedError)) return;
            correct = true;
            _rectTransform.SetAsFirstSibling();
            _rectTransform.anchoredPosition = CorrectCenter;
            var pixels = _texture.GetPixels();

            for (var i = 0; i < pixels.Length; i++)
            {
                if (pixels[i] == Color.white) pixels[i] = new Color(0.28f, 1f, 0.46f);
            }

            _texture.SetPixels(pixels);
            _texture.Apply();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (correct) return;
            _rectTransform.anchoredPosition += eventData.delta;
            _rectTransform.SetAsLastSibling();
        }
    }
}