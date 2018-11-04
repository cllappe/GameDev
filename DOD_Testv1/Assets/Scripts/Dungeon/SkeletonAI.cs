/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{

    Transform currentPoint;
    int currentPointIndex;
    bool facingRight = true;
    public float Speed;


    public Transform[] moveSpot;

    // Use this for initialization
    void Start()
    {
        currentPointIndex = 0;
        currentPoint = moveSpot[currentPointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        /*transform.Translate(Vector3.up * Time.deltaTime * Speed);
        if(Vector3.Distance(transform.position,currentPoint.position)<.1f)
        {
            if(currentPointIndex+1<patrolPoints.Length)
            {
                currentPointIndex++;
            }
            else
            {
                currentPointIndex = 0;
            }
            currentPoint = patrolPoints[currentPointIndex];
        }

        Vector3 patrolPointDirection = currentPoint.position - transform.position;
        float angle = Mathf.Atan2(patrolPointDirection.y, patrolPointDirection.x) * Mathf.Rad2Deg - 90f;

        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 180f);--

        transform.position = Vector2.MoveTowards(transform.position, moveSpot[currentPointIndex].position, Speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpot[currentPointIndex].position) < .2f)
        {
            if (currentPointIndex + 1 < moveSpot.Length)
            {
                currentPointIndex++;
                if (currentPointIndex <= 2 && !facingRight)
                    Flip();
                else if (currentPointIndex >= 3 && facingRight)
                {
                    Flip();
                }
            }
            else
            {
                currentPointIndex = 0;
            }
            currentPoint = moveSpot[currentPointIndex];
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}*/
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Dod
{
    [RequireComponent(typeof(IMovement))]
    public class SkeletonAI : MonoBehaviour
    {
        [SerializeField]
        private float _trackingDistance = 5.0f;

        private Vector2 DirectionToPlayer
        {
            get
            {
                var directionToPlayer = _playerTransform.position - transform.position;
                return new Vector2(directionToPlayer.x, directionToPlayer.y).normalized;
            }
        }

        private Transform _playerTransform;
        private IMovement _movement;
        private bool _isTrackingPlayer;

        private void Awake()
        {
            //DontDestroyOnLoad(this);
          
            _movement = GetComponentInChildren<IMovement>();
            _playerTransform = FindObjectOfType<PlayerController>().transform;

        }

        private void Update()
        {
            _isTrackingPlayer = Vector3.Distance(transform.position, _playerTransform.position) <= _trackingDistance;
        }

        private void FixedUpdate()
        {
            _movement.Move(_isTrackingPlayer ? DirectionToPlayer : Vector2.zero);
        }

        
    }
}
