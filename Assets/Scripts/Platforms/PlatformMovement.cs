using UnityEngine;

namespace Platforms
{
    public class PlatformMovement : MonoBehaviour
    {
        public float _speed = 0f;
        private int _dir = 1;
    
        private Vector3 moveDir = Vector3.zero;

        private void Start()
        {
            ResetMoveVector();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.position += moveDir * Time.fixedDeltaTime;
        }

        void ResetMoveVector()
        {
            moveDir = new Vector3(_dir * _speed, 0, 0);
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
            ResetMoveVector();
        }

        public void SetDir(int dir)
        {
            _dir = dir;
            ResetMoveVector();
        }
    }
}
