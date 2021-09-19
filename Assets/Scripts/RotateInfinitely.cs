using UnityEngine;

public class RotateInfinitely : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 50;

    [SerializeField]
    private float oscillationSpeed = 2;

    [SerializeField]
    private float oscillationRange = 0.1f;

    private Vector3 basePosition;

    private void Start()
    {
        basePosition = transform.position;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(90, rotationSpeed * Time.time, 0);
        transform.position = basePosition + oscillationRange * Mathf.Sin(oscillationSpeed * Time.time) * Vector3.up;
    }
}