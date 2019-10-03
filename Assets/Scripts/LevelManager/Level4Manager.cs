using Assets.Scripts.Observer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelManager
{
    public class Level4Manager : MonoBehaviour, IObserver<EventPlayerDestinationReached>
    {
        public GameObject EndPanel;
        public PlayerMovementController PlayerController;
        public IDisposable PlayerControlerUnsubscriber { get; private set; }

        public int checkPointIndex = 0;

        private void Start()
        {
            PlayerControlerUnsubscriber = PlayerController.Subscribe(this);
            SetNextCheckPoint(checkPointIndex);
        }

        private bool SetNextCheckPoint(int index)
        {
            var destination = GameObject.Find("CheckPoint" + (checkPointIndex+1));
            if (destination != null)
            {
                Debug.Log("Level4Manager: SETTING CHECKPOINT " + ++checkPointIndex);
                PlayerController.Destination = GameObject.Find("CheckPoint" + checkPointIndex);
                PlayerController.Move();
                return true;
            } else
            {
                return false;
            }
            
        }

        void IObserver<EventPlayerDestinationReached>.OnNext(EventPlayerDestinationReached value)
        {
            var result = SetNextCheckPoint(checkPointIndex);
            if (!result)
            {
                GameManager.OnLevelCompleted(EndPanel);
            }
        }

        void IObserver<EventPlayerDestinationReached>.OnCompleted()
        {
            
        }

        void IObserver<EventPlayerDestinationReached>.OnError(Exception error)
        {
            
        }
    }
}
