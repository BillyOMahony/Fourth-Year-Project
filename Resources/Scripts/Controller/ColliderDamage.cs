using UnityEngine;
using System.Collections;

public class ColliderDamage : MonoBehaviour {

	void Damage(float damage)
    {
        transform.parent.GetComponent<PlayerManager>().DamageTaken(damage);
    }
}