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
    int randNum;
    public int destinationAmount;
    public Vector3 rayCastOffset;

    void Start()
    {
        walking = true;
        randNum = Random.Range(0, destinationAmount);
        currentDest = destinations[randNum];
    }

    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                walking = false;
                StopCoroutine("stayIdle");
                StopCoroutine("chaseRoutine");
                StartCoroutine("chaseRoutine");
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
            if (ai.remainingDistance <= catchDistance)
            {
                //player.gameObject.SetActive(false);
                //animator.ResetTrigger("Idle");
                //animator.ResetTrigger("Walk");
                //animator.ResetTrigger("Sprint");
                //animator.SetTrigger("Jumpscare");
                StartCoroutine(deathRoutine());
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
                StopCoroutine("stayIdle");
                StartCoroutine("stayIdle");
                walking = false;
            }
        }
    }

    IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        randNum = Random.Range(0, destinationAmount);
        currentDest = destinations[randNum];
    }

    IEnumerator chaseRoutine()
    {
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);
        walking = true;
        chasing = false;
        randNum = Random.Range(0, destinationAmount);
        currentDest = destinations[randNum];
    }

    IEnumerator deathRoutine()
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene("DeathScene");
    }
}