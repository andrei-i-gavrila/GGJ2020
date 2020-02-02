using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

namespace GGJ.Puzzles.Memory
{
    public class MemoryPuzzleController : BasePuzzleController
    {
        protected override Component puzzleContainer => tilesContainer;
        public override string PuzzleId => Constants.MEMORY_ID;
        
        private int pairCount;
        private RectTransform tilesContainer;

        private MemoryPiece currentPiece;
        private int correctPairs = 0;
        private bool waiting;
        private GridLayoutGroup _gridLayoutGroup;


        protected void Awake()
        {
            Utils.GetComponentInChild(transform, "StartText", out startText);
            Utils.GetComponentInChild(transform, "TilesContainer", out tilesContainer);
            _gridLayoutGroup = tilesContainer.GetComponent<GridLayoutGroup>();
        }

        private void Update()
        {
            if (!opened) return;

            if (Input.GetKeyUp(KeyCode.Space) && !started) StartPuzzle();
        }


        protected override void StartPuzzle()
        {
            base.StartPuzzle();
            
            tilesContainer.gameObject.SetActive(true);
        }


        protected override void generatePuzzleData()
        {
            pairCount = (int) Mathf.Lerp(3, 18, difficulty / 10f);


            var availSize = tilesContainer.sizeDelta.x * tilesContainer.sizeDelta.y;
            var tileSize = Mathf.Sqrt(availSize / (pairCount * 2)) * 0.7f;

            _gridLayoutGroup.cellSize = Vector2.one * tileSize;

            var colors = new[] {Color.red, Color.cyan, Color.yellow, Color.blue, Color.green, Color.white};
            var shapes = new[] {"Circle", "Diamond", "Hexagon", "Square", "Triangle"};

            var alreadyUsed = new HashSet<Pair<string, Color>>();

            Pair<string, Color> randomTileConfig()
            {
                while (true)
                {
                    var shape = shapes[Random.Range(0, shapes.Length)];
                    var color = colors[Random.Range(0, colors.Length)];

                    var pair = new Pair<string, Color>(shape, color);

                    if (!alreadyUsed.Contains(pair))
                    {
                        alreadyUsed.Add(pair);
                        return pair;
                    }
                }
            }

            var tilePrefab = Resources.Load<MemoryPiece>("Prefabs/MemoryTiles/Tile");
            var shapesPrefabs = new Dictionary<string, Sprite>();
            foreach (var shape in shapes)
            {
                shapesPrefabs[shape] = Resources.Load<Sprite>($"Prefabs/MemoryTiles/{shape}");
            }

            for (var i = 0; i < pairCount; i++)
            {
                var config = randomTileConfig();

                for (var j = 0; j < 2; j++)
                {
                    var tile = Instantiate(tilePrefab, tilesContainer);

                    tile.FaceImage.sprite = shapesPrefabs[config.Item1];
                    tile.FaceImage.color = config.Item2;
                    tile.faceImageRectTransform.sizeDelta = new Vector2(tileSize * 0.8f, tileSize * 0.8f);
                    tile.config = config;
                    tile.OnClicked += () => TileClicked(tile);
                }
            }

            for (var i = 0; i < pairCount * 2 * 10; i++)
            {
                tilesContainer.GetChild(Random.Range(0, tilesContainer.childCount)).SetAsLastSibling();
            }
        }

        private void TileClicked(MemoryPiece tile)
        {
            if (waiting) return;
            tile.FaceImage.gameObject.SetActive(true);
            waiting = true;

            if (currentPiece == null)
            {
                currentPiece = tile;
                waiting = false;
            }
            else
            {
                if (Equals(tile.config, currentPiece.config))
                {
                    tile.OnClicked = null;
                    currentPiece.OnClicked = null;
                    correctPairs++;
                    if (correctPairs == pairCount)
                    {
                        completed();
                    }

                    waiting = false;
                    currentPiece = null;
                }
                else
                {
                    Invoke(() =>
                    {
                        tile.FaceImage.gameObject.SetActive(false);
                        currentPiece.FaceImage.gameObject.SetActive(false);
                        waiting = false;
                        currentPiece = null;
                    }, .5f);
                }
            }
        }


        protected override void resetPuzzle()
        {
            base.resetPuzzle();
            tilesContainer.gameObject.SetActive(false);
        }
    }
}