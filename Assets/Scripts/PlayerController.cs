using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public NavMeshSurface navMeshSurface;

    private GameObject outOfPathBlock;
    private GameObject invisibleBlock;
    private GameObject textObject;
    private Text text;
    private Vector3 destination;
    private bool passageAllowed;
    private bool wasPassageAllowed;


    private void Start()
    {
        outOfPathBlock = GameObject.FindWithTag("OutOfPath");
        invisibleBlock = GameObject.Find("InvisibleBlock");
        textObject = GameObject.FindWithTag("Text");
        text = textObject.GetComponent<Text>();
        destination = GameObject.FindWithTag("Destination").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckNavMesh();
        MovePlayer();
    }

    void CheckNavMesh()
    {
        bool camDirection = CheckCameraDirection();
        bool camPosition = CheckCameraPosition();

        //text.text = "DIRECTION: "+camDirection+" | POSITION: "+camPosition;

        Debug.Log("Cam Direction: " + camDirection + " | Cam Position: " + camPosition);

        passageAllowed = camDirection && camPosition;
        if (passageAllowed)
        {
            if (!wasPassageAllowed)
            {
                text.text = "PASSAGE ALLOWED";
                invisibleBlock.layer = 9;//path
                navMeshSurface.BuildNavMesh();
            }
        }
        else
        {
            if (wasPassageAllowed)
            {
                text.text = "PASSAGE FORBIDDEN";
                invisibleBlock.layer = 0;//default
                navMeshSurface.BuildNavMesh();
            }
        }
        wasPassageAllowed = passageAllowed;
    }

    bool CheckCameraDirection()
    {
        Debug.DrawRay(outOfPathBlock.transform.position, outOfPathBlock.transform.right * 1, Color.red);
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 1, Color.red);

        var dot = Vector3.Dot(outOfPathBlock.transform.right.normalized, cam.transform.forward.normalized);
        Debug.Log("Dot product: "+dot);
        return dot == -1;
    }

    bool CheckCameraPosition()
    {
        var deltaY = outOfPathBlock.transform.position.y - cam.transform.position.y;
        var deltaZ = outOfPathBlock.transform.position.z - cam.transform.position.z;

        //Debug.Log("DeltaY: "+deltaY+" | DeltaZ: "+deltaZ);

        bool Ypass = Mathf.Abs(deltaY) < 0.5;
        bool Zpass = Mathf.Abs(deltaZ) < 0.3;

        return Ypass && Zpass;
    }

    void MovePlayer()
    {
        var playerPosition = Mathf.Round(transform.position.z);
        var destinationPosition = Mathf.Round(destination.z);

        //Debug.Log("player: " + playerPosition + ", destination: " + destinationPosition);
        if (playerPosition != destinationPosition)
        {
            //Debug.Log("setting destination");
            agent.SetDestination(destination);
        }
        else
        {
            //Debug.Log("already on destination");
        }
    }
}
