using System;
using Assets.Scripts.Observer;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LevelManager
{
    public class Level4Manager : MonoBehaviour, IObserver<EventPlayerDestinationReached>
    {
        [FormerlySerializedAs("EndPanel")] public GameObject endPanel;
        public GameObject UI;
        [FormerlySerializedAs("PlayerController")]
        public NavMeshAgentController playerController;
        public MyTrackableEventHandler levelTargetHandler;
        public int checkPointIndex = 0;
        public GameObject objectiveObject;
        
        private IDisposable _unsubscriber;
        private TextMeshProUGUI _objectiveText;

        private void Start()
        {
            _unsubscriber = playerController.Subscribe(this);
            SetNextCheckPoint(checkPointIndex);

            if (objectiveObject != null)
            {
                _objectiveText = objectiveObject.GetComponent<TextMeshProUGUI>();
            }
        }

        private bool SetNextCheckPoint(int index)
        {
            var destination = GameObject.Find("CheckPoint" + (checkPointIndex + 1));
            if (destination == null)
            {
                return false;
            }

            var checkPointInfo = destination.GetComponent<CheckPointInfo>();
            SetObjective(checkPointInfo);

            checkPointIndex++;
            Debug.Log("Level4Manager: SETTING CHECKPOINT " + checkPointIndex);
            playerController.Destination = destination;
            playerController.Move();
            return true;
        }

        private void SetObjective(CheckPointInfo checkPointInfo)
        {
            if (checkPointInfo == null) return;
            if (objectiveObject == null) return;
            
            _objectiveText = objectiveObject.GetComponent<TextMeshProUGUI>();
            
            var text = checkPointInfo.objectiveText;
            if (string.IsNullOrEmpty(text)) return;
            
            var objective = "Objetivo: " + text;
            _objectiveText.SetText(objective);
            Debug.Log("Level4Manager: OBJECTIVE SET: " + objective);
        }

        void IObserver<EventPlayerDestinationReached>.OnNext(EventPlayerDestinationReached value)
        {
            var result = SetNextCheckPoint(checkPointIndex);
            if (!result)
            {
                GameManager.OnLevelCompleted(endPanel, UI);
            }
        }

        void IObserver<EventPlayerDestinationReached>.OnCompleted()
        {
            _unsubscriber.Dispose();
        }

        void IObserver<EventPlayerDestinationReached>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }
    }
}