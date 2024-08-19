using UnityEngine;

public class WardrobeHiding : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public float loseDistance = 10f;
    private bool isInWardrobe = false;
    public Transform wardrobeDoor; 
    public float doorCloseAngle = 5f;
    public WardrobeDoor door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isInWardrobe = true;
            Debug.Log("Player is inside the wardrobe");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isInWardrobe = false;
            Debug.Log("Player is outside the wardrobe");
        }
    }

    void Update()
    {
        if (isInWardrobe)
        {
            float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.transform.position);

            if (distanceToPlayer > loseDistance && door.isClose == true)
            {
                Debug.Log("Enemy can't see the player");
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null && enemyAI.chasing)
                {
                    enemyAI.StopChase();
                }
            }
            else if (door.isClose == false)
            {
                Debug.Log("Wardrobe door is open! Enemy can still see the player");
            }
            else
            {
                Debug.Log("Enemy can still see the player");
            }
        }
    }
}
