using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    public GameObject m_RoadPiecePrefab;

    private float m_NumOfRoadPieces = 240f;
    private float m_PieceOffset = 10f;
    
    void Start()
    {
        for (int i = 0; i < m_NumOfRoadPieces; i++)
        {
            GameObject road = Instantiate(
                m_RoadPiecePrefab, 
                new Vector3(0, 0, i * m_PieceOffset), 
                Quaternion.identity, 
                transform
            );
            if (i == 198)
            {
                road.transform.Rotate(-10, 0, 0);
            }
        }        
    }

    
}
