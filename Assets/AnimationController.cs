using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        var isMoving = _navMeshAgent.velocity != Vector3.zero;
        _animator.SetBool(IsMoving, isMoving);
    }
}