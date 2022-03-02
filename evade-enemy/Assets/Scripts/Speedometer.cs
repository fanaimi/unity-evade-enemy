using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour
{

    private const float MIN_SPEED_ANGLE = 220f;
    private const float MAX_SPEED_ANGLE = -30f;
    
    private Transform m_SpeedNeedleTr;


    private float m_CurrentSpeed;
    private float m_MaxSpeed;
    

    private void Awake()
    {
        m_SpeedNeedleTr = transform.Find("SpeedNeedle");
        m_CurrentSpeed = 0f;
        m_MaxSpeed = 120f;
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(m_SpeedNeedleTr.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpeedNeedle(float _speed)
    {
        m_CurrentSpeed = _speed;
        if (m_CurrentSpeed > m_MaxSpeed)
        {
            m_CurrentSpeed = m_MaxSpeed;
        }

        m_SpeedNeedleTr.eulerAngles = new Vector3(0, 0, GetSpeedNedleRotation());
    }



    float GetSpeedNedleRotation()
    {
        float totalRotationAngle = MIN_SPEED_ANGLE - MAX_SPEED_ANGLE;

        float normalizedSpeed = m_CurrentSpeed / m_MaxSpeed; // value between 0 -> 1

        return MIN_SPEED_ANGLE - normalizedSpeed * totalRotationAngle ;
    }

}
