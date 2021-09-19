using System.Collections.Generic;
using UnityEngine;

public class GetSegment : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> segments;

    public GameObject getSegment()
    {
        return Instantiate(segments[Random.Range(0, segments.Count)]);
    }
}