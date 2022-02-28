using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarMoveController : MonoBehaviour 
{
   // ------ references
   // all wheel colliders
   [SerializeField] private WheelCollider m_FLwheel;
   [SerializeField] private WheelCollider m_FRwheel;
   [SerializeField] private WheelCollider m_BLwheel;
   [SerializeField] private WheelCollider m_BRwheel;

   // all wheel transforms
   [SerializeField] private Transform m_FLtransform;
   [SerializeField] private Transform m_FRtransform;
   [SerializeField] private Transform m_BLtransform;
   [SerializeField] private Transform m_BRtransform;
   
   private Joystick m_Joystick;
   private Rigidbody m_Rb;
   
   
   // ------ vars
   // public vars
   [SerializeField] public float m_Acceleration = 400f;
   [SerializeField] public float m_BreakingForce = 250f;
   
   [HideInInspector] public bool m_BrakePressed;
   
   // private vars
   [SerializeField] private int[] m_GearSpeeds = new int[6];
   private float m_CurrentSpeed;
   private float m_MaxSpeed = 60f;
   private float m_GearGap = 10f;
   private float m_MinPitch = .27f;
   private float m_CurrentAcceleration = 0f;
   private float m_CurrentBrakeForce = 0f;
   private float m_JsDeadZone = .3f;
   private float m_MaxTurnAngle = 5f;
   private float m_CurrentTurnAngle;

   private void Awake()
   {
      m_Joystick = FindObjectOfType<Joystick>();
      m_Rb = GetComponent<Rigidbody>();
   }

   private void Start()
   {
      // AudioManager.Instance.PlayOnce("Idle");
   }

   private void FixedUpdate()
   {
      GetVerticalAcceleration();
      ListenToBrakes();
      ApplyWheelsAcceleration();
      ApplyWheelsBrake();
      // ApplySteering();
      ApplyColliderStateIntoWheels();
      ControlEngineSound();
   }

   private void ControlEngineSound()
   {
      var audio = GetComponent<AudioSource>();
      m_CurrentSpeed = m_Rb.velocity.magnitude;
      Debug.Log(m_CurrentSpeed);


      /*
      if ( Mathf.RoundToInt(m_CurrentSpeed) % 10 == 0)
      {
         m_MaxSpeed -= 10;
         Debug.Log(m_MaxSpeed);
      }*/

      float soundPitch = m_CurrentSpeed / m_MaxSpeed + m_MinPitch;
      
      Debug.Log(soundPitch);
      audio.pitch = soundPitch;
   } // ControlEngineSound

   private void ApplyColliderStateIntoWheels()
   {
      UpdateWheelTransforms(m_FLwheel, m_FLtransform);
      UpdateWheelTransforms(m_FRwheel, m_FRtransform);
      UpdateWheelTransforms(m_BLwheel, m_BLtransform);
      UpdateWheelTransforms(m_BRwheel, m_BRtransform);
   } // ApplyColliderStateIntoWheels

   private void ApplySteering()
   {
      if (Mathf.Abs(m_Joystick.Direction.x) > m_JsDeadZone)
      {
         m_CurrentTurnAngle = m_MaxTurnAngle * m_Joystick.Direction.x;
         m_FLwheel.steerAngle = m_CurrentTurnAngle;
         m_FRwheel.steerAngle = m_CurrentTurnAngle;
      }
   } // ApplySteering

   private void GetVerticalAcceleration()
   {
      m_CurrentAcceleration = m_Acceleration * m_Joystick.Direction.y;
   } // GetVerticalAcceleration

   private void ApplyWheelsBrake()
   {
      m_FLwheel.brakeTorque = m_CurrentBrakeForce;
      m_FRwheel.brakeTorque = m_CurrentBrakeForce;
      m_BRwheel.brakeTorque = m_CurrentBrakeForce;
      m_BLwheel.brakeTorque = m_CurrentBrakeForce;
   } // ApplyWheelsBrake

   /// <summary>
   /// acceleration for front wheels only
   /// </summary>
   private void ApplyWheelsAcceleration()
   {
      m_FLwheel.motorTorque = m_CurrentAcceleration;
      m_FRwheel.motorTorque = m_CurrentAcceleration;
      /*m_BLwheel.motorTorque = m_CurrentAcceleration;
      m_BRwheel.motorTorque = m_CurrentAcceleration;*/
   } // ApplyWheelsAcceleration

   private void ListenToBrakes()
   {
      if (m_BrakePressed)
      {
         AudioManager.Instance.PlayOnce("Brake");
         // Debug.Log("down");
         m_CurrentBrakeForce = m_BreakingForce;
      }
      else
      {
         
         AudioManager.Instance.Stop("Brake");
         // Debug.Log("up");
         m_CurrentBrakeForce = 0f;
      }
   } // ListenToBrakes


   void UpdateWheelTransforms(WheelCollider _collider, Transform _transform)
   {
      // getting data from wheel collider into these vars
      Vector3 position;
      Quaternion rotation;
      
      _collider.GetWorldPose(out position, out rotation);
      
      // setting data into wheel transform
      _transform.position = position;
      _transform.rotation = rotation;

   }

}
