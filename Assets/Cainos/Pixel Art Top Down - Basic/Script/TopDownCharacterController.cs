using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        private Animator animator;
        private Rigidbody2D rigid;

        private Vector2 direction;

        private void Start()
        {
            animator = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
        }

        public void SetDirection(Vector2 dir)
        {
            this.direction = dir;
        }


        private void Update()
        {
            //Vector2 dir = Vector2.zero;
            //if (Input.GetKey(KeyCode.A))
            //{
            //    dir.x = -1;
            //    animator.SetInteger("Direction", 3);
            //}
            //else if (Input.GetKey(KeyCode.D))
            //{
            //    dir.x = 1;
            //    animator.SetInteger("Direction", 2);
            //}

            //if (Input.GetKey(KeyCode.W))
            //{
            //    dir.y = 1;
            //    animator.SetInteger("Direction", 1);
            //}
            //else if (Input.GetKey(KeyCode.S))
            //{
            //    dir.y = -1;
            //    animator.SetInteger("Direction", 0);
            //}

            Vector2 dir = direction;
            //Debug.Log("DIRECTION" + direction.x + " " + direction.y);

            //dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);
            Debug.Log("MAGN " + direction.magnitude + " VELOCITY " + (speed * dir).magnitude);

            rigid.velocity = speed * direction;
        }
    }
}
