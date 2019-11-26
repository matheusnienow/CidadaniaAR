using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enum;
using Observer;
using Puzzles.Base;
using TMPro;
using UnityEngine;

namespace Manager
{
    public class MonkeyLevelManager : LevelManager
    {
        [SerializeField] private List<GameObject> metalPuzzles;
        [SerializeField] private List<GameObject> metalGameObjects;

        [SerializeField] private List<GameObject> plasticPuzzles;
        [SerializeField] private List<GameObject> plasticGameObjects;

        [SerializeField] private GameObject scorePanel;
        private TextMeshProUGUI _scoreText;

        private Dictionary<GameObject, bool> _objectStatus;
        private int Score { get; set; }

        private bool _isMetalPuzzlesCompleted;

        protected override void Start()
        {
            base.Start();
            InitComponents();
            CreateObjectDictionary();
            DisablePuzzles();
        }

        private void InitComponents()
        {
            _scoreText = scorePanel.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void DisablePuzzles()
        {
            plasticPuzzles.ForEach(p => p.GetComponent<Puzzle>().enabled = false);
            metalPuzzles.ForEach(p => p.GetComponent<Puzzle>().enabled = false);
        }

        private void ActivateMetalPuzzle()
        {
            metalPuzzles.ForEach(p => p.GetComponent<Puzzle>().enabled = true);
        }

        private void ActivatePlasticPuzzle()
        {
            plasticPuzzles.ForEach(p => p.GetComponent<Puzzle>().enabled = true);
        }

        private void CreateObjectDictionary()
        {
            _objectStatus = new Dictionary<GameObject, bool>();
            metalGameObjects.ForEach(mg => { _objectStatus.Add(mg, false); });
            plasticGameObjects.ForEach(pg => { _objectStatus.Add(pg, false); });
        }

        protected override void SubscribeOnPuzzleEvents()
        {
            metalPuzzles.ForEach(mp => mp.GetComponent<Puzzle>().Subscribe(this));
            plasticPuzzles.ForEach(pp => pp.GetComponent<Puzzle>().Subscribe(this));
        }

        protected override IEnumerator StartLevel()
        {
            SetHelperMessage("Olá novamente!");
            yield return new WaitForSeconds(3);

            SetHelperMessage("Seu objetivo nesse nível é ajudar o personagem na separação do lixo.");
            yield return new WaitForSeconds(5);

            SetHelperMessage("O personagem irá até duas lixeiras e você terá que ajudá-lo a encontrar os " +
                             "produtos corretos para cada uma delas.");
            yield return new WaitForSeconds(7);

            SetHelperMessage("Veja, o personagem está andando até a primeira lixeira.");
            SetNextCheckPoint(1);
        }

        private IEnumerator MetalGarbageScript()
        {
            SetHelperMessage("O personagem chegou na lixeira AMARELA. Que tipos de produtos devem ser jogados " +
                             "nessa leixeira?");
            yield return new WaitForSeconds(5);

            SetHelperMessage("Existem diversos prodtos espalhados ao redor do mapa.");
            yield return new WaitForSeconds(5);

            SetHelperMessage("Para poder avançar, visualize dois produtos feitos do material que devem ser jogados " +
                             "nessa lixeira. O painel superior direito indica a quantidade de produtos encontrados.");

            ActivateScorePanel();
            ActivateMetalPuzzle();
        }

        private IEnumerator MoveToPlasticScript()
        {
            yield return new WaitForSeconds(5);

            SetHelperMessage("Muito bem, você conseguiu!");
            yield return new WaitForSeconds(4);

            SetHelperMessage("Na lixeira amarela vão os produtos feitos de METAL.");
            yield return new WaitForSeconds(5);

            SetNextCheckPoint();
            SetHelperMessage("Certo, agora O personagem irá até a próxima lixeira. Faça a mesma coisa e " +
                             "ajude-o a encontrar os produtos corretos!");
        }

        private IEnumerator PlasticGarbageScript()
        {
            SetHelperMessage("O personagem chegou na lixeira vermelha. Os produtos feitos de que material devem ser " +
                             "jogados nessa leixeira?");
            yield return new WaitForSeconds(4);

            SetHelperMessage("Para poder avançar, visualize dois produtos que devem ser jogados nessa lixeira, " +
                             "os produtos estão espalhados ao redor do mapa.");

            ActivatePlasticPuzzle();
            UpdateScorePanel();
        }

        private IEnumerator FinalScript()
        {
            yield return new WaitForSeconds(5);

            SetHelperMessage("Muito bem, você já sabe as cores para reciclar metais e plásticos.");
            yield return new WaitForSeconds(5);

            SetHelperMessage("Vamos para a próxima fase!");
            yield return new WaitForSeconds(5);
            EndGame();
        }

        private void ActivateScorePanel()
        {
            if (scorePanel != null) scorePanel.SetActive(true);
        }

        protected override bool HandleSpecialCheckPoint(string destinationName)
        {
            switch (destinationName)
            {
                case "CheckPoint5":
                    StartCoroutine(MetalGarbageScript());
                    return true;
                case "CheckPoint9":
                    StartCoroutine(PlasticGarbageScript());
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

            SetProductFoundMessage(puzzleEvent.GameObjectName);
            RegisterObjectAsSolved(puzzleEvent.GameObjectName);
            UpdateScorePanel();
            CheckScore();
        }

        private void SetProductFoundMessage(string productName)
        {
            SetHelperMessage($"Produto encontrado: {productName}");
        }

        private void CheckScore()
        {
            if (Score != 2) return;

            if (!_isMetalPuzzlesCompleted)
            {
                OnMetalPuzzlesComplete();
            }
            else
            {
                OnPlasticPuzzlesComplete();
            }
        }

        private void OnPlasticPuzzlesComplete()
        {
            Score = 0;
            StartCoroutine(FinalScript());
        }

        private void OnMetalPuzzlesComplete()
        {
            _isMetalPuzzlesCompleted = true;
            Score = 0;
            StartCoroutine(MoveToPlasticScript());
        }

        private void UpdateScorePanel()
        {
            Score = GetScore();
            Debug.Log("Score: " + Score);

            _scoreText.SetText(Score + "/2");
        }

        private int GetScore()
        {
            var score = 0;
            var puzzleObjects = _isMetalPuzzlesCompleted ? plasticGameObjects : metalGameObjects;

            puzzleObjects.ForEach(po =>
                {
                    score += _objectStatus.Count(keyValuePair =>
                        po.name == keyValuePair.Key.name && keyValuePair.Value);
                }
            );

            return score;
        }

        protected override void EndGame()
        {
            scorePanel.SetActive(false);
            base.EndGame();
        }

        private void RegisterObjectAsSolved(string gameObjectName)
        {
            var key = _objectStatus
                .Where(keyValuePair => keyValuePair.Key.name == gameObjectName)
                .Select(keyValuePair => keyValuePair.Key).FirstOrDefault();

            if (key != null)
            {
                _objectStatus[key] = true;
            }
        }
    }
}