using UnityEngine;

public class RoadTrigger : MonoBehaviour
{
    private ObjectSpawner _objectSpawner;

    private void OnEnable()
    {
        _objectSpawner = FindObjectOfType<ObjectSpawner>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            _objectSpawner.SpawnGround();
            gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}