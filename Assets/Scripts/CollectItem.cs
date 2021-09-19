using UnityEngine;

public class CollectItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameplayController gameplayController = GameObject.Find("Gameplay Controller").GetComponent<GameplayController>();
            gameplayController.score += gameplayController.itemCollectBonus;
            Destroy(gameObject);
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.playSound(playerMovement.coinCollect);
        }
    }
}