using System.Collections;
using UnityEngine;

public class PlatformCollisions : MonoBehaviour
{
    public float bounceVelocity = 7.5f;

    private PlatformProperties _platformProperties;
    private bool canFadeOut = true;
    public float fadeOutTime = 1.5f;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _platformProperties = transform.GetComponent<PlatformProperties>();
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f)
        {
            var playerProperties = collision.collider.GetComponent<PlayerProperties>();

            if (playerProperties != null)
            {
                if (playerProperties.lose) return;
                
                if (_platformProperties.IsLatest() && playerProperties != null && !playerProperties.win)
                {
                    playerProperties.win = true;
                    return;
                }

                var colliderRb = collision.collider.GetComponent<Rigidbody2D>();
                var velocity = colliderRb.velocity;
                velocity.y = bounceVelocity;
                colliderRb.velocity = velocity;
                
                switch (_platformProperties.type)
                {
                    case 1: 
                        StartCoroutine(DestroyAfterTime(0));
                        break;
                    case 2:
                        if (canFadeOut)
                        {
                            canFadeOut = false;
                            StartCoroutine(FadeTo(0f, fadeOutTime));
                            StartCoroutine(DestroyAfterTime(fadeOutTime));
                        }
                        break;
                }
            }
        }
    }
    
    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(transform.gameObject);
    }
    
    private IEnumerator FadeTo(float newAlpha, float alphaTime)
    {
        var oldAlpha = _spriteRenderer.material.color.a;
        for (var t = 0.0f; t < alphaTime; t += Time.deltaTime)
        {
            var oldColor = _spriteRenderer.material.color;
            var newColor = new Color(oldColor.r, oldColor.g, oldColor.b, Mathf.Lerp(oldAlpha, newAlpha, t));
            _spriteRenderer.color = newColor;
            yield return null;
        }
    }
}
