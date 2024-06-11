using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMonster : MonsterBase
{


#if UNITY_EDITOR

    public void Test_LightMonster_Attack()
    {
        Debug.Log($"LightMonster가 Attack 상태로 전환");
        State = MonsterState.Attack;
    }

#endif
}
