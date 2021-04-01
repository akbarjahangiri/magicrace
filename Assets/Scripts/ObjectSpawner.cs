using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private float GroundSpawnDistance = 96f;
    private ObjectPooler _objectPooler;
    private GameManager _gameManager;
    
    [SerializeField] private GameObject _player;
    
    private void OnEnable()
    {
        _objectPooler = FindObjectOfType<ObjectPooler>();
        _gameManager = FindObjectOfType<GameManager>();
    }
    
    public void InitialSpawnRoad()
    {
        for (int i = 0; i < 2; i++)
        {
            _objectPooler.SpawnFromPool("ground", new Vector3(0, 0, GroundSpawnDistance * i),
                Quaternion.identity);
        }
    }

    public void SpawnGround()
    {
        GroundSpawnDistance = _objectPooler._lastPoolObject == null
            ? GroundSpawnDistance
            : (_objectPooler._lastPoolObject.transform.position.z + 96f);
        _objectPooler.SpawnFromPool("ground", new Vector3(0, 0, GroundSpawnDistance), Quaternion.identity);
    }

    public void SpawnStar()
    {
        int xPositionRandom = UnityEngine.Random.Range(0, _gameManager._starXPosition.Length);
        _objectPooler.SpawnFromPool("star",
            new Vector3(_gameManager._starXPosition[xPositionRandom], _gameManager._starYPosition,
                _player.transform.position.z + 10f), Quaternion.Euler(90,45,80));
    }

    public void SpawnCar()
    {
        int xPositionRandom = UnityEngine.Random.Range(0, _gameManager._carXPosition.Length);
        _objectPooler.SpawnFromPool("car",
            new Vector3(_gameManager._carXPosition[xPositionRandom], _gameManager._carYPosition,
                _player.transform.position.z + 11f), Quaternion.identity);
    }
}