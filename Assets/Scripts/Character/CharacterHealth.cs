using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    public void LaserDamage()
    {
        _health -= 100f * Time.deltaTime;
        HealthBar.SetHealth(_health);
    }

    public void BladeDamage()
    {
        _health -= 2f;
        HealthBar.SetHealth(_health);
    }

}
