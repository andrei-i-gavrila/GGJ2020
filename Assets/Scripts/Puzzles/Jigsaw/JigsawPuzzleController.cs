using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

namespace GGJ.Puzzles.Jigsaw
{
	public class JigsawPuzzleController : BasePuzzleController
	{
		public float difficulty = 1f;

		private TextMeshProUGUI startText;

		private bool opened;
		private bool started;

		private int puzzleWidth = 800;
		private int puzzleHeight = 400;
		private List<JigsawPiece> pieces = new List<JigsawPiece>();
		private RectTransform jigsawRoot;
		public override string PuzzleId => Constants.JIGSAW_ID;

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

		private void fail()
		{
			resetPuzzle();
		}

		private void completed()
		{
			resetPuzzle();
		}

		public override void Open()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			generatePuzzleData();
			opened = true;
		}

		private void StartPuzzle()
		{
			startText.gameObject.SetActive(false);
			jigsawRoot.gameObject.SetActive(true);
			started = true;
		}


		private void generatePuzzleData()
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			var pieceCount = (int)Mathf.Lerp(5, 15, difficulty / 10f);
			var thresholdDistance = puzzleHeight * puzzleWidth / (pieceCount * Mathf.PI);

			var centers = new List<Vector2Int>();

			while (centers.Count < pieceCount)
			{
				var nextCenter = new Vector2Int(Random.Range(0, puzzleWidth), Random.Range(0, puzzleHeight));

				var good = centers.All(center => (center - nextCenter).sqrMagnitude > thresholdDistance);
				if (good) centers.Add(nextCenter);
			}

			var pixelsPerCenter = centers.Select(center => new HashSet<Vector2Int>()).ToList();
			for (var x = 0; x < puzzleWidth; x++)
			{
				for (var y = 0; y < puzzleHeight; y++)
				{
					var pos = new Vector2Int(x, y);

					var minIndex = -1;
					var minDistance = int.MaxValue;

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

			stopWatch.Stop();
			UnityEngine.Debug.LogError($"Puzzle-generation {stopWatch.Elapsed}");
			stopWatch.Reset();
			stopWatch.Start();

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

			stopWatch.Stop();
			UnityEngine.Debug.LogError($"Creatin  {stopWatch.Elapsed}");
			stopWatch.Reset();
			stopWatch.Start();

			resetPuzzle();
		}


		private void resetPuzzle()
		{
			started = false;
			startText.gameObject.SetActive(true);
			jigsawRoot.gameObject.SetActive(false);

		}
	}

	class JigsawPiece : BaseBehaviour, IEndDragHandler, IDragHandler
	{
		public Vector2Int CorrectCenter;
		private RectTransform _rectTransform;
		public bool correct;
		private Texture2D _texture;
		private List<Vector2Int> cardinals = new List<Vector2Int>() { new Vector2Int(0, -2), new Vector2Int(0, 2), new Vector2Int(-2, 0), new Vector2Int(2, 0) };
		public void Init(Vector2Int correctCenter, Vector2Int center, HashSet<Vector2Int> pixelsOffsets)
		{
			CorrectCenter = correctCenter;

			createSprite(pixelsOffsets);
			_rectTransform.anchoredPosition = center;
		}

		private void createSprite(HashSet<Vector2Int> pixelOffsets)
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
			if (!((CorrectCenter - _rectTransform.anchoredPosition).sqrMagnitude <= 100)) return;
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