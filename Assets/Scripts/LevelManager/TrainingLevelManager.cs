using System;
using System.Collections;
using System.Threading;
using Enum;
using Observer;
using Puzzles;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LevelManager
{
    [RequireComponent(typeof(PuzzleBrokenObject))]
    public class TrainingLevelManager : MonoBehaviour, IObserver<EventPlayerDestinationReached>,
        IObserver<EventTargetTracking>, IObserver<EventPuzzle>
    {
        [FormerlySerializedAs("EndPanel")] public GameObject endPanel;

        [FormerlySerializedAs("PlayerController")] [SerializeField]
        private NavMeshAgentController playerController;

        [SerializeField] private MyTrackableEventHandler levelTargetHandler;
        [SerializeField] private GameObject objectiveObject;

        [SerializeField] private PuzzleIncompletePath puzzleIncompletePath;
        [SerializeField] private PuzzleBrokenObject puzzleMonkey;

        [SerializeField] private GameObject greenPanel;
        [SerializeField] private GameObject yellowPanel;

        [SerializeField] private CamHelper camHelper;

        private IDisposable _playerControllerUnsubscriber;
        private IDisposable _targetUnsubscriber;
        private TextMeshProUGUI _objectiveText;
        private int _checkPointIndex;
        private bool _isTutorialStarted;

        private float _levelTime;

        private void Start()
        {
            _levelTime = 0f;
            _isTutorialStarted = false;
            _checkPointIndex = 0;
            _playerControllerUnsubscriber = playerController.Subscribe(this);
            _targetUnsubscriber = levelTargetHandler.Subscribe(this);

            _objectiveText = objectiveObject != null ? objectiveObject.GetComponent<TextMeshProUGUI>() : null;

            if (puzzleIncompletePath != null) puzzleIncompletePath.Subscribe(this);
            if (puzzleMonkey != null) puzzleMonkey.Subscribe(this);
        }

        private IEnumerator StartTutorial()
        {
            _isTutorialStarted = true;

            SetHelperMessage("Oi, vamos aprender a jogar?");
            yield return new WaitForSeconds(5);

            SetHelperMessage("A posição do personagem está indicada pelo diamante verde.");
            yield return new WaitForSeconds(10);

            SetHelperMessage(
                "Seu objetivo é fazer com que o personagem consiga chegar na loja vermelha do outro lado da cidade.");
            yield return new WaitForSeconds(15);

            SetCheckPointDestination(1);
            SetHelperMessage("Veja, o personagem se move sozinho.");
            yield return new WaitForSeconds(10);

            SetHelperMessage("Parece que há um problema, não existe um caminho para o outro lado da cidade.");
            yield return new WaitForSeconds(10);

            SetHelperMessage(
                "Tente mover seu dispositivo de forma que seu ponto de perspectiva crie um caminho para o personagem. " +
                "A flecha na parte superior da tela aponta para o objeto que deve ser visualizado");
            camHelper.Init();
        }

        private IEnumerator StartPart2()
        {
            SetHelperMessage("Muito bem, o personagem conseguiu chegar no outro lado da cidade!");
            yield return new WaitForSeconds(5);

            SetNextCheckPoint();
            SetHelperMessage("Agora precisamos fazer com que ele chegue até a loja vermelha!");
            yield return new WaitForSeconds(5);

            SetHelperMessage("Visualize o letreiro com o nome do jogo para permitir a passagem do personagem!");
            camHelper.FocusOnSign();
            yield return new WaitForSeconds(5);
        }

        private IEnumerator StartPart3()
        {
            SetHelperMessage("Muito bem, o personagem conseguiu chegar à loja vermelha!");
            yield return new WaitForSeconds(5);

            SetHelperMessage("Agora ele deve voltar para casa.");
            yield return new WaitForSeconds(5);

            SetHelperMessage(
                "Para isso será necessário atravessar a ponte novamente. Porém, você já sabe como resolver isso, né?");
            SetNextCheckPoint();
            camHelper.FocusOnBridge();
            yield return new WaitForSeconds(5);
        }

        private IEnumerator StartPart4()
        {
            SetHelperMessage("Você aprendeu muito bem e já pode seguir para as próximas fases!");
            SetNextCheckPoint();
            yield return new WaitForSeconds(1);
        }

        private void SetHelperMessage(string message)
        {
            _objectiveText.SetText(message);
        }

        private bool SetNextCheckPoint()
        {
            var nextIndex = _checkPointIndex + 1;
            var result = SetCheckPointDestination(nextIndex);
            if (result)
            {
                _checkPointIndex = nextIndex;
            }

            return result;
        }

        private bool SetCheckPointDestination(int index)
        {
            var destination = GameObject.Find("CheckPoint" + index);
            if (destination == null)
            {
                return false;
            }

            Debug.Log("Level4Manager: SETTING CHECKPOINT " + index);
            playerController.Destination = destination;
            playerController.Move();

            _checkPointIndex = index;

            return true;
        }

        private bool HandleTutorialCheckPoint(string destinationName)
        {
            switch (destinationName)
            {
                case "CheckPoint1":
                    StartCoroutine(StartPart2());
                    return true;
                case "CheckPoint2":
                    StartCoroutine(StartPart3());
                    return true;
                case "CheckPoint5":
                    StartCoroutine(StartPart4());
                    return true;
                default:
                    return false;
            }
        }

        void IObserver<EventPlayerDestinationReached>.OnNext(EventPlayerDestinationReached value)
        {
            if (value?.Destination != null)
            {
                var destinationName = value.Destination != null ? value.Destination.name : null;
                Debug.Log("Level4Manager: DESTINATION REACHED: " + destinationName);

                var isTutorialCheckPoint = HandleTutorialCheckPoint(destinationName);
                if (isTutorialCheckPoint)
                {
                    return;
                }
            }

            var result = SetNextCheckPoint();
            if (!result)
            {
                GameManager.OnLevelCompleted(endPanel);
            }
        }

        public void OnNext(EventTargetTracking targetTracking)
        {
            if (targetTracking.IsVisible && !_isTutorialStarted)
            {
                StartCoroutine(StartTutorial());
            }
        }

        public void OnNext(EventPuzzle puzzleEvent)
        {
            bool showGreenPane;
            bool showYellowPane;

            switch (puzzleEvent.Status)
            {
                case EPuzzleStatus.InProgress:
                    showGreenPane = false;
                    showYellowPane = true;
                    break;
                case EPuzzleStatus.Solved:
                    showYellowPane = false;
                    showGreenPane = true;
                    break;
                default:
                    showGreenPane = false;
                    showYellowPane = false;
                    break;
            }

            greenPanel.SetActive(showGreenPane);
            yellowPanel.SetActive(showYellowPane);

            if (puzzleEvent.IsTimed)
            {
                StartCoroutine(DisableAfterSeconds(greenPanel, 3));
            }
        }

        private IEnumerator DisableAfterSeconds(GameObject target, int seconds)
        {
            yield return new WaitForSeconds(seconds);
            target.SetActive(false);
        }

        void IObserver<EventPlayerDestinationReached>.OnCompleted()
        {
            _playerControllerUnsubscriber.Dispose();
        }

        public void OnCompleted()
        {
            _targetUnsubscriber.Dispose();
        }

        void IObserver<EventPlayerDestinationReached>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }
    }
}