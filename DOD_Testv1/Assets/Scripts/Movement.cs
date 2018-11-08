using UnityEngine;

namespace Dod
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour, IMovement
    {
        bool facingRight = true;
        [SerializeField]
        private float _accelerationTime = 2.0f;

        [SerializeField]
        private float _decelerationTime = 1.0f;

        [SerializeField]
        private float _maxSpeed = 3.0f;

        private float AccelerationPerTick
        {
            get { return _maxSpeed / _accelerationTime * Time.deltaTime; }
        }

        private float DecelerationPerTick
        {
            get { return _maxSpeed / _decelerationTime * Time.deltaTime; }
        }

        private float _currentSpeed;
        private Rigidbody2D _rigidBody;
        private Vector2 _direction;
        private Vector2 _pastDirection;
        bool Wall = false;

        private void Awake()
        {
            _rigidBody = GetComponentInChildren<Rigidbody2D>();
        }

        public void Move(Vector2 direction)
        {
            _direction = direction;
        }

        private void FixedUpdate()
        {
            if (_direction != Vector2.zero && !Wall)
            {
                _currentSpeed = Mathf.Min(_maxSpeed, _currentSpeed + AccelerationPerTick);
                _rigidBody.velocity = new Vector3(_direction.x, _direction.y, 0.0f ) * _currentSpeed;

                _pastDirection = _direction;
            }
            else if (!Wall)
            {
                _currentSpeed = Mathf.Max(0.0f, _currentSpeed - DecelerationPerTick);

                if (_currentSpeed <= 0.0f)
                {
                    _rigidBody.velocity = Vector3.zero;
                }
                else
                {
                    _rigidBody.velocity = new Vector3(_direction.x, _direction.y, 0.0f) * _currentSpeed;
                }
            }

            if (_direction.x > 0 && !facingRight)
                Flip();
            else if (_direction.x < 0 && facingRight)
                Flip();

            _direction = Vector2.zero;
        }
        void Flip()
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Wall")
            {
                _currentSpeed = 0.0f;
                _rigidBody.velocity = Vector3.zero;
                Wall = true;
            }
        }

        




















    }
    
}