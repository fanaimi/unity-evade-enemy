using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tachometer : MonoBehaviour
{

    private const float MIN_RPM_ANGLE = 213f;
    private const float MAX_RPM_ANGLE = -42f;
    
    private Transform m_RpmNeedleTr;


    private float m_CurrentRpm;
    private float m_MaxRpm;
    

    private void Awake()
    {
        m_RpmNeedleTr = transform.Find("EngineNeedle");
        m_CurrentRpm = 0f;
        m_MaxRpm = 9f;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(m_RpmNeedleTr.position);
        SetRpmNeedle(4.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRpmNeedle(float rpm)
    {
        m_CurrentRpm = rpm;
        if (m_CurrentRpm > m_MaxRpm)
        {
            m_CurrentRpm = m_MaxRpm;
        }

        m_RpmNeedleTr.eulerAngles = new Vector3(0, 0, GetRpmNedleRotation());
    }



    float GetRpmNedleRotation()
    {
        float totalRotationAngle = MIN_RPM_ANGLE - MAX_RPM_ANGLE;

        float normalizedRpm = m_CurrentRpm / m_MaxRpm; // value between 0 -> 1

        return MIN_RPM_ANGLE - normalizedRpm * totalRotationAngle ;
    }

}
