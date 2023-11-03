using UnityEngine;
using UnityEngine.XR;

public class PlayerContext : MonoBehaviour
{
    //Depenceies
    Rigidbody2D _rb;

    //Parameters for stuff
    [SerializeField] float _speed;

    [SerializeField] float _jumpHeight; //This means how high player jumps
    //gc means ground check, not garbage collector
    [SerializeField] Vector2 _gcOffset;
    [SerializeField] Vector2 _gcSize;

    [SerializeField] float _dashDistance;
    [SerializeField] float _dashDelay;
    //Note: Dash Delay mush be a fractional value
    //since _dashCount is reset to one 


    //These are just same as above, but for down dash
    [SerializeField] float _downDashForce;
    [SerializeField] float _downDashDelay;



    //Don't think of modifying these, they just keep changing
    //Don't use serializefield, just use debug mode
    float _direction;
    bool _isGrounded;
    float _dashCount;
    //_dashCount MUST be less than one
    //it is always reset to one

    float _downDashCount;

    void OnDrawGizmosSelected()
    {
        //Draw the ground checker
        Gizmos.DrawCube(transform.position + (Vector3)_gcOffset, (Vector3)_gcSize);
    }

    void Awake()
    {
        //Setup dependencies
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //It'll just do all these
        HandleMovement();
        HandleJump();
        HandleGroundCheck();
        HandleDash();
        HandleDownDash();
        
        DelayDash();
    }
    
    //Walking movement
    void HandleMovement()
    {
        _direction = Input.GetAxis("Horizontal");


        _rb.position += Vector2.right * _direction * _speed * Time.deltaTime;

        if(Mathf.Abs(_direction) > 0.2f)
            transform.localScale = new Vector3(Mathf.Sign(_direction), 1f, 1f);
    }
    //Jumping and stuff
    void HandleJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.AddForce(Vector2.up * Mathf.Sqrt(2 * 9.81f * _jumpHeight), ForceMode2D.Impulse);

        }
    }
    //This will change whether grounded or not
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
    //This is responsible for making player dash
    void HandleDash()
    {
        if(Input.GetKeyDown(KeyCode.E) && _dashCount <= 0)
        {
            float mul = _isGrounded ? 1.5f : 1;
            _rb.AddForce(Vector2.right * _dashDistance * Mathf.Sign(transform.localScale.x) * mul, ForceMode2D.Impulse);
            //_rb.velocity = Vector2.right * _dashDistance * Mathf.Sign(transform.localScale.x) + Vector2.up * _rb.velocity.y;

            _dashCount = 1;
        }
    }

    void HandleDownDash()
    {
        if(Input.GetKeyDown(KeyCode.S) && _downDashCount <= 0)
        {
            _rb.velocity = Vector2.zero;
            _rb.AddForce(Vector2.down * _downDashForce, ForceMode2D.Impulse);
        }
    }

    void DelayDash()
    {
        //Code for normal dashing
        if(_dashCount <= 0)
        {
            _dashCount = 0;
        }
        else
        {
            _dashCount -= Time.deltaTime / _dashDelay;
        }

        //Code for down dashing

        if(_downDashCount <= 0)
        {
            _downDashCount = 0;
        }
        else
        {
            _downDashCount -= Time.deltaTime / _downDashDelay;
        }
    }

}