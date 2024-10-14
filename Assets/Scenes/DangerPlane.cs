using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class DangerPlane : MonoBehaviour
{
    public int DamagePointCount;
    private GameManager _GameManager;

    private void Start()
    {
        _GameManager = FindObjectOfType<GameManager>();
    }

    private IEnumerator LavaDamage()
    {
        while (true)
        {
            _GameManager.Damaging(DamagePointCount);
            yield return new WaitForSeconds(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(LavaDamage());
    }

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(LavaDamage());
    }
}
