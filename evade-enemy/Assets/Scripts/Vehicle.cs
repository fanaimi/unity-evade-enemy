using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Vehicle : MonoBehaviour
{
   
   
   
   // ------ references
   // all wheel colliders
   [SerializeField] private WheelCollider m_FLwheel;
   [SerializeField] private WheelCollider m_FRwheel;
   [SerializeField] private WheelCollider m_BLwheel;
   [SerializeField] private WheelCollider m_BRwheel;
   [SerializeField] private Transform m_CentreOfMass;

   // all wheel transforms
   [SerializeField] private Transform m_FLtransform;
   [SerializeField] private Transform m_FRtransform;
   [SerializeField] private Transform m_BLtransform;
   [SerializeField] private Transform m_BRtransform;
   
   private Rigidbody m_Rb;
   
   // ------ vars
   // public vars
   [SerializeField] public float m_Acceleration = 8f;
   [SerializeField] public float m_BreakingForce = 5f;
   
   [HideInInspector] public bool m_BrakePressed;
   [HideInInspector] public bool m_IsGrounded;
   
   // private vars
   [SerializeField] private int[] m_GearSpeeds = new int[6];
   private float m_CurrentSpeed;
   private float m_CurrentRpm;
   private float m_MaxSpeed = 60f;
   private float m_MinPitchAddOn = .27f;
   private float m_PitchAddOn;
   private float m_CurrentAcceleration = 0f;
   private float m_CurrentBrakeForce = 0f;
   
   
   private float speedToDialRatio = 2f;
   
   private float m_Radius = 6f;
   private float m_DownardForce = 10f;

   private void Awake()
   {
      m_Rb = GetComponent<Rigidbody>();
      // SetLowerCentreOfMass();
   }

   private void SetLowerCentreOfMass()
   {
      m_Rb.centerOfMass = m_CentreOfMass.transform.localPosition;   
   }

   private void Start()
   {
      // m_Tachometer.SetRpmNeedle(8);
      // m_Speedometer.SetSpeedNeedle(100f);
      // AudioManager.Instance.PlayOnce("Idle");
   }

   private void FixedUpdate()
   {
      CheckIfGrounded();
      AddDownwardForce();
      GetVerticalAcceleration();
      ApplyWheelsAcceleration();
      ApplyWheelsBrake();
      ControlEngineSound();
   }

   private void AddDownwardForce()
   {
      if(m_IsGrounded)
         m_Rb.AddForce(-transform.up * m_DownardForce * m_Rb.velocity.magnitude );
   }

   private void CheckIfGrounded()
   {
      if (m_FLwheel.isGrounded &&
          m_FRwheel.isGrounded &&
          m_BLwheel.isGrounded &&
          m_BRwheel.isGrounded)
      {
         m_IsGrounded = true;
      }
      else
      {
         m_IsGrounded = false;
      }
   }

   
   

   private void LateUpdate()
   {
      ApplyColliderStateIntoWheels();
   }

   
   
   private void ControlEngineSound()
   {
      float gearMinValue  = 0f;
      float gearMaxValue = 0f;
      var audio = GetComponent<AudioSource>();
      // Debug.Log(audio);
      
      if (m_Rb.velocity.z >= 0)
      {

         if (m_CurrentSpeed < m_GearSpeeds[0])
         {
            gearMinValue = 0;
            gearMaxValue = m_GearSpeeds[0] - 1;
            m_PitchAddOn = m_MinPitchAddOn;
         }
         else if (m_CurrentSpeed < m_GearSpeeds[1])
         {
            gearMinValue = m_GearSpeeds[0];
            gearMaxValue = m_GearSpeeds[1] - 1;
            m_PitchAddOn = m_MinPitchAddOn * 2;
         }
         else if (m_CurrentSpeed < m_GearSpeeds[2])
         {
            gearMinValue = m_GearSpeeds[1];
            gearMaxValue = m_GearSpeeds[2] - 1;
            m_PitchAddOn = m_MinPitchAddOn * 3;
         }
         else if (m_CurrentSpeed < m_GearSpeeds[3])
         {
            gearMinValue = m_GearSpeeds[2];
            gearMaxValue = m_GearSpeeds[3] - 1;
            m_PitchAddOn = m_MinPitchAddOn * 4;
         }
         else if (m_CurrentSpeed < m_GearSpeeds[4])
         {
            gearMinValue = m_GearSpeeds[3];
            gearMaxValue = m_GearSpeeds[4] - 1;
            m_PitchAddOn = m_MinPitchAddOn * 5;
         }
         else if (m_CurrentSpeed < m_GearSpeeds[5])
         {
            gearMinValue = m_GearSpeeds[4];
            gearMaxValue = m_GearSpeeds[5] - 1;
            m_PitchAddOn = m_MinPitchAddOn * 6;
         }



         float enginePitch = ((m_CurrentSpeed - gearMinValue) / (gearMaxValue - gearMinValue)) + m_PitchAddOn;
         m_CurrentRpm = enginePitch;
         audio.pitch = enginePitch;
      }
      else
      {
         float reversedPitch = m_CurrentSpeed / m_MaxSpeed + m_MinPitchAddOn;
         m_CurrentRpm = reversedPitch;
         audio.pitch = reversedPitch;
      }

   } // ControlEngineSound

   private void ApplyColliderStateIntoWheels()
   {
      UpdateWheelTransforms(m_FLwheel, m_FLtransform);
      UpdateWheelTransforms(m_FRwheel, m_FRtransform);
      UpdateWheelTransforms(m_BLwheel, m_BLtransform);
      UpdateWheelTransforms(m_BRwheel, m_BRtransform);
   } // ApplyColliderStateIntoWheels

   


   private void GetVerticalAcceleration()
   {
      m_CurrentAcceleration = m_Acceleration ;
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
