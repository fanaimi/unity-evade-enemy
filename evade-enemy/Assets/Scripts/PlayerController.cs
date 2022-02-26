using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private float m_MoveSpeed;
    [SerializeField] private float m_TurnSpeed;
    private float m_DeadZone = .2f;

    private Rigidbody m_Rb;
    private Joystick m_Joystick;

    private void Awake()
    {
        m_MoveSpeed = 10f;
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
            /*//Debug.Log(transform.position.y);
            m_Rb.AddForce(transform.forward * m_Joystick.Direction.y * m_MoveSpeed);
            m_Rb.AddForce(transform.right * m_Joystick.Direction.x * m_TurnSpeed);
            Debug.Log(m_Joystick.Direction);*/
            transform.position = new Vector3(
                transform.position.x + m_Joystick.Direction.x * Time.fixedDeltaTime * m_MoveSpeed,
                transform.position.y,
                transform.position.z + m_Joystick.Direction.y * Time.fixedDeltaTime * m_MoveSpeed
            );
        }
        else
        {
            // STOPPED
        }
    }
}
