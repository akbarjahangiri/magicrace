
using UnityEngine;

public class RoadTriggerManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, _player.transform.position.z - 20);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("car"))
        {
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("star"))
        {
            other.gameObject.SetActive(false);
        }
    }
}