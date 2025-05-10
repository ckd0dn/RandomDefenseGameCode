using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    public void Init(Vector3 position)
    {
        transform.position = position;
        transform.localScale = new Vector3(0.1f, 1f, 1f); 
        transform.gameObject.SetActive(true);
    }
}
