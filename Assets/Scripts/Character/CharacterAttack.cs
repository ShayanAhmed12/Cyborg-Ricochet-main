using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private Trajectory _trajectory;
    Animator _animator;

    private void Start()
    {
        _trajectory = GetComponentInChildren<Trajectory>();
        _animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        if (_trajectory.characterAim && Input.GetMouseButtonUp(0))
        {
            AttackAndAnimate();
        }
    }

    private void AttackAndAnimate()
    {
        if(Input.GetMouseButtonUp(0))
        {
            _animator.SetBool("Attack",true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            
            Destroy(other.gameObject);
            Debug.Log("Add Bullet explosion effect here");
        }
    }
}
