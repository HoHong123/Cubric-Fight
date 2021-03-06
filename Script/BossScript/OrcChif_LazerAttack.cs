﻿using UnityEngine;
using System.Collections.Generic;

/*
    근접 범위 밖에 존재하는 캐릭터를 무작위로 선택하여 공격
    
    !. 쿨타임이 되면 근접 범위 밖에 존재하는 캐릭터를 확인하여 공격 실행
    !. 가로 공격 : 선택된 캐릭터를 기준으로 맵을 가로로 가로지르는 범위 공격 실시
    !. 세로 공격 : 선택된 캐릭터를 기준으로 맵을 세로로 가로지르는 범위 공격 
*/

public class OrcChif_LazerAttack : State<OrcChif_AI>
{
    private static OrcChif_LazerAttack _instance; // 해당 클래스 객체
    private OrcChif_LazerAttack()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    } // 생성자
    public static OrcChif_LazerAttack Instance {
        get { // Instance변수를 호출시
            if (_instance == null)
            {
                new OrcChif_LazerAttack(); // 해당 클래스의 객체가 비어있으면 생성자를 통해 생성
            }

            return _instance; // 해당 클래스 객체 반환
        }
    }

    private List<Transform> m_AttackPosition;

    private float m_LazerCoolDown;
    private int m_RandomSelect;


    public override void EnterState(OrcChif_AI owner)
    {
        m_LazerCoolDown = Random.Range(owner.m_LazerCoolTime, owner.m_LazerCoolTime + 2);
        m_AttackPosition = new List<Transform>();
    }

    /// 쿨타임이 다되면 플레이어 캐릭터 중 하나 이상 근접 범위 밖이라면
    /// 해당 캐릭터들 중 랜덤으로 하나의 캐릭터에 원거리 공격 실시
    public override void UpdateState(OrcChif_AI owner)
    {
        if (m_LazerCoolDown < 0)
        {
            m_LazerCoolDown = Random.Range(owner.m_LazerCoolTime, owner.m_LazerCoolTime + 3); // 쿨타임 갱신

            for (int i = 0; i < owner.m_Chesses.Count; i++)
            {
                if (owner.m_Chesses[i] != null)
                {
                    if (Vector3.Distance(owner.transform.position, owner.m_Chesses[i].transform.position) >= 10)
                    {
                        m_AttackPosition.Add(owner.m_Chesses[i].transform);
                    }
                }
            }

            if(m_AttackPosition.Count > 0)
            {
                int i = Random.Range(1, m_AttackPosition.Count+1) - 1;

                Vector3 v;
                switch (Random.Range(1, 3))
                {
                    case 1: // 가로
                        v = new Vector3(0, 1.28f, m_AttackPosition[i].transform.position.z);
                        Destroy(Instantiate( owner.m_HorizontalPrefab, v, Quaternion.Euler(0,0,0)), 4.0f);
                        break;

                    case 2: // 세로
                        v = new Vector3(m_AttackPosition[i].transform.position.x, 1.28f, 0);
                        Destroy(Instantiate( owner.m_VerticalPrefab, v, Quaternion.Euler(0, 90, 0)), 4.0f);
                        break;
                }

                m_AttackPosition.Clear();
            }

            return;
        }

        m_LazerCoolDown -= Time.deltaTime;
    }

    public override void ExitState(OrcChif_AI owner)
    {

    }
}
