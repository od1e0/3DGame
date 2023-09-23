using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ITakingDamage
{
    void TakingDamage(int damage, Transform sorceDamageTransform);

    void TakingBombDamage(int damage);

}
