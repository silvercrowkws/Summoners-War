using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMonster : MonsterBase
{


#if UNITY_EDITOR

    public void Test_WaterMonster_Attack()
    {
        Debug.Log($"WaterMonster가 Attack 상태로 전환");
        State = MonsterState.Attack;
    }

#endif
}
