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
   
   
   // ------ vars
   // public vars
   public float m_Acceleration = 500f;
   public float m_BreakingForce = 300f;

   public bool m_BrakePressed;
   
   // private vars
   private float m_CurrentAcceleration = 0f;
   private float m_CurrentBrakeForce = 0f;
   private float m_JsDeadZone = .2f;
   private float m_MaxTurnAngle = 8f;
   private float m_CurrentTurnAngle;

   private void Awake()
   {
      m_Joystick = FindObjectOfType<Joystick>();
   }

   private void Start()
   {
      // AudioManager.Instance.Play("Idle");
   }

   private void FixedUpdate()
   {
      GetVerticalAcceleration();
      ListenToBrakes();
      ApplyWheelsAcceleration();
      ApplyWheelsBrake();
      ApplySteering();
      ApplyColliderStateIntoWheels();
   }

   private void ApplyColliderStateIntoWheels()
   {
      UpdateWheelTransforms(m_FLwheel, m_FLtransform);
      UpdateWheelTransforms(m_FRwheel, m_FRtransform);
      UpdateWheelTransforms(m_BLwheel, m_BLtransform);
      UpdateWheelTransforms(m_BRwheel, m_BRtransform);
   } // ApplyColliderStateIntoWheels

   private void ApplySteering()
   {
      m_CurrentTurnAngle = m_MaxTurnAngle * m_Joystick.Direction.x;
      m_FLwheel.steerAngle = m_CurrentTurnAngle;
      m_FRwheel.steerAngle = m_CurrentTurnAngle;
   } // ApplySteering

   private void GetVerticalAcceleration()
   {
      if (m_Joystick.Direction.magnitude > m_JsDeadZone)
      {
         // AudioManager.Instance.Stop("Idle");
         // AudioManager.Instance.PlayOnce("Low");
         AudioManager.Instance.sounds[0].source.Play();
         // carSounds.nitro.volume = Mathf.MoveTowards(carSounds.nitro.volume, 0.0f, Time.deltaTime * 2.0f);
         m_CurrentAcceleration = m_Acceleration * m_Joystick.Direction.y;
      }
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
   } // ApplyWheelsAcceleration

   private void ListenToBrakes()
   {
      if (m_BrakePressed)
      {
         AudioManager.Instance.PlayOnce("Brake");
         Debug.Log("down");
         m_CurrentBrakeForce = m_BreakingForce;
      }
      else
      {
         
         AudioManager.Instance.Stop("Brake");
         Debug.Log("up");
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
