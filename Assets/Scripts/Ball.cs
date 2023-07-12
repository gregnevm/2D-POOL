using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _forcePower;
    [SerializeField] private bool _isProtectedFromDestroy;
    private Rigidbody rb;
    private Vector3 _startPosition;

    public bool OnMoving { get; private set; }

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _startPosition = transform.position;
    }   
    void Update()
    {
        // AddStandardForceInRandomDirectionFromInputButton(); //debug feature
        OnMoving = CheckSpeed();        
    }
    private void Init()
    {
        GameController.AddBall(this);
    }

    private bool CheckSpeed()
    {
        if (rb.velocity == Vector3.zero)
        {
            return false;
        }
        else
        {
            GameController.OnMoveStarted();
            CheckYPosition();
            return true;
        }
    }
    private void CheckYPosition()
    {
        if (this.gameObject.transform.position.y < -1f)
        {           
            if(_isProtectedFromDestroy)                
            {
                rb.isKinematic = true;
                transform.position = _startPosition;
                rb.isKinematic = false;
                return;
            }
            else { Destroy(); }            
        }
    }

    public void AddDirectionForce(Vector3 direction)
    {
        rb.AddForce(new Vector3(direction.x, 0f, direction.z) * _forcePower , ForceMode.Force);        
    }    

    public void Destroy()
    {
        if (!_isProtectedFromDestroy)
        {
            GameController.RemoveBall(this);
            Destroy(this.gameObject, 1f);
        }
    }

    // Debug methods
    void InputButtonAddStandardForceInRandomDirection()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Vector3 randomDirection = GetRandomDirection();
            rb.AddForce(randomDirection * _forcePower, ForceMode.Force);
        }
    }
    Vector3 GetRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);        
        float randomZ = Random.Range(-1f, 1f);
        return new Vector3(randomX, 0f, randomZ);
    }

}