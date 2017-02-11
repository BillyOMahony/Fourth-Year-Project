using UnityEngine;
using System.Collections;

public class ColliderDamage : MonoBehaviour {

	void Damage(float damage)
    {
        transform.GetComponent<PlayerManager>().DamageTaken(damage);
    }
}