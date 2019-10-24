using Assets.Scripts.Observer;
using Command;
using Puzzles.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzles
{
    public class PuzzleBrokenObject : OneTimePuzzle
    {
        private Camera cam;

        public Transform brokenObject;
        public Transform target;

        private bool isOk;
        private bool wasOk;

        private GameObject textObject;
        private GameObject textHintObject;
        private Text textMonkey;
        private Text textHint;

        private float timer = 0.0f;

        private new void Start()
        {
            base.Start();
            cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            textObject = GameObject.Find("MonkeyText");
            textHintObject = GameObject.Find("DotText");
            textMonkey = textObject.GetComponent<Text>();
            textHint = textHintObject.GetComponent<Text>();
        }

        private bool IsCameraDirectionCorrect()
        {
            //Debug.DrawRay(target.transform.position, target.transform.forward * 1, Color.red);
            //Debug.DrawRay(cam.transform.position, cam.transform.forward * 1, Color.red);

            var dot = Vector3.Dot(brokenObject.transform.forward.normalized, cam.transform.forward.normalized);
            //Debug.Log("Dot product: "+dot);
            return Mathf.Abs(dot) < 1.3 && Mathf.Abs(dot) > 0.90;
        }

        private bool IsCameraPositionCorrect()
        {
            var deltaY = brokenObject.transform.position.y - cam.transform.position.y;
            //var deltaZ = brokenObject.transform.position.z - cam.transform.position.z;

            //Debug.Log("DeltaY: "+deltaY+" | DeltaZ: "+deltaZ);

            var isYPositionCorrect = Mathf.Abs(deltaY) > 0.0 && Mathf.Abs(deltaY) < 1;
            //bool Zpass = Mathf.Abs(deltaZ) > 8.4 && Mathf.Abs(deltaZ) < 8.6;

            //return Ypass && Zpass;
            return isYPositionCorrect;
        }

        protected override Message GetOnNextMessage()
        {
            throw new System.NotImplementedException();
        }

        protected override bool IsConditionMet()
        {
            var camDirection = IsCameraDirectionCorrect();
            var camPosition = IsCameraPositionCorrect();
            Debug.Log("Cam Direction: " + camDirection + " | Cam Position: " + camPosition);

            return camDirection && camPosition;
        }

        protected override void OnConditionMet()
        {
            if (!wasOk)
            {
                textMonkey.text = "MONKEY VISIBLE (target: "+ brokenObject.name+")";
            }

            timer += Time.deltaTime;
            var seconds = timer % 60;

            if (seconds < 2)
            {
                return;
            }
        
            target.GetComponent<ActivateEntityCommand>().Execute();
            gameObject.SetActive(false);

            wasOk = true;
        }

        protected override void OnConditionNotMet()
        {
            timer = 0.0f;
            if (wasOk)
            {
                textMonkey.text = "MONKEY NOT VISIBLE";
            }

            wasOk = false;
        }
    }
}
