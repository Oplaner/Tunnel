using UnityEngine;

public class TunnelExtension : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject tunnelManager;

    [SerializeField]
    private GameObject previousSegment = null;

    [SerializeField]
    private bool didGenerateNextSegment = false;

    public void generateNextSegment()
    {
        if (!didGenerateNextSegment)
        {
            GameObject nextSegment = tunnelManager.GetComponent<GetSegment>().getSegment();
            TunnelExtension extension = nextSegment.GetComponent<TunnelExtension>();
            extension.player = player;
            extension.tunnelManager = tunnelManager;
            extension.previousSegment = gameObject;
            nextSegment.transform.position = transform.position + 10 * Vector3.forward;
            GameplayController gameplayController = GameObject.Find("Gameplay Controller").GetComponent<GameplayController>();
            gameplayController.score += gameplayController.obstaclePassBonus;
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.playSound(playerMovement.obstaclePass);
            didGenerateNextSegment = true;
        }
    }

    public void destroyPreviousSegment()
    {
        if (previousSegment != null)
        {
            Destroy(previousSegment);
            previousSegment = null;
        }
    }
}