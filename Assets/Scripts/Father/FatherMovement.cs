using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;


public class FatherMovement : MonoBehaviour, IGameData
{
    [Serializable]
    public class FatherData
    {
        public Vector3 position;
        public int checkpointId;
        public Vector3 destination;
        public bool isIdle;
        public bool isChasing;
    }

    [Header("Target")]
    public Transform player;
    public float viewRange = 10f;
    public float viewAngle = 45f;
    public float chaseDistance = 10f;

    [Header("Patrol")]
    public List<Transform> Checkpoints = new List<Transform>();
    public int idleTimeMin;
    public int idleTimeMax;

    [Header("Movement")]
    public float walkSpeed;
    public float chaseSpeed;

    [Header("References")]
    public Transform fatherEye;

    int currentCheckpointId = 0;

    NavMeshAgent agent;
    Animator animator;
    AudioSource audioSource;

    bool isIdle = false;
    bool isChasing = false;
    bool hasCatched = false;
    private float idleTimer = 0f;

    string k_catch = "Catch";
    string k_chasing = "Chasing";
    string k_idle = "Idle";


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        PatrolRoute();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (SeeTarget())
        {
            Debug.Log("See target");
            ChaseTo(player.position);
        }
    }


    private void UpdateAnimation()
    {
        animator.SetBool(k_chasing, isChasing);
        animator.SetBool(k_idle, isIdle);
    }

    private bool SeeTarget()
    {
        if (hasCatched)
            return false;

        Vector3 directionToTarget = (player.position - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        float distanceToTarget = (transform.position - player.position).magnitude;

        if (angleToTarget <= viewAngle / 2 && distanceToTarget <= viewRange)
        {
            if (Physics.Raycast(fatherEye.position, directionToTarget, out RaycastHit hit))
            {
                if (hit.collider.gameObject == player.gameObject)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void Wait(float waitTime = -1)
    {
        if (waitTime < 0)
            waitTime = UnityEngine.Random.Range(idleTimeMin, idleTimeMax);

        agent.SetDestination(transform.position);
        isIdle = true;
        isChasing = false;
        idleTimer = waitTime;
    }

    void ChaseTo(Vector3 target)
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(target);

        isIdle = false;
        isChasing = true;
    }

    void WalkTo(Vector3 target)
    {
        agent.speed = walkSpeed;
        agent.SetDestination(target);

        isIdle = false;
        isChasing = false;
    }

    void Catch()
    {
        if (hasCatched)
            return;

        hasCatched = true;
        animator.SetTrigger(k_catch);

        player.gameObject.GetComponent<IDamagable>().Hit();
    }

    private bool ReachedDestination()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
    }

    private void PatrolRoute()
    {
        if (Checkpoints.Count == 0) return;

        if (ReachedDestination())
        {
            Debug.Log(isIdle);
            if (!isIdle)
                Wait();
            Debug.Log(isIdle);

        }

        if (!isIdle) return;

        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0)
        {
            idleTimer = 0;
            MoveToNextCheckpoint();
        }
        Debug.Log(isIdle);
    }

    private void MoveToNextCheckpoint()
    {
        if (Checkpoints.Count == 0) return;

        currentCheckpointId = (currentCheckpointId + 1) % Checkpoints.Count;
        Transform currentCheckpoint = Checkpoints[currentCheckpointId];

        WalkTo(currentCheckpoint.position);
    }

    

    public void Hear(Vector3 soundPosition, float soundRadius)
    {
        float distanceToSound = Vector3.Distance(transform.position, soundPosition);

        if (distanceToSound <= soundRadius && !hasCatched)
        {
            Debug.Log("I hear you");
            ChaseTo(soundPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            Catch();
        }

        if (other.gameObject.TryGetComponent<Door>(out Door door))
        {
            door.Open();
        }
    }

    public void Save(string root)
    {
        string savePath = Path.Join(root, "father.json");
        FatherData fatherData = new FatherData();
        
        fatherData.position = transform.position;
        fatherData.checkpointId = currentCheckpointId;
        fatherData.destination = agent.destination;
        fatherData.isIdle = isIdle;
        fatherData.isChasing = isChasing;

        Debug.Log(String.Format("Save father to {0}", savePath));
        File.WriteAllText(savePath, JsonUtility.ToJson(fatherData));
    }

    public void Load(string root)
    {
        try
        {
            string savePath = Path.Join(root, "father.json");
            FatherData fatherData = JsonUtility.FromJson<FatherData>(File.ReadAllText(savePath));

            transform.position = fatherData.position;
            currentCheckpointId = fatherData.checkpointId;

            isIdle = fatherData.isIdle;
            isChasing = fatherData.isChasing;

            if (isIdle)
            {
                Wait();
            } else if (hasCatched)
            {
                if (isChasing)
                    ChaseTo(fatherData.destination);
                else
                    WalkTo(fatherData.destination);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }
}
