using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float movementRange = 1f;
    public float movementSpeed = .75f;

    private Rigidbody2D _body;
    private PlatformProperties _properties;
    private float _baseX;
    public bool moveRight = true;
    
    void Start()
    {
        _properties = transform.GetComponent<PlatformProperties>();
        _baseX = transform.position.x;
    }

    private void Update()
    {
        if (_properties.moving)
        {
            if (transform.position.x - _baseX > movementRange)
            {
                moveRight = false;
            }
            
            if (transform.position.x - _baseX < -movementRange)
            {
                moveRight = true;
            }

            float newPosition;
            if (moveRight)
            {
                newPosition = transform.position.x + movementSpeed * Time.deltaTime;
            }
            else
            {
                newPosition = transform.position.x - movementSpeed * Time.deltaTime;
            }
            transform.position = new Vector2(newPosition, transform.position.y);
        }
    }
}
