using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Assets.Scripts.Puzzles;
using Assets.Scripts.Enum;
using System;
using Assets.Scripts.Observer;

public class PuzzleIncompletePath : OneTimePuzzle
{
    [SerializeField, Range(0, 1f)]
    public float CameraDirectionThreshold;

    [SerializeField, Range(0, 1f)]
    public float CameraPositionThreshold;

    public GameObject outOfPathBlock;
    [SerializeField]
    public DirectionEnum outOfPathBlockDirection;

    public GameObject invisibleBlock;
    public GameObject NavMesh;

    private Camera cam;
    private Text text;    
    private ICommand buildNavMeshCommand;

    private bool passageAllowed;
    private bool wasPassageAllowed;

    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        text = GameObject.Find("DebugText").GetComponent<Text>();

        NavMeshSurface navMeshSurface = NavMesh.GetComponent<NavMeshSurface>();
        buildNavMeshCommand = new BuildNavMeshCommand(navMeshSurface);
    }

    bool CheckCameraDirection()
    {
        var camDirectionVector = cam.transform.forward;
        var outOfPathBlockDirectionVector = GetDirection(outOfPathBlock, outOfPathBlockDirection);

        Debug.DrawRay(outOfPathBlock.transform.position, outOfPathBlockDirectionVector * 10, Color.red);
        Debug.DrawRay(cam.transform.position, camDirectionVector * 10, Color.red);

        var dotAbs = Mathf.Abs(Vector3.Dot(outOfPathBlockDirectionVector.normalized, camDirectionVector.normalized));

        var maxValue = (1 + CameraDirectionThreshold);
        var minValue = (1 - CameraDirectionThreshold);

        return dotAbs < maxValue && dotAbs > minValue;
    }

    private Vector3 GetDirection(GameObject outOfPathBlock, DirectionEnum outOfPathBlockDirection)
    {
        switch (outOfPathBlockDirection)
        {
            case DirectionEnum.FORWARD:
                return outOfPathBlock.transform.forward;
            case DirectionEnum.BACKWARD:
                return outOfPathBlock.transform.forward * -1;
            case DirectionEnum.RIGHT:
                return outOfPathBlock.transform.right;
            case DirectionEnum.LEFT:
                return outOfPathBlock.transform.right * -1;
        }

        return new Vector3();
    }

    bool CheckCameraPosition()
    {
        var deltaY = outOfPathBlock.transform.position.y - cam.transform.position.y;
        bool Ypass = Mathf.Abs(deltaY) < CameraPositionThreshold;

        return Ypass;
    }

    protected override bool IsConditionMet()
    {
        bool camDirection = CheckCameraDirection();
        bool camPosition = CheckCameraPosition();

        Debug.Log("DIRECTION: " + camDirection + " | POSITION: " + camPosition);

        return camDirection && camPosition;
    }

    protected override void OnConditionMet()
    {
        if (!wasPassageAllowed)
        {
            text.text = "PASSAGE ALLOWED";
            BuilNavMesh(9);
        }

        wasPassageAllowed = true;
    }

    protected override void OnConditionNotMet()
    {
        if (wasPassageAllowed)
        {
            text.text = "PASSAGE NOT ALLOWED";
            BuilNavMesh(0);
        }

        wasPassageAllowed = false;
    }

    private void BuilNavMesh(int layer)
    {
        invisibleBlock.layer = layer;
        buildNavMeshCommand.Execute();
    }

    internal override Message GetOnNextMessage()
    {
        throw new NotImplementedException();
    }
}

