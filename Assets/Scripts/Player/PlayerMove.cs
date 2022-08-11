using UnityEngine;

namespace Player
{
    public class PlayerMove : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private PlayerState _playerState;

        [SerializeField] private float speed;
    
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _playerState = PlayerState.Running;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            switch (_playerState)
            {
                case PlayerState.Running:
                    Vector3 temp = _rigidbody.velocity; //ignores gravity otherwise, not that THIS is particularly pretty
                    temp.z = speed;
                    _rigidbody.velocity = temp;
                    break;
                default:
                    break;
            }
        }
    }
}
