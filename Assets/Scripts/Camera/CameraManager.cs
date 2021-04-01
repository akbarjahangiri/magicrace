using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform _cameraTransform;
    private PlayerManager _playerManager;

    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _traget;
    [SerializeField] private float _smoothSpeed;
    [SerializeField] private Vector3 _offset;


    // Start is called before the first frame update
    private void OnEnable()
    {
        _playerManager = FindObjectOfType<PlayerManager>();
        PlayerManager.crashCar += ShakeCamera;
    }

    private void OnDisable()
    {
        PlayerManager.crashCar -= ShakeCamera;
    }

    private void LateUpdate()
    {
        Vector3 desiredPostion =
            new Vector3(transform.position.x, transform.position.y, _traget.transform.position.z + 2.6f);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPostion, _smoothSpeed);
        transform.position = smoothedPosition;
    }

    private void ShakeCamera()
    {
        transform.DOShakePosition(1f, new Vector3(0.5f, 0.5f, 0.5f));
    }
    
}