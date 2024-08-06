using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] GameObject GameOver;
   private int _health;

   private void Start()
   {
      _health = 100;
   }

   private void Update()
   {
      if (_health <= 0)
      {
         GameOver.SetActive(true);
         Debug.Log("Add Character dead animation here"); 
         // add level reset code here 
      }
   }
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Bullet"))
      {
         _health -= 10;
      }
   }
   private void OnCollisionEnter(Collision other)
   {
      if (other.gameObject.CompareTag("enemy"))
      {
         _health = 100;
      }
   }
}
