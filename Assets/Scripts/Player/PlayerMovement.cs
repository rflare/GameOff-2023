using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Depenceies
    Rigidbody2D _rigidbody;

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
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        UpdateCount();
        if(_downDashCount > 0.5f)
            return;
        //It'll just do all these
        HandleMovement();
        HandleJump();
        HandleGroundCheck();
        HandleDash();
        HandleDownDash();
    }
    
    //Walking movement
    void HandleMovement()
    {
        _direction = Input.GetAxis("Horizontal");


        _rigidbody.position += Vector2.right * _direction * _speed * Time.deltaTime;

        if(Mathf.Abs(_direction) > 0.2f)
            transform.localScale = new Vector3(Mathf.Sign(_direction), 1f, 1f);
    }
    //Jumping and stuff
    void HandleJump()
    {
        if(!Input.GetKeyDown(KeyCode.Space))
            return;

        if(!_isGrounded)
            return;

        _rigidbody.AddForce(Vector2.up * Mathf.Sqrt(2 * 9.81f * _jumpHeight), ForceMode2D.Impulse);
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
        if(!Input.GetKeyDown(KeyCode.E))
            return;

        if(_dashCount > 0)
            return;
        
        _rigidbody.velocity = new Vector2(0, 1.5f);
        float mul = _isGrounded ? 1.5f : 1;
        _rigidbody.AddForce(Vector2.right * _dashDistance * Mathf.Sign(transform.localScale.x) * mul, ForceMode2D.Impulse);

        _dashCount = 1;
        
    }

    void HandleDownDash()
    {
        if(!Input.GetKeyDown(KeyCode.S))
            return;

        if(_downDashCount > 0)
            return;
        
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(Vector2.down * _downDashForce, ForceMode2D.Impulse);

        _downDashCount = 1;
        
    }

    void UpdateCount()
    {
        UpdateDash();

        UpdateDownDash();
    }

    void UpdateDash()
    {
        if(_dashCount <= 0)
        {
            _dashCount = 0;
            return;
        }
        _dashCount -= Time.deltaTime / _dashDelay;

    }

    void UpdateDownDash()
    {
        if(_downDashCount <= 0)
        {
            _downDashCount = 0;
            return;
        }
        
        _downDashCount -= Time.deltaTime / _downDashDelay;
        
    }
}