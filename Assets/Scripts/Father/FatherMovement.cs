using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;


public class FatherMovement : MonoBehaviour
{
    public Transform Target;
    public float UpdateSpeed = 0.1f;
    public AudioSource SoundSource;
    public List<Transform> Checkpoints = new List<Transform>();
    public float ViewAngle = 45f;
    public float ChaseDistance = 10f;
    public float chaseSpeed;
    public float walkSpeed;

    private int CurrentCheckpointIndex = 0;
    private float CatchedThreshold = 5f;
    private float IdleTimeAtCheckpoint;
    private NavMeshAgent Agent;
    private Animator Animator;

    private bool IsIdle = false;
    private bool IsChasing = false;
    private float IdleTimer = 0f;

    string k_catch = "Catch";

    bool hasCatched = false;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Debug.Log(k_catch);
        float distanceToTarget = Vector3.Distance(Target.position, transform.position);

        if (distanceToTarget <= CatchedThreshold && hasCatched == false)
        {
            hasCatched = true;
            Animator.SetTrigger(k_catch);
            Agent.ResetPath();
            Wait(2);
            return;
        }
        else
        {
            if (ShouldChase())
            {
                Agent.speed = chaseSpeed;
                Debug.Log("COMMING............");
                Agent.SetDestination(Target.position);
                IsIdle = false;
                IsChasing = true;
                Animator.SetBool("IsIdle", IsIdle);
                Animator.SetBool("IsChasing", IsChasing);
            }
            else
            {
                Agent.speed = walkSpeed;
                PatrolRoute();
            }
        }
    }

    private void Wait(float waitTime = -1)
    {
        if (waitTime < 0)
            waitTime = Random.Range(1, 5);
        Agent.SetDestination(transform.position);

        IsIdle = true;
        IdleTimeAtCheckpoint = waitTime;
    }
  
    private bool IsReachedDestination()
    {
        if(!Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance)
        {
            if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
    }

    private void PatrolRoute()
    {
        if (Checkpoints.Count == 0) return;
        if (IsReachedDestination() && IsIdle == false)
        {
            IsIdle = true;
            Wait(3);
            Agent.ResetPath();
            Animator.SetBool("IsChasing", false);
        }

        Animator.SetBool("IsIdle", IsIdle);
        if (IsIdle)
        {
            IdleTimer += Time.deltaTime;
            if (IdleTimer >= IdleTimeAtCheckpoint)
            {
                IdleTimer = 0;
                MoveToNextCheckpoint();
            }
        }
    }

    private void MoveToNextCheckpoint()
    {
        if (Checkpoints.Count == 0) return;

        CurrentCheckpointIndex = (CurrentCheckpointIndex + 1) % Checkpoints.Count;
        Transform currentCheckpoint = Checkpoints[CurrentCheckpointIndex];
        Agent.SetDestination(currentCheckpoint.position);
        IsIdle = false;
        IsChasing = false;
        Animator.SetBool("IsIdle", IsIdle);
        Animator.SetBool("IsChasing", IsChasing);
    }

    private bool ShouldChase()
    {
        if (hasCatched)
            return false;

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

    public void Hear(Vector3 soundPosition, float soundRadius)
    {
        float distanceToSound = Vector3.Distance(transform.position, soundPosition);

        if (distanceToSound <= soundRadius)
        {
            Debug.Log("Hear Something at" + soundPosition);
            Agent.SetDestination(soundPosition);
            IsIdle = false;
            Animator.SetBool("IsIdle", IsIdle);
            IdleTimer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Father:OnTriggerEnter");
        if (other.gameObject.TryGetComponent<Door>(out Door door))
        {
            Debug.Log("Father:open");
            door.Open();
        }
    }
}
