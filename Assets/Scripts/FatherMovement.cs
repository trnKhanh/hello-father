using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FatherMovement : MonoBehaviour
{
    public Transform Target;
    public float UpdateSpeed = 0.1f;
    public float ChaseDistance = 10f;
    public AudioSource SoundSource;
    public List<Transform> Checkpoints = new List<Transform>();

    private int CurrentCheckpointIndex = 0;
    private float ReachCheckpointThreshold = 2f;
    private NavMeshAgent Agent;
    private Animator Animator;

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
            if (distance <= ReachCheckpointThreshold)
            {
                Animator.SetInteger("State", 0); // Chased
            }
            else
            {
                Animator.SetInteger("State", 1); // Walking
            }
            if (ShouldChase())
            {
                Agent.SetDestination(Target.position);
            }
            else
            {
                Transform currentCheckpoint = Checkpoints[CurrentCheckpointIndex];
                Agent.SetDestination(currentCheckpoint.position);

                float distanceToCheckpoint = Vector3.Distance(transform.position, currentCheckpoint.position);
                if (distanceToCheckpoint <= ReachCheckpointThreshold)
                {
                    CurrentCheckpointIndex = (CurrentCheckpointIndex + 1) % Checkpoints.Count;
                }
            }

            yield return wait;
        }
    }

    private bool ShouldChase()
    {
        float distanceToTarget = Vector3.Distance(transform.position, Target.position);
        if (distanceToTarget <= ChaseDistance)
        {
            return true;
        }

        if (SoundSource != null && SoundSource.isPlaying)
        {
            return true;
        }

        return false;
    }
}
