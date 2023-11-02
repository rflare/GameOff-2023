using UnityEngine;
using UnityEngine.XR;

public class PlayerContext : MonoBehaviour
{
    Rigidbody2D _rb;


    [SerializeField] float _speed;

    [SerializeField] float _jumpHeight;
    [SerializeField] Vector2 _gcOffset;
    [SerializeField] Vector2 _gcSize;

    [SerializeField] float _dashDistance;
    [SerializeField] float _dashDelay;




    [SerializeField] float _direction;
    [SerializeField] bool _isGrounded;

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position + (Vector3)_gcOffset, (Vector3)_gcSize);
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleGroundCheck();
    }
    
    void HandleMovement()
    {
        _direction = Input.GetAxis("Horizontal");


        _rb.position += Vector2.right * _direction * _speed * Time.deltaTime;

        if(Mathf.Abs(_direction) > 0.2f)
            transform.localScale = new Vector3(Mathf.Sign(_direction), 1f, 1f);
    }

    void HandleJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.AddForce(Vector2.up * Mathf.Sqrt(2 * 9.81f * _jumpHeight), ForceMode2D.Impulse);

        }
    }

    void HandleGroundCheck()
    {
        _isGrounded = false;

        RaycastHit2D hit = Physics2D.BoxCast(
            (Vector2)transform.position + _gcOffset,
            _gcSize,
            0f,
            Vector2.up,
            Mathf.Infinity,
            1
        );

        _isGrounded = hit.transform != null;

    }

    void HandleDash()
    {
        
    }

}