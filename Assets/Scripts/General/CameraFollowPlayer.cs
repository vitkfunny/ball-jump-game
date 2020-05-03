using System;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float speed = 0.25f;

    private PlayerProperties _playerProperties;

    private void Start()
    {
        _playerProperties = player.GetComponent<PlayerProperties>();
    }

    void LateUpdate()
    {
        var cameraTransform = transform;
        var cameraPosition = cameraTransform.position;

        var distance = new Vector3(0, player.position.y - cameraPosition.y);
        
        if (player.position.y > cameraTransform.position.y)
        {
            Vector3 newPosition = new Vector3(cameraPosition.x, player.position.y, cameraPosition.z);
            cameraTransform.position = Vector3.Lerp(cameraPosition, newPosition, speed);
        }
        else if (Math.Abs(distance.y) > Math.Abs(_playerProperties.loseDistance))
        {
            _playerProperties.lose = true;
        }
    }
}
