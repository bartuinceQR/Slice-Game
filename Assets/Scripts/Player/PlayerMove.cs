using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerState _playerState;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerState = PlayerState.Waiting;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (_playerState)
        {
            case PlayerState.Running:
                _rigidbody.AddForce(new Vector3(0, 0, 2f), ForceMode.Impulse);
                break;
            default:
                break;
        }
    }
}
