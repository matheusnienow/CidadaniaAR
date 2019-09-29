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

        private void Start()
        {
            PlayerControlerUnsubscriber = PlayerController.Subscribe(this);
            SetFirstCheckPoint();
        }

        private void SetFirstCheckPoint()
        {
            Debug.Log("SETTING FIRST CHECKPOINT");
            PlayerController.Destination = GameObject.Find("FirstCheckPoint");
            PlayerController.Move();
        }

        private void SetSecondCheckPoint()
        {
            PlayerController.Destination = GameObject.Find("SecondCheckPoint");
            PlayerController.Move();
        }

        private void CheckFirstCheckpoint()
        {
            //IncompletePath


        }

        void IObserver<EventPlayerDestinationReached>.OnNext(EventPlayerDestinationReached value)
        {
            if (value.Destination.name == "FirstCheckPoint")
            {
                GameManager.OnLevelCompleted(EndPanel);
            }
        }

        void IObserver<EventPlayerDestinationReached>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        void IObserver<EventPlayerDestinationReached>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }
    }
}
