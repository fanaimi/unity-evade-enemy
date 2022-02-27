using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarMoveController : MonoBehaviour 
{
   // all wheel colliders
   [SerializeField] private WheelCollider m_FLwheel;
   [SerializeField] private WheelCollider m_FRwheel;
   [SerializeField] private WheelCollider m_BLwheel;
   [SerializeField] private WheelCollider m_BRwheel;
   
   // some vars
   public float m_Acceleration = 500f;
   public float m_BreakingForce = 300f;

   public bool m_BrakePressed;
   

   private float m_CurrentAcceleration = 0f;
   private float m_CurrentBrakeForce = 0f;


   private void FixedUpdate()
   {
      ListenToBrakes();
   }

   private void ListenToBrakes()
   {
      if (m_BrakePressed)
      {
         m_CurrentBrakeForce = m_BreakingForce;
      }
      else
      {
         m_CurrentBrakeForce = 0f;
      }
   } // ListenToBrakes
}
