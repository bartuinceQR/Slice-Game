using System;
using Gameplay;
using Items;
using UnityEngine;

namespace Player
{
    public class PlayerMove : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private PlayerState _playerState;

        private Transform _target;

        private float _targetX;

        [SerializeField] private float speed;
        [SerializeField] private Animator modelAnimator;
        [SerializeField] private Transform followTarget;
        [SerializeField] private AudioClip dieSound;
    
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

                    temp.x = Mathf.Abs(transform.position.x - _targetX) > Single.Epsilon
                        ? (_targetX - transform.position.x) * 10f
                        : 0;
                    
                    _rigidbody.velocity = temp;
                    break;
                case PlayerState.Finalising:
                    transform.LookAt(_target);
                    _rigidbody.velocity = transform.forward * speed;
                    break;
                case PlayerState.Dancing:
                    _rigidbody.velocity = Vector3.zero;
                    break;
                default:
                    break;
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Level End"))
            {
                other.GetComponent<DiamondBehaviour>().GetCollected();
            } else if (other.CompareTag("Finish"))
            {
                FinishArea areaTrigger = other.GetComponent<FinishArea>();
                areaTrigger.SetFinishState();
                SetTarget(areaTrigger.GetTarget());
            } else if (other.CompareTag("Respawn"))
            {
                Die();
            }
        }

        public void SetState(PlayerState state)
        {
            _playerState = state;
            OnStateEnter();
        }
        
        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void SetTargetX(float x)
        {
            _targetX = x;
        }

        void Die()
        {
            followTarget.SetParent(null);
            AudioManager.Instance.PlaySound(dieSound);
            GameplayManager.Instance.EndGame();
        }

        //oh boohoo, let me do a sad dance for you on the world's smallest State Machine
        void OnStateEnter()
        {
            switch (_playerState)
            {
                case PlayerState.Running:
                    modelAnimator.Play("Run");
                    break;
                case PlayerState.Dancing:
                    modelAnimator.Play("dance");
                    break;
                default:
                    break;
            }
        }
        
    }
}
