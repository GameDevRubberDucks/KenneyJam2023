/*
 * Spawning.cs
 * 
 * Description:
 * - 
 * 
 * Author(s): 
 * - 
*/

using UnityEngine;
using UnityEngine.Events;

using System.Collections.Generic;

namespace RubberDucks.ProjectName.Module
{
    public class Spawning : MonoBehaviour
    {
        //--- Properties ---//

        //--- Public Variables ---//

        //--- Protected Variables ---//

        //--- Private Variables ---//
        [SerializeField] private Transform m_BoundsMin;
        [SerializeField] private Transform m_BoundsMax;

        [SerializeField] private Vector3 m_ZoneSpawnPosDrop = default;
        [SerializeField] private Vector3 m_ZoneSpawnPosCollect = default;
        [SerializeField] private float m_MinSpawnDist = default;

        [SerializeField] private GameObject m_ZoneDrop;
        [SerializeField] private GameObject m_ZoneCollect;
        [SerializeField] private List<GameObject> m_ActiveZones;

        //--- Unity Methods ---//
        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnZones();
            }
        }

        //--- Public Methods ---//
        public void SpawnZones()
        {
            GenerateSpawnZones();
        }

        //--- Protected Methods ---//

        //--- Private Methods ---//
        private void DetermineSpawnCollect()
        {
            float xCoord = Random.Range(m_BoundsMin.position.x, m_BoundsMax.position.x);
            float zCoord = Random.Range(m_BoundsMin.position.z, m_BoundsMax.position.z);

            m_ZoneSpawnPosCollect = new Vector3(xCoord, 0.0f, zCoord);
        }

        private void DetermineSpawnDrop()
        {
            float xCoord = Random.Range(m_BoundsMin.position.x, m_BoundsMax.position.x);
            float zCoord = Random.Range(m_BoundsMin.position.z, m_BoundsMax.position.z);

            m_ZoneSpawnPosDrop = new Vector3(xCoord, 0.0f, zCoord);
        }

        private void GenerateSpawnZones()
        {
            DetermineSpawnCollect();
            DetermineSpawnDrop();

            //float dist = Vector3.Distance(m_ZoneSpawnPosCollect, m_ZoneSpawnPosCollect);


            DestroyOldZones();

            m_ActiveZones.Add(Instantiate(m_ZoneDrop, m_ZoneSpawnPosDrop, Quaternion.identity));
            m_ActiveZones.Add(Instantiate(m_ZoneCollect, m_ZoneSpawnPosCollect, Quaternion.identity));


        }

        private void DestroyOldZones()
        {
            for (int i = 0; i < m_ActiveZones.Count; i++)
            {
                Destroy(m_ActiveZones[i]);
            }

            m_ActiveZones.Clear();
        }
    }
}
