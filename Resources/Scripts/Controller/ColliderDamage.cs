using UnityEngine;
using System.Collections;

public class ColliderDamage : MonoBehaviour {

	void Damage(DamageInfo damageInfo)
    {
        float damage = damageInfo.getDamage();
        string hitBy = damageInfo.getHitBy();
        transform.GetComponent<PlayerManager>().DamageTaken(damage, hitBy);
        Debug.Log("Calling DamageTaken(" + damage + ", " + hitBy + ")");
    }
}