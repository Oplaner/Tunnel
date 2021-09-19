using UnityEngine;
using UnityEngine.UI;

public class UpdateLabel : MonoBehaviour
{
    public void updateLabel(string text)
    {
        GetComponent<Text>().text = text;
    }
}