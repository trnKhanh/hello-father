using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FatherMovement : MonoBehaviour
{
    public Transform Target;
    public float UpdateSpeed = 0.1f;
    public AudioSource SoundSource;
    public List<Transform> Checkpoints = new List<Transform>();
    public float ViewAngle = 45f;
    public float ChaseDistance = 10f;

    private int CurrentCheckpointIndex = 0;
    private float CatchedThreshold = 5f;
    private float IdleTimeAtCheckpoint = 5f;
    private NavMeshAgent Agent;
    private Animator Animator;

    private bool IsIdle = false;
    private float IdleTimer = 0f;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(Target.position, transform.position);

        if (distanceToTarget <= CatchedThreshold)
        {
            Animator.SetBool("IsCatched", true);
            Agent.ResetPath();
        }
        else
        {
            Animator.SetBool("IsCatched", false);
            if (ShouldChase())
            {
                Debug.Log("COMMING............");
                Agent.SetDestination(Target.position);
                Animator.SetBool("IsIdle", false);
                IsIdle = false;
            }
            else
            {
                PatrolRoute();
            }
        }
    }
  
    private bool IsReachedDestination()
    {
        if(!Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance)
        {
            if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                return true;
        }
        return false;
    }

    private void PatrolRoute()
    {
        if (Checkpoints.Count == 0) return;

        if (IsIdle)
        {
            IdleTimer += Time.deltaTime;
            if (IdleTimer >= IdleTimeAtCheckpoint)
            {
                IdleTimer = 0;
                IsIdle = false;
                MoveToNextCheckpoint();
            }
        }
        else
        {
            if (IsReachedDestination())
            {
                IsIdle = true;
                Animator.SetBool("IsIdle", true);
                Animator.SetBool("IsCatched", false);
                Agent.ResetPath();
            }
        }
    }

    private void MoveToNextCheckpoint()
    {
        if (Checkpoints.Count == 0) return;

        CurrentCheckpointIndex = (CurrentCheckpointIndex + 1) % Checkpoints.Count;
        Transform currentCheckpoint = Checkpoints[CurrentCheckpointIndex];
        Agent.SetDestination(currentCheckpoint.position);
        Animator.SetBool("IsCatched", false);
        Animator.SetBool("IsIdle", false);
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

    public void hear(Vector3 soundPosition, float soundRadius)
    {
        float distanceToSound = Vector3.Distance(transform.position, soundPosition);

        if (distanceToSound <= soundRadius)
        {
            Debug.Log("Hear Something at" + soundPosition);

            Agent.SetDestination(soundPosition);

            Animator.SetBool("IsIdle", false);
            IsIdle = false;
            IdleTimer = 0f;
        }
    }
}
