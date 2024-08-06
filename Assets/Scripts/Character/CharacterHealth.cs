using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] GameObject GameOver;
   private int _health;
   public HealthBar HealthBar;

   private void Start()
   {
      _health = 100;
      HealthBar.SetMaxHealth(_health);
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
         HealthBar.SetHealth(_health);
      }
   }
   private void OnCollisionEnter(Collision other)
   {
      if (other.gameObject.CompareTag("enemy"))
      {
         _health = 100;
         HealthBar.SetMaxHealth(_health);
         Debug.Log("Healed!");
      }
   }
}
