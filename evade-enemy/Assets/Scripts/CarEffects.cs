using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour
{

    private CarMoveController m_CarMoveController;
    [SerializeField] private TrailRenderer[] m_SkidMarks = new TrailRenderer[4];
    [SerializeField] private AudioSource m_SkidAudio;
    private bool m_SkidMarksOn;
    
    
    // Start is called before the first frame update
    void Start()
    {
        m_CarMoveController = GetComponent<CarMoveController>();
    }

    // Update is called once per frame
    void Update()
    {
        ListenToBrakes();
    }

    private void ListenToBrakes()
    {
        // skid marks: https://www.youtube.com/watch?v=0LOcxZhkVwc&ab_channel=pabloslab
        if (m_CarMoveController.m_BrakePressed)
        {
            StartSkidMarks();
        }
        else
        {
            StopSkidMarks();
        }
    }
    private void StartSkidMarks()
    {
        if (m_SkidMarksOn) return;
        foreach (TrailRenderer SkidMark in m_SkidMarks)
        {
            SkidMark.emitting = true;
        }
        m_SkidAudio.Play();
        m_SkidMarksOn = true;
    } // START

    private void StopSkidMarks()
    {
        if (!m_SkidMarksOn) return;
        foreach (TrailRenderer SkidMark in m_SkidMarks)
        {
            SkidMark.emitting = false;
        }
        m_SkidAudio.Stop();
        m_SkidMarksOn = false;
    } // STOP

}
