using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarMoveController : MonoBehaviour
{
   private  bool STEERING = true;
   private const bool SPACE_BREAKING= true;
 
   
   /*internal enum PlayType{
      player,
      opponent,
      still
   }
   [SerializeField]private PlayType m_playType;*/
   
   
   
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
   
   [SerializeField] private AudioSource m_engineAudio;
   
   [SerializeField] private BoxCollider m_StartTrack;
   [SerializeField] private BoxCollider m_EndTrack;
   
   private Joystick m_Joystick;
   private Rigidbody m_Rb;
   private Speedometer m_Speedometer;
   private Tachometer m_Tachometer;
   
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
   // private float m_JsDeadZone = .35f;
   // private float m_GearGap = 10f;
   private float m_MaxTurnAngle = 15f;
   private float m_CurrentTurnAngle;
   private float speedToDialRatio = 2f;
   private float rpmToDialRatio = 3f;
   private float m_Radius = 6f;
   private float m_DownardForce = 10f;

   private void Awake()
   {
      m_Joystick = FindObjectOfType<Joystick>();
      m_Rb = GetComponent<Rigidbody>();
      m_Speedometer = FindObjectOfType<Speedometer>();
      m_Tachometer = FindObjectOfType<Tachometer>();
      SetLowerCentreOfMass();
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
      ListenToSpaceBrake();
      ListenToBrakes();
      ApplyWheelsAcceleration();
      ApplyWheelsBrake();
      ApplySteering();
      ControlEngineSound();
      SetCurrentSpeedAndSpedometer();
      SetUpTachometer();
      ControlSteering();
   }

   void ControlSteering()
   {
      if (Input.GetKeyDown(KeyCode.S))
      {
         STEERING = !STEERING;
      }
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

   private void SetUpTachometer()
   {
      // Debug.Log(m_CurrentRpm);
      m_Tachometer.SetRpmNeedle(m_CurrentRpm * rpmToDialRatio);
   }
   
   
   private void SetCurrentSpeedAndSpedometer()
   {
      // Vector3 localVelocity = transform.InverseTransformDirection(m_Rb.velocity);
      
      m_CurrentSpeed = m_Rb.velocity.magnitude;
      m_Speedometer.SetSpeedNeedle(m_CurrentSpeed*2f);
   }

   private void LateUpdate()
   {
      ApplyColliderStateIntoWheels();
   }

   
   private void ControlEngineSound()
   {
      float gearMinValue  = 0f;
      float gearMaxValue = 0f;
      // var audio = GetComponent<AudioSource>();
      
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
         m_engineAudio.pitch = enginePitch;
      }
      else
      {
         float reversedPitch = m_CurrentSpeed / m_MaxSpeed + m_MinPitchAddOn;
         m_CurrentRpm = reversedPitch;
         m_engineAudio.pitch = reversedPitch;
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
      if (STEERING)
      {
         /*m_CurrentTurnAngle = m_MaxTurnAngle * m_Joystick.Direction.x * .8f;
         m_FLwheel.steerAngle = m_CurrentTurnAngle;
         m_FRwheel.steerAngle = m_CurrentTurnAngle;

         Invoke("RemoveSteering", .5f);*/
         
         // ACKERMAN FORMULA FOR STEERING
         //steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontalInput;

         if (m_Joystick.Direction.x > 0)
         {
            m_FLwheel.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (m_Radius + (1.5f / 2))) * m_Joystick.Direction.x;
            m_FRwheel.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (m_Radius - (1.5f / 2))) * m_Joystick.Direction.x;
         }
         else if(m_Joystick.Direction.x < 0)
         {
            m_FLwheel.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (m_Radius - (1.5f / 2))) * m_Joystick.Direction.x;
            m_FRwheel.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (m_Radius + (1.5f / 2))) * m_Joystick.Direction.x;
         }
         else
         {
            m_FLwheel.steerAngle = 0;
            m_FRwheel.steerAngle = 0;
         }

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

   void ListenToSpaceBrake()
   {
      if (SPACE_BREAKING)
      {
         if (Input.GetKey(KeyCode.Space))
         {
            m_BrakePressed = true;
         }
         else
         {
            m_BrakePressed = false;
         }
      }
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


   private void OnCollisionEnter(Collision other)
   {
      if (other.gameObject.CompareTag("Vehicle"))
      {
         AudioManager.Instance.Play("HardCrash");
      }
   }
   
   

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag(m_EndTrack.gameObject.tag) 
      )
      {
         float x = transform.position.x;
         float y = m_StartTrack.gameObject.transform.position.y;
         float z = m_StartTrack.gameObject.transform.position.z;
         transform.position = new Vector3(x,y,z);
      }
   }
   
}
