using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour

{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public Animator animator;
    public float walkSpeed, chaseSpeed, idleTime, minIdleTime, maxIdleTime, sightDistance, minChaseTime, maxChaseTime, chaseTime, jumpscareTime, catchDistance;
    public bool walking, chasing;
    public Transform player;
    Transform currentDest;
    Vector3 dest;
    public Vector3 rayCastOffset;
    public float aiDistance;

    void Start()
    {
        walking = true;
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }

    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;
        aiDistance = Vector3.Distance(player.position, this.transform.position);
        if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                walking = false;
                StopCoroutine("StayIdle");
                StopCoroutine("ChaseRoutine");
                StartCoroutine("ChaseRoutine");
                chasing = true;
            }
        }

        if (chasing == true) 
        {
            dest = player.position;
            ai.destination = dest;
            ai.speed = chaseSpeed;
            //animator.ResetTrigger("Idle");
            //animator.ResetTrigger("Walk");
            //animator.SetTrigger("Sprint");
            if (aiDistance <= catchDistance)
            {
                //player.gameObject.SetActive(false);
                //animator.ResetTrigger("Idle");
                //animator.ResetTrigger("Walk");
                //animator.ResetTrigger("Sprint");
                //animator.SetTrigger("Jumpscare");
                StartCoroutine(DeathRoutine());
                chasing = false;
            }
        }

        if (walking == true)
        {
            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;
            //animator.ResetTrigger("Sprint");
            //animator.ResetTrigger("Idle");
            //animator.SetTrigger("Walk");
            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                //animator.ResetTrigger("Sprint");
                //animator.ResetTrigger("Walk");
                //animator.SetTrigger("Idle");
                ai.speed = 0;
                StopCoroutine("StayIdle");
                StartCoroutine("StayIdle");
                walking = false;
            }
        }
    }

    IEnumerator StayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }

    IEnumerator ChaseRoutine()
    {
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);
        StopChase();
    }

    public void StopChase()
    {
        walking = true;
        chasing = false;
        StopCoroutine(ChaseRoutine());
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene("DeathScene");
    }
}