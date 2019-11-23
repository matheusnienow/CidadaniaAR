using System;
using System.Collections;
using Controller;
using Enum;
using Observer;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Manager
{
    public abstract class LevelManager : MonoBehaviour, IObserver<EventPuzzle>,
        IObserver<EventTargetTracking>, IObserver<EventPlayerDestinationReached>
    {
        [SerializeField] protected NavMeshAgentController playerController;
        [SerializeField] protected MyTrackableEventHandler levelTargetHandler;
        [SerializeField] protected GameObject objectiveGameObject;
        [SerializeField] protected GameObject greenPanel;
        [SerializeField] protected GameObject yellowPanel;
        [SerializeField] protected GameObject endPanel;
        [SerializeField] protected GameObject messagePanel;

        private TextMeshProUGUI _objectiveText;
        private IDisposable _targetUnsubscriber;
        private bool _isLevelStarted;
        private int _checkPointIndex;

        protected virtual void Start()
        {
            _isLevelStarted = false;
            Init();
            levelTargetHandler.Subscribe(this);
            playerController.Subscribe(this);
            SubscribeOnPuzzleEvents();
        }

        protected abstract void SubscribeOnPuzzleEvents();

        private void Init()
        {
            _objectiveText = objectiveGameObject.GetComponent<TextMeshProUGUI>();
        }

        protected void SetHelperMessage(string message)
        {
            if (_objectiveText == null)
            {
                _objectiveText = objectiveGameObject.GetComponent<TextMeshProUGUI>();
            }
            _objectiveText.SetText(message);
        }

        protected bool SetNextCheckPoint()
        {
            var nextIndex = _checkPointIndex + 1;
            var result = SetNextCheckPoint(nextIndex);
            if (result)
            {
                _checkPointIndex = nextIndex;
            }

            return result;
        }

        protected bool SetNextCheckPoint(int index)
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

        protected abstract IEnumerator StartLevel();

        private static IEnumerator DisableAfter(GameObject panel, int seconds)
        {
            yield return new WaitForSeconds(seconds);
            panel.SetActive(false);
        }

        public virtual void OnNext(EventPuzzle puzzleEvent)
        {
            switch (puzzleEvent.Status)
            {
                case PuzzleStatus.InProgress:
                    yellowPanel.SetActive(true);
                    greenPanel.SetActive(false);
                    break;
                case PuzzleStatus.Solved:
                    yellowPanel.SetActive(false);
                    greenPanel.SetActive(true);
                    break;
                case PuzzleStatus.NotSolved:
                    yellowPanel.SetActive(false);
                    greenPanel.SetActive(false);
                    break;
                default:
                    yellowPanel.SetActive(false);
                    greenPanel.SetActive(true);
                    break;
            }

            if (puzzleEvent.IsTimed)
            {
                StartCoroutine(DisableAfter(greenPanel, 3));
            }
        }

        public void OnNext(EventTargetTracking targetTracking)
        {
            if (!targetTracking.IsVisible || _isLevelStarted) return;

            _isLevelStarted = true;
            Debug.Log("LevelManager: Starting level...");
            StartCoroutine(StartLevel());
        }

        public void OnNext(EventPlayerDestinationReached destinationReachedEvent)
        {
            if (destinationReachedEvent?.Destination != null)
            {
                var destinationName = destinationReachedEvent.Destination != null
                    ? destinationReachedEvent.Destination.name
                    : null;

                var isSpecialCheckPoint = HandleSpecialCheckPoint(destinationName);
                if (isSpecialCheckPoint)
                {
                    return;
                }
            }

            var result = SetNextCheckPoint();
            if (!result)
            {
                EndGame();
            }
        }

        protected void EndGame()
        {
            if (messagePanel != null) messagePanel.SetActive(false);
            endPanel.SetActive(true);
        }

        protected abstract bool HandleSpecialCheckPoint(string destinationName);

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}