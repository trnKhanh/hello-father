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
    List <Transform> CheckPoints = new List <Transform>();

    private NavMeshAgent Agent;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateSpeed);
        while(enabled)
        {
            if (ShouldChase())
            {
                Agent.SetDestination(Target.position);
            }
            else
            {
                Agent.SetDestination(transform.position);
            }
            yield return Wait;
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
