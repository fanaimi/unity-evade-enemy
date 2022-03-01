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
   [SerializeField] public float m_Acceleration = 8f;
   [SerializeField] public float m_BreakingForce = 5f;
   
   [HideInInspector] public bool m_BrakePressed;
   
   // private vars
   [SerializeField] private int[] m_GearSpeeds = new int[6];
   private float m_CurrentSpeed;
   private float m_MaxSpeed = 60f;
   private float m_GearGap = 10f;
   private float m_MinPitchAddOn = .27f;
   private float m_PitchAddOn;
   private float m_CurrentAcceleration = 0f;
   private float m_CurrentBrakeForce = 0f;
   private float m_JsDeadZone = .35f;
   private float m_MaxTurnAngle = 15f;
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
      ApplySteering();
      // ApplyColliderStateIntoWheels();
      ControlEngineSound();
   }

   private void LateUpdate()
   {
      ApplyColliderStateIntoWheels();
   }

   private void ControlEngineSound_SINGLEGEAR()
   {
      var audio = GetComponent<AudioSource>();
      m_CurrentSpeed = m_Rb.velocity.magnitude;
      
      float soundPitch = m_CurrentSpeed / m_MaxSpeed + m_MinPitchAddOn;
      audio.pitch = soundPitch;
   } // ControlEngineSound_SINGLEGEAR
   
   
   private void ControlEngineSound()
   {
      float gearMinValue  = 0f;
      float gearMaxValue = 0f;
      var audio = GetComponent<AudioSource>();
      m_CurrentSpeed = m_Rb.velocity.magnitude;
      // Debug.Log(m_Rb.velocity);
      

      /*for (int i = 0; i < m_GearSpeeds.Length; i++)
      {
         if (m_CurrentSpeed < m_GearSpeeds[i])
         {
            if (i == 0)
            {
               gearMinValue = 0;
            }
            else
            {
               gearMinValue = m_GearSpeeds[i];
            }

            m_PitchAddOn = m_MinPitchAddOn * i;
            gearMaxValue = m_GearSpeeds[i] - 1;
         }
         else
         {
            break;
         }
      }*/

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
         audio.pitch = enginePitch;
      }
      else
      {
         float reversedPitch = m_CurrentSpeed / m_MaxSpeed + 3 * m_MinPitchAddOn;
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

   private void ApplySteering()
   {
      // if (Mathf.Abs(m_Joystick.Direction.x) > m_JsDeadZone)
      if (true)
      {
         m_CurrentTurnAngle = m_MaxTurnAngle * m_Joystick.Direction.x * .8f;
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
