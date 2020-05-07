using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _body;

    private float _movement;

    public float movementSpeed = 1;
    
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float movementDirection;
        
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            
            if (touch.position.x < Screen.width / 2)
            {
                movementDirection = -1 * 0.5f;
            }
            else
            {
                movementDirection = 0.5f;
            }
        }
        else
        {
            movementDirection = Input.GetAxis("Horizontal");
        }
        _movement = movementDirection * movementSpeed;
    }

    void FixedUpdate()
    {
        var velocity = _body.velocity;
        velocity.x = _movement;
        _body.velocity = velocity;
    }
}
