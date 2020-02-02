using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

namespace GGJ.Puzzles.ReactionSpeed
{
    public class ReactionSpeedPuzzleController : BasePuzzleController
    {
        public override string PuzzleId => Constants.REACTION_SPEED_ID;


        private List<Pair<int, float>> challenges;
        private float maxError;
        private float speed;
        private float currentError;

        private int stage;
        private bool moving;
        private float moveProgress;


        private RectTransform containerBar;
        private RectTransform sliderRect;
        private RectTransform targetRect;
        private Image targetRectImage;

        protected override Component puzzleContainer => containerBar;


        protected void Awake()
        {
            Utils.GetComponentInChild(transform, "StartText", out startText);
            Utils.GetComponentInChild(transform, "Bar", out containerBar);
            Utils.GetComponentInChild(transform, "Slider", out sliderRect);
            Utils.GetComponentInChild(transform, "Target", out targetRect);
            targetRectImage = targetRect.GetComponent<Image>();
        }

        private void Update()
        {
            if (!opened) return;

            if (Input.GetKeyUp(KeyCode.Space) && !started) StartPuzzle();

            if (!started) return;

            if (moving)
            {
                moveProgress += challenges[stage].Item1 * speed * Time.deltaTime;

                if (moveProgress > 1f || moveProgress < 0)
                {
                    fail();
                    return;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (Mathf.Abs(moveProgress - challenges[stage].Item2) < currentError)
                    {
                        setStatus(Color.green);

                        stage++;

                        if (stage == challenges.Count)
                        {
                            completed();
                            return;
                        }

                        moveProgress = challenges[stage].Item1 < 0 ? 1f : 0f;
                        moving = false;
                        Invoke(startBarMovement, 1);
                    }
                    else
                    {
                        setStatus(Color.red);
                        fail();
                        return;
                    }
                }
            }

            targetRect.anchoredPosition = new Vector2(Mathf.Lerp(sliderRect.sizeDelta.x / 2 - containerBar.rect.width / 2, containerBar.rect.width / 2 - sliderRect.sizeDelta.x / 2f, challenges[stage].Item2), 0);
            targetRect.sizeDelta = new Vector2(Mathf.Lerp(0, containerBar.rect.width, currentError), containerBar.rect.height);

            sliderRect.anchoredPosition = new Vector2(Mathf.Lerp(sliderRect.sizeDelta.x / 2 - containerBar.rect.width / 2, containerBar.rect.width / 2 - sliderRect.sizeDelta.x / 2f, moveProgress), 0);
        }

        private void setStatus(Color color)
        {
            targetRectImage.color = color;
            Invoke(() => targetRectImage.color = Color.white, .5f);
        }

        protected override void StartPuzzle()
        {
            base.StartPuzzle();
            containerBar.gameObject.SetActive(true);
            Invoke(startBarMovement, 1);
        }

        private void startBarMovement()
        {
            targetRectImage.color = Color.white;

            moving = true;
        }


        protected override void generatePuzzleData()
        {
            var challengeCount = Game.Instance.DificultyManager.GetNumberOfReactionTimeChallenges();
            challenges = new List<Pair<int, float>>(challengeCount);
            speed = Game.Instance.DificultyManager.GetReactTimeSpeed();
            for (var i = 0; i < challengeCount; i++)
            {
                var dir = Random.value < 0.5 ? -1 : 1;
                var target = Random.Range(speed / 3f, 1 - speed / 3f);

                challenges.Add(new Pair<int, float>(dir, target));
            }

            maxError = Game.Instance.DificultyManager.GetMaxReactionSpeedTimeError();
        }


        protected override void resetPuzzle()
        {
            base.resetPuzzle();
            containerBar.gameObject.SetActive(false);

            moving = false;
            stage = 0;
            currentError = maxError;
            moveProgress = challenges[0].Item1 < 0 ? 1f : 0f;
        }
    }
}