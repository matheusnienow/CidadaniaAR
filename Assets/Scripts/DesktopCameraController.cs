using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class CameraScript : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Rigidbody rig;

    private float x = 0.0f;
    private float y = 0.0f;
    private bool rightClicked;

    // Use this for initialization
    private void Start()
    {
        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rig = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rig != null)
        {
            rig.freezeRotation = true;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rightClicked = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            rightClicked = false;
        }
    }

    public void LateUpdate()
    {
        if (!target || !rightClicked)
        {
            return;
        }
        
        x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        var rotation = Quaternion.Euler(y, x, 0);

        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

        if (Physics.Linecast(target.position, transform.position, out var hit))
        {
            distance -= hit.distance;
        }
        var negDistance = new Vector3(0.0f, 0.0f, -distance);
        var position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}