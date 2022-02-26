using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform m_LeftGuardRail;
    [SerializeField] private Transform m_RightGuardRail;
    
    private float m_VertSpeed;
    private float m_HorSpeed;
    private float m_DeadZone = .2f;

    private Rigidbody m_Rb;
    private Joystick m_Joystick;

    private void Awake()
    {
        m_VertSpeed = 25f;
        m_HorSpeed = 5f;
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
            float deltaX =  m_Joystick.Direction.x * Time.fixedDeltaTime * m_HorSpeed;
            
            
            if (
                transform.position.x + deltaX <= m_LeftGuardRail.position.x  + .5f|| 
                transform.position.x + deltaX >= m_RightGuardRail.position.x -.5f 
            )
            {
                deltaX = 0;
            }
            


                // MOVING
            /*//Debug.Log(transform.position.y);
            m_Rb.AddForce(transform.forward * m_Joystick.Direction.y * m_MoveSpeed);
            m_Rb.AddForce(transform.right * m_Joystick.Direction.x * m_TurnSpeed);
            Debug.Log(m_Joystick.Direction);*/
            transform.position = new Vector3(
                transform.position.x + deltaX,
                transform.position.y,
                transform.position.z + m_Joystick.Direction.y * Time.fixedDeltaTime * m_VertSpeed
            );
        }
        else
        {
            // STOPPED
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Vehicle"))
        {
           
        }
    }
}
