using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int HP = 1;
    public float speed = 1F;
    public int damage = 1;
    public float damageDelay = 0.1F;
    public float attackDistance = 1F;
    public float pauseDelaySec = 1F;
    public AudioSource biteSound;

    private TopDownCharacterController _player;
    [SerializeField]
    private AnimationCurve attackShift = AnimationCurve.Linear(0, 0, 1, 1);
    private AState _state;

    abstract class AState
    {
        protected Monster go;
        public AState(Monster go)
        {
            this.go = go;
        }
        public abstract void Update();
    }
    class MoveState : AState
    {
        public MoveState(Monster go) : base(go) { }

        public override void Update()
        {
            go.transform.position = Vector3.MoveTowards(go.transform.position, go._player.transform.position, go.speed * Time.deltaTime);

            if((go._player.transform.position - go.transform.position).magnitude < go.attackDistance)
            {
                go._state = new AttackState(go);
            }
        }
    }
    class AttackState : AState
    {
        float _attackEnd = 0F;
        Vector3 startPos;
        bool _isPlayerDamaged = false;
        public AttackState(Monster go) : base(go)
        {
            var end = go.attackShift.keys[go.attackShift.keys.Length - 1].time;
            _attackEnd = Time.time + end;
            startPos = go.transform.position;
            if(go.biteSound)
            {
                go.biteSound.Play();
            }
        }

        public override void Update()
        {
            //var direction = (go._player.transform.position - go.transform.position);
            var direction = go._player.transform.position - go.transform.position;
            float lastKeyTime = go.attackShift.keys[go.attackShift.keys.Length - 1].time;
            go.transform.position = startPos + direction * go.attackShift.Evaluate(lastKeyTime - (_attackEnd - Time.time));

            if (!_isPlayerDamaged)
            {
                go._player.Damage(go);
                _isPlayerDamaged = true;
            }

            if(Time.time > _attackEnd)
            {
                go._state = new PauseState(go);
            }
        }
    }
    class PauseState : AState
    {
        private float _pauseEnd = 0F;
        public PauseState(Monster go) : base(go)
        {
            _pauseEnd = Time.time + go.pauseDelaySec;
        }
        public override void Update()
        {
            if(Time.time > _pauseEnd)
            {
                go._state = new MoveState(go);
            }
        }
    }

    void Start()
    {
        _player = FindObjectOfType<TopDownCharacterController>();
        _state = new MoveState(this);
    }

    // Update is called once per frame
    void Update()
    {
        _state.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BULLET 2 " + collision.gameObject.name);
        var bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet)
        {
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("BULLET " + collision.gameObject.name);
        var bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet)
        {
            HP -= bullet.damage;
            if(HP <= 0)
            {
                Die();
            }
            Destroy(bullet);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
