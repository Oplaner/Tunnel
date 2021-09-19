using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;

    private void Update()
    {
        transform.position = targetTransform.position;
    }
}