using System;
using System.Collections;
using Controller;
using Enum;
using Observer;
using Puzzles;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    [RequireComponent(typeof(PuzzleBrokenObject))]
    public class TrainingLevelManager : LevelManager
    {

        [SerializeField] private PuzzleIncompletePath puzzleIncompletePath;
        [SerializeField] private PuzzleBrokenObject puzzleMonkey;

        [SerializeField] private CamHelper camHelper;

        private IDisposable _playerControllerUnsubscriber;
        private IDisposable _targetUnsubscriber;

        protected override void SubscribeOnPuzzleEvents()
        {
            if (puzzleIncompletePath != null) puzzleIncompletePath.Subscribe(this);
            if (puzzleMonkey != null) puzzleMonkey.Subscribe(this);
        }

        protected override IEnumerator StartLevel()
        {
            SetHelperMessage("Oi, vamos aprender a jogar?");
            yield return new WaitForSeconds(5);

            SetHelperMessage("A posição do personagem está indicada pelo diamante verde.");
            yield return new WaitForSeconds(10);

            SetHelperMessage(
                "Seu objetivo é fazer com que o personagem consiga chegar na loja vermelha do outro lado da cidade.");
            yield return new WaitForSeconds(15);

            SetNextCheckPoint(1);
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

        protected override bool HandleSpecialCheckPoint(string destinationName)
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
    }
}