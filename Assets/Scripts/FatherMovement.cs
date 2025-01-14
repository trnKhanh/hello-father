using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Change Corroutine
// 
public class FatherMovement : MonoBehaviour
{
    public Transform Target;
    public float UpdateSpeed = 0.1f;
    public AudioSource SoundSource;
    public List<Transform> Checkpoints = new List<Transform>();
    public float ViewAngle = 45f;
    public float ChaseDistance = 10f;

    private int CurrentCheckpointIndex = 0;
    private float ReachCheckpointThreshold = 2f;
    private float CatchedThreshold = 5f;
    private NavMeshAgent Agent;
    private Animator Animator;

    private bool isHearedSound = false;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }

    public void Start()
    {
        StartCoroutine(FollowRoute());
    }

    private IEnumerator FollowRoute()
    {
        WaitForSeconds wait = new WaitForSeconds(UpdateSpeed);

        while (enabled)
        {
            float distance = Vector3.Distance(Target.position, transform.position);
            if (distance <= CatchedThreshold)
            {
                Animator.SetBool("IsChased", true); // Chased -> Stop
            }
            else
            {
                Animator.SetBool("IsChased", false); // Walking
            }

            if (ShouldChase())
            {
                Debug.Log("COMMING...");
                Agent.SetDestination(Target.position);
            }
            else
            {
                PatrolRoute();
            }

            yield return wait;
        }
    }

    private bool IsReachedDestination()
    {
        return true;
    }

    private void PatrolRoute()
    {
        if (Checkpoints.Count == 0) return;

        Transform currentCheckpoint = Checkpoints[CurrentCheckpointIndex];
        if (IsReachedDestination())
        {
            // Timeout for idle.
            // Change destination.
            Agent.SetDestination(currentCheckpoint.position);
        }

        float distanceToCheckpoint = Vector3.Distance(transform.position, currentCheckpoint.position);
        if (distanceToCheckpoint <= ReachCheckpointThreshold)
        {
            CurrentCheckpointIndex = (CurrentCheckpointIndex + 1) % Checkpoints.Count;
        }
    }

    private bool ShouldChase()
    {
        float distanceToTarget = Vector3.Distance(transform.position, Target.position);
        // If player makes sound, chasing
        if (SoundSource != null && SoundSource.isPlaying && distanceToTarget <= ChaseDistance)
        {
            return true;
        }

        Vector3 directionToTarget = (Target.position - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        // If father can't see player, not chasing
        if (angleToTarget > ViewAngle / 2)
        {
            return false;
        }
        else
        {
            if (Physics.Raycast(transform.position + Vector3.up, directionToTarget, out RaycastHit hit))
            {
                if (hit.transform == Target)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void hear(/*position of sound, ambe of sound*/)
    {
        // If hearing, update parameter
    }
}
