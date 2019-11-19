using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Enum;
using Model;
using Observer;
using Puzzles;
using UnityEngine;
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
        private Queue<GarbageImage> _glassImagesQueue;
        private Queue<GarbageImage> _paperImagesQueue;
        private bool _isImageQueueReady;

        private const string ResourcesPrefix = "/Resources/";
        private const string GlassFolderPath = "Glass";
        private const string PaperFolderPath = "Paper";

        protected override void Start()
        {
            base.Start();
            _image = imagePanel.transform.GetChild(0).GetComponent<Image>();
            LoadImages();
        }

        private void LoadImages()
        {
            _glassImagesQueue = new Queue<GarbageImage>();
            _paperImagesQueue = new Queue<GarbageImage>();

            var glassSprites = LoadSprites(GlassFolderPath).ToList();
            glassSprites.ForEach(s => _glassImagesQueue.Enqueue(new GarbageImage(s, GarbageType.Glass)));

            var paperSprites = LoadSprites(PaperFolderPath).ToList();
            paperSprites.ForEach(s => _paperImagesQueue.Enqueue(new GarbageImage(s, GarbageType.Paper)));

            _isImageQueueReady = true;
        }

        private static IEnumerable<Sprite> LoadSprites(string path)
        {
            return Resources.LoadAll<Sprite>(path);
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
            while (!_isImageQueueReady)
            {
                SetHelperMessage("Carregando imagens!");
                yield return new WaitForSeconds(1);
            }

            SetHelperMessage(
                "No painel superior direito será mostrada a imagem de um produto que deve ser reciclado. " +
                "Você deve indicar em que lixo esse produto deve ser jogado");
            yield return new WaitForSeconds(1);

            SetHelperMessage("Existem duas ilhas, cada uma com um tipo de lixeira. Um verde e outro azul.");
            yield return new WaitForSeconds(1);

            SetHelperMessage(
                "Para indicar o lixeiro a ser usado, crie o caminho para o personagem chegar até a lixeira.");
            yield return new WaitForSeconds(1);

            StartCoroutine(SetSprites());
        }

        private IEnumerator SetSprites()
        {
            Debug.Log($"Manager: setting sprites {_glassImagesQueue.Count}");
            while (_glassImagesQueue.Count > 0)
            {
                var image = _glassImagesQueue.Dequeue();
                Debug.Log($"Manager: sprite loaded({image.Sprite})");

                _image.sprite = image.Sprite;
                _image.overrideSprite = image.Sprite;
                _image.type = Image.Type.Simple;
                _image.preserveAspect = true;

                yield return new WaitForSeconds(5);
            }
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
            if (puzzleEvent.Status != PuzzleStatus.Solved)
            {
                return;
            }
        }
    }
}