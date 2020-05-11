using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct PlatformVariant
{
    public GameObject platformPrefab;
    public int probability;
}

public class LevelGenerator : MonoBehaviour
{
    public List<PlatformVariant> platformList;
    
    public float startX;
    public float startY;
    public float screenToMaxYOffset = 7.5f;
    public float minToMaxYScale = 2f;
    private float _levelWidth;
    private float _minOffsetY = .2f;
    private float _maxOffsetY = 3f;
    public int maxPlatforms;
    public float movingProbabilityPct = 10f;

    private int[] _platformPrefabProbability;
    
    void Start()
    {
        var platformPrefabProbabilityList = new List<int>();
        for (int platformNumber = 0; platformNumber < platformList.Count; platformNumber++)
        {
            for (int i = 0; i < platformList[platformNumber].probability; i++)
            {
                platformPrefabProbabilityList.Add(platformNumber);
            }
        }

        _platformPrefabProbability = platformPrefabProbabilityList.ToArray();
        
        var gameCamera = Camera.main;
        var gameCameraHeight = 2f * gameCamera.orthographicSize;
        var gameCameraWidth = gameCameraHeight * gameCamera.aspect;

        _maxOffsetY = gameCameraHeight / screenToMaxYOffset;
        _minOffsetY = _maxOffsetY / minToMaxYScale;
        _levelWidth = gameCameraWidth * 0.85f / 2f;
        
        GameObject newPlatform = null;
        var newPlatformPosition = new Vector3(startX, startY);
        
        for (int i = 0; i < maxPlatforms; i++)
        {
            var platformPrefabId = _platformPrefabProbability[Random.Range(0, _platformPrefabProbability.Length)];
            var platformPrefab = platformList[platformPrefabId].platformPrefab;
            newPlatform = Instantiate(platformPrefab, newPlatformPosition, Quaternion.identity);
            if (Random.Range(0, 100) < movingProbabilityPct)
            {
                newPlatform.GetComponent<PlatformProperties>().moving = true;
            }

            var offsetX = Random.Range(-_levelWidth, _levelWidth);
            var offsetY = Random.Range(_minOffsetY, _maxOffsetY);
            newPlatformPosition.x = offsetX;
            newPlatformPosition.y += offsetY;
        }

        if (newPlatform != null) newPlatform.GetComponent<PlatformProperties>().SetLatest(true);
    }
}
