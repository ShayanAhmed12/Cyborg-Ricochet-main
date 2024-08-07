using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private Trajectory _trajectory;
    private Audio _audioManager;
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
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
    }

    private void Update()
    {
        if (_trajectory.characterAim && Input.GetMouseButtonUp(0))
        {
            Debug.Log("Aim: "+_trajectory.characterAim);
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
            _audioManager.PlaySFX(_audioManager.BulletSound);
            Destroy(other.gameObject);
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

    public void SwordSound()
    {
        _audioManager.PlaySFX(_audioManager.SwordAttack);
    }
    
    
}
