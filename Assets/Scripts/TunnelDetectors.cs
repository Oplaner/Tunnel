using UnityEngine;

public class TunnelDetectors : MonoBehaviour
{
    [SerializeField]
    private GameObject tunnelSegment;

    private bool didDetect = false;

    private GameObject player
    {
        get { return GameObject.FindGameObjectWithTag("Player"); }
    }

    private void Update()
    {
        if (!didDetect && player.transform.position.z >= transform.position.z)
        {
            if (gameObject.name == "Detector 1")
            {
                tunnelSegment.GetComponent<TunnelExtension>().generateNextSegment();
            }
            else if (gameObject.name == "Detector 2")
            {
                tunnelSegment.GetComponent<TunnelExtension>().destroyPreviousSegment();
            }

            player.GetComponent<PlayerMovement>().setBackWall(transform.position.z);
            didDetect = true;
        }
    }
}