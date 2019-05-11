using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    보스가 원거리 공격을 하면 발생하는 공격 범위의 알파 값을 변경하는 클래스
*/

public class AttackRange : MonoBehaviour {

    private SpriteRenderer m_SR;
    private Color color;
    private float lerp;
    private bool fade;

    private void Start()
    {
        m_SR = GetComponent<SpriteRenderer>();
        color = m_SR.color;

        lerp = 0;
        fade = true;
    }
    
    private void Update()
    {
        if (m_SR.color.a < 1 && fade)
        {
            lerp += Time.deltaTime / 2; // 2초간 색이 진해짐

            color.a = Mathf.Lerp(0, 1, lerp);

            m_SR.color = color;

            if(m_SR.color.a >= 1)
            {
                fade = false;
                lerp = 0;
                color.a = 1;
            }
        } else if (!fade)
        {
            lerp += Time.deltaTime / 1; // 1초간 색이 옅어짐

            color.a = Mathf.Lerp(1, 0, lerp);

            m_SR.color = color;
        }
    }
}
