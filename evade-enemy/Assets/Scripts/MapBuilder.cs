using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private GameObject m_RoadPiecePrefab;

    private float m_NumOfRoadPieces = 200f;
    private float m_PieceOffset = 10f;
    
    void Start()
    {
        for (int i = 0; i < m_NumOfRoadPieces; i++)
        {
            Instantiate(m_RoadPiecePrefab, new Vector3(0, 0, i * m_PieceOffset), Quaternion.identity, transform);
        }        
    }

    
}
