using System;
using Assets.Scripts.Observer;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace LevelManager
{
    public class Level4Manager : MonoBehaviour, IObserver<EventPlayerDestinationReached>
    {
        [FormerlySerializedAs("EndPanel")] public GameObject endPanel;
        [FormerlySerializedAs("PlayerController")] public PlayerMovementController playerController;
        public MyTrackableEventHandler levelTargetHandler;
        public Vector3 spawnPosition;
        public int checkPointIndex = 0;
        private IDisposable _unsubscriber;

        private void Start()
        {
            _unsubscriber = playerController.Subscribe(this);
            //playerController.Warp(spawnPosition);
            SetNextCheckPoint(checkPointIndex);
        }

        private bool SetNextCheckPoint(int index)
        {
            var destination = GameObject.Find("CheckPoint" + (checkPointIndex+1));
            if (destination == null)
            {
                return false;
            }

            checkPointIndex++;
            Debug.Log("Level4Manager: SETTING CHECKPOINT " + checkPointIndex);
            playerController.Destination = destination;
            playerController.Move();
            return true;

        }

        void IObserver<EventPlayerDestinationReached>.OnNext(EventPlayerDestinationReached value)
        {
            var result = SetNextCheckPoint(checkPointIndex);
            if (!result)
            {
                GameManager.OnLevelCompleted(endPanel);
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