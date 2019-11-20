using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enum;
using Model;
using Observer;
using Puzzles;
using Puzzles.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class BridgeLevelManager : LevelManager
    {
        [SerializeField] private GameObject imagePanel;
        [SerializeField] private GameObject glassCheckPoint;
        [SerializeField] private GameObject paperCheckPoint;

        [SerializeField] private GameObject glassPuzzle;
        [SerializeField] private GameObject paperPuzzle;

        [SerializeField] private GameObject glassDumpster;
        [SerializeField] private GameObject paperDumpster;

        [SerializeField] private GameObject glassBridge;
        [SerializeField] private GameObject paperBridge;

        private Image _image;
        private GameObject _imageGameObject;
        private Queue<GarbageImage> _glassImagesQueue;
        private Queue<GarbageImage> _paperImagesQueue;
        private bool _isImageQueueReady;

        private const string GlassFolderPath = "Glass";
        private const string PaperFolderPath = "Paper";

        private GarbageImage _currentGarbageImage;

        private Vector3 _playerCheckPointPosition;
        private int _score;
        private bool _isSolved;

        protected override void Start()
        {
            base.Start();
            Init();
            EnablePuzzles(false);
            LoadImages();
        }

        private void EnablePuzzles(bool shouldEnable)
        {
            glassPuzzle.GetComponent<Puzzle>().enabled = shouldEnable;
            paperPuzzle.GetComponent<Puzzle>().enabled = shouldEnable;
        }

        private void Init()
        {
            //_playerCheckPointPosition = new Vector3(15f, 0.5f, -150f);
            _imageGameObject = imagePanel.transform.GetChild(0).gameObject;
            _image = _imageGameObject.GetComponent<Image>();
            _playerCheckPointPosition = Vector3.zero;
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
            glassPuzzle.GetComponent<Puzzle>().Subscribe(this);
            paperPuzzle.GetComponent<Puzzle>().Subscribe(this);
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
                SetHelperMessage("Aguarde, o jogo está carregando as imagens!");
                yield return new WaitForSeconds(0.3f);
            }

            SetHelperMessage(
                "No painel superior direito será mostrada a imagem de um produto que deve ser reciclado. " +
                "Você deve indicar em que lixo esse " +
                "produto deve ser jogado");
            yield return new WaitForSeconds(1);

            SetHelperMessage("Existem duas ilhas, cada uma com um tipo de lixeira. Um verde e outro azul.");
            yield return new WaitForSeconds(1);

            SetHelperMessage(
                "Para indicar o lixeiro a ser usado, crie o caminho para o personagem chegar até a lixeira.");
            yield return new WaitForSeconds(1);

            DoGarbageGameLoop();
        }

        private IEnumerator ResultScript(bool correctResponse)
        {
            var text = correctResponse ? "Parabéns! Você acertou." : "Que pena! Você errou.";
            SetHelperMessage(text);
            yield return new WaitForSeconds(5);

            DoGarbageGameLoop();
        }

        private IEnumerator FinalScript()
        {
            SetHelperMessage("Fim do jogo. Veja sua pontuação no painel ao lado.");
            yield return new WaitForSeconds(10);

            EndGame();
        }

        private void DoGarbageGameLoop()
        {
            var hasGarbageToRecycle = HasGarbageToRecycle();
            if (!hasGarbageToRecycle)
            {
                StartCoroutine(FinalScript());
                return;
            }

            ResetPlayerPosition();
            ShowNextImage();
            EnablePuzzles(true);
        }

        private void ShowNextImage()
        {
            _currentGarbageImage = DequeueGarbageImage();
            SetGarbageImage(_currentGarbageImage);
        }

        private void ResetPlayerPosition()
        {
            if (_playerCheckPointPosition == Vector3.zero)
            {
                _playerCheckPointPosition = playerController.transform.position;
                return;
            }

            playerController.Destination = null;
            playerController.Stop();
            playerController.gameObject.transform.position = new Vector3(15f, 2f, -150f);
        }

        private GarbageImage DequeueGarbageImage()
        {
            Queue<GarbageImage> queue;

            if (_paperImagesQueue.Count == 0)
            {
                queue = _glassImagesQueue;
            }
            else if (_glassImagesQueue.Count == 0)
            {
                queue = _paperImagesQueue;
            }
            else
            {
                var rnd = new System.Random();
                var index = rnd.Next(1, 3);

                queue = index == 1 ? _paperImagesQueue : _glassImagesQueue;
            }

            return queue.Dequeue();
        }

        private bool HasGarbageToRecycle()
        {
            return _glassImagesQueue.Count > 0 || _paperImagesQueue.Count > 0;
        }

        private void SetGarbageImage(GarbageImage image)
        {
            _imageGameObject.SetActive(true);
            _image.sprite = image.Sprite;
            _image.overrideSprite = image.Sprite;
            _image.type = Image.Type.Simple;
            _image.preserveAspect = true;
        }

        private void ActivateImagePanel()
        {
            if (imagePanel != null) imagePanel.SetActive(true);
        }

        protected override bool HandleSpecialCheckPoint(string destinationName)
        {
            if (destinationName == "CheckPoint3")
            {
                StartCoroutine(GarbageStartScript());
                return true;
            }

            if (destinationName == glassDumpster.name)
            {
                OnGlassCheckPointReached();
                return true;
            }

            if (destinationName == paperDumpster.name)
            {
                OnPaperCheckPointReached();
                return true;
            }

            return false;
        }

        private void OnPaperCheckPointReached()
        {
            CheckResult(GarbageType.Paper);
        }

        private void OnGlassCheckPointReached()
        {
            CheckResult(GarbageType.Glass);
        }

        private void CheckResult(GarbageType garbageType)
        {
            if (garbageType == _currentGarbageImage.GarbageType)
            {
                _score++;
                StartCoroutine(ResultScript(true));
            }
            else
            {
                StartCoroutine(ResultScript(false));
            }
        }

        public override void OnNext(EventPuzzle puzzleEvent)
        {
            base.OnNext(puzzleEvent);
            if (puzzleEvent.Status != PuzzleStatus.Solved)
            {
                _isSolved = false;
                return;
            }

            if (_isSolved) return;
            OnPuzzleSolved(puzzleEvent);
        }

        private void OnPuzzleSolved(EventPuzzle puzzleEvent)
        {
            _isSolved = true;

            if (puzzleEvent.GameObjectName == glassBridge.name)
            {
                Debug.Log("BridgeLevelManager: GLASS PuzzleSolved");
                SetNextCheckPoint(glassCheckPoint);
                return;
            }

            if (puzzleEvent.GameObjectName == paperBridge.name)
            {
                Debug.Log("BridgeLevelManager: PAPER PuzzleSolved");
                SetNextCheckPoint(paperCheckPoint);
                return;
            }

            SetNextCheckPoint();
        }

        private void SetNextCheckPoint(GameObject checkPoint)
        {
            playerController.Destination = checkPoint;
            playerController.Move();
        }
    }
}