using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Animator _animator;
    private bool _isMoving;
    private static readonly int SpeedF = Animator.StringToHash("Speed_f");
    private static readonly int StaticB = Animator.StringToHash("Static_b");

    public void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null) return;
        
        // Set the character animation to idle and enable non-static animations.
        _animator.SetFloat(SpeedF, 0.0f);
        _animator.SetBool(StaticB, false);
    }

    public void Update()
    {
        // Process the movement input.
        ProcessMovementInput();
    }
    
    private void ProcessMovementInput()
    {
        if (_animator == null) return;
        
        // Get the movement input (if any) from the horizontal and vertical axes.
        var weAxis = Input.GetAxis("Horizontal");
        var nsAxis = Input.GetAxis("Vertical");

        // Process the movement input.
        if ((Mathf.Abs(weAxis) > 0.0f) || (Mathf.Abs(nsAxis) > 0.0f))
        {
            // If the character is currently idle...
            if (!_isMoving)
            {
                // Transition to the walking state.
                _isMoving = true;
                _animator.SetFloat(SpeedF, 0.4f);
            }

            if (weAxis < 0.0f)
            {
                // If the character should be walking west, rotate them to face west.
                transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }
            else if (weAxis > 0.0f)
            {
                // If the character should be walking east, rotate them to face east.
                transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
            }
            else if (nsAxis < 0.0f)
            {
                // If the character should be walking south, rotate them to face south.
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            else if (nsAxis > 0.0f)
            {
                // If the character should be walking north, rotate them to face north.
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
        }
        else if (_isMoving)
        {
            // If there is no movement input and the character is currently moving, transition to the idle state.
            _isMoving = false;
            _animator.SetFloat(SpeedF, 0.0f);
        }
    }
}
