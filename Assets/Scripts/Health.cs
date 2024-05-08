using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float healthAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            player.Health(healthAmount);
            Destroy(gameObject);
        }
    }
}
