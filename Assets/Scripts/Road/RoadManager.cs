using UnityEngine;

public class RoadManager : MonoBehaviour
{
    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;
    }
}