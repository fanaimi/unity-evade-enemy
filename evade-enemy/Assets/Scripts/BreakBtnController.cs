using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BreakBtnController : MonoBehaviour
{
    private CarMoveController m_CarMoveController;
    // Start is called before the first frame update
    void Start()
    {
        m_CarMoveController = FindObjectOfType<CarMoveController>();
    }

    public void OnPressedrDown()
    {
        Debug.Log("down");
        m_CarMoveController.m_BrakePressed = true;
    }
 
    public void OnPressedUp()
    {
        Debug.Log("up");
        m_CarMoveController.m_BrakePressed = false;
    }
    
    
}
