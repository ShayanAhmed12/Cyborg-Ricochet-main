using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private Trajectory _trajectory;
    Animator _animator;
    private Animator enemyAnimator;
    public GameObject _gameObject;
    public bool IndirectAttack;
    private Rigidbody _rb;

    private void Start()
    {
        _trajectory = GetComponentInChildren<Trajectory>();
        _animator = GetComponent<Animator>();
        enemyAnimator = _gameObject.GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_trajectory.characterAim && Input.GetMouseButtonUp(0))
        {
            IndirectAttack = true;
            AttackAndAnimate();
        }
    }

    private void AttackAndAnimate()
    {
            _animator.SetTrigger("attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            
            Destroy(other.gameObject);
            Debug.Log("Add Bullet explosion effect here");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            _rb.velocity = Vector3.zero;
        }
    }

    public void EnemyAnimation()
    {
        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("Death",true);
            Destroy(_gameObject,0.75f);
            Debug.Log("Enemy Dead!");
            IndirectAttack = false;
        }
    }
    
}
