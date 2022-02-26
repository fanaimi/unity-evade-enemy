using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_TurnSpeed;
    private float m_DeadZone = .2f;

    private Rigidbody m_Rb;
    private Joystick m_Joystick;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_Joystick = FindObjectOfType<Joystick>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        ListenToJoystick();
    }

    private void ListenToJoystick()
    {
        if (m_Joystick.Direction.magnitude > m_DeadZone)
        {
            // MOVING
   
        }
        else
        {
            // STOPPED
        }
    }
}
