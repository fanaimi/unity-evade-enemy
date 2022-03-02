using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour
{

    private CarMoveController m_CarMoveController;
    
    
    
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
        
    } // START

    private void StopSkidMarks()
    {
        
    } // STOP

}
