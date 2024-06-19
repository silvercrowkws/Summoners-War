using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonsterBase
{
    

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        
        // 보스 전용 룬도 따로 만들어야 함
    }
}
