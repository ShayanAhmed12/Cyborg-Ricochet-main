using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] GameObject GameOver;
   public float _health;
   public HealthBar HealthBar;

   private void Start()
   {
      Time.timeScale = 1f;
      _health = 100f;
      HealthBar.SetMaxHealth(_health);
   }

   private void Update()
   {
      if (_health <= 0)
        {
            Time.timeScale = 0f;
            GameOver.SetActive(true);
            
        }
    }
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Bullet"))
      {
         _health -= 10f;
         HealthBar.SetHealth(_health);
      }
   }
   private void OnCollisionEnter(Collision other)
   {
      if (other.gameObject.CompareTag("enemy"))
      {
         _health = 100f;
         HealthBar.SetMaxHealth(_health);
         Debug.Log("Healed!");
      }
   }
   public void LaserDamage()
   {
      _health -= 0.5f;
      HealthBar.SetHealth(_health);
   }
}
