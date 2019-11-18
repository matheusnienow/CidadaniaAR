using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enum;
using Observer;
using Puzzles;
using Puzzles.Base;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

namespace Manager
{
    public class BridgeLevelManager : LevelManager
    {
        [SerializeField] private GameObject imagePanel;
        [SerializeField] private GameObject glassCheckPoint;
        [SerializeField] private GameObject paperCheckPoint;

        [SerializeField] private PuzzleIncompletePath glassPuzzle;
        [SerializeField] private PuzzleIncompletePath paperPuzzle;

        private Image _image;

        protected override void Start()
        {
            base.Start();
            _image = imagePanel.GetComponentInChildren<Image>();
        }

        protected override void SubscribeOnPuzzleEvents()
        {
            glassPuzzle.Subscribe(this);
            paperPuzzle.Subscribe(this);
        }

        protected override IEnumerator StartLevel()
        {
            SetHelperMessage("Olá novamente!");
            yield return new WaitForSeconds(1);

            SetHelperMessage("Nesse nível continuaremos a ajudar o personagem na separação do lixo.");
            yield return new WaitForSeconds(1);

            SetHelperMessage("Dessa vez vamos classificar algumas imagens!");
            ActivateImagePanel();
            SetNextCheckPoint(1);
        }

        private IEnumerator GarbageStartScript()
        {
            yield return new WaitForSeconds(0);
        }

        private IEnumerator FinalScript()
        {
            SetHelperMessage("Muito bem, você já sabe as cores para reciclar papel e vidro.");
            yield return new WaitForSeconds(1);

            SetHelperMessage("Você venceu o jogo!");
            SetNextCheckPoint();
        }

        private void ActivateImagePanel()
        {
            if (imagePanel != null) imagePanel.SetActive(true);
        }

        protected override bool HandleSpecialCheckPoint(string destinationName)
        {
            switch (destinationName)
            {
                case "CheckPoint3":
                    StartCoroutine(GarbageStartScript());
                    return true;
                default:
                    return false;
            }
        }

        public override void OnNext(EventPuzzle puzzleEvent)
        {
            base.OnNext(puzzleEvent);
            if (puzzleEvent.Status != EPuzzleStatus.Solved)
            {
                return;
            }
        }
    }
}