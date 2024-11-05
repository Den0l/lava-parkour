using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject WeaponPrefab;
    public GameObject mash;
    private void OnTriggerEnter(Collider other)
    { 
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.AddWeaponToInventory(WeaponPrefab, mash);
            gameObject.SetActive(false);
        }
  
    }
}
