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
using RubberDucks.KenneyJam.Level;
using RubberDucks.Utilities;

namespace RubberDucks.KenneyJam.Zones
{
    public class Spawning : MonoBehaviour
    {
        //--- Properties ---//

        //--- Public Variables ---//

        //--- Protected Variables ---//

        //--- Private Variables ---//
        [SerializeField] private LevelAutoGenerator m_LevelGenerator;
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
            DetermineSpawnCollect();
            DetermineSpawnDrop();
            SpawnCollectZone();
        }

        private void Update()
        {

        }

        //--- Public Methods ---//
        [EditorFunctionCall("SpawnDropZone")]

        public void SpawnDropZone()
        {
            Debug.Log("Spawning Drop Zone");
            SpawnZones(true);
        }

        [EditorFunctionCall("SpawnCollectZone")]
        public void SpawnCollectZone()
        {
            Debug.Log("Spawning Collect Zone");
            SpawnZones(false);
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

        private Vector3 DetermineSpawnPositionBasedOnTexture(float colourCutoff, bool canSpawnOnRed, bool canSpawnOnGreen, bool canSpawnOnBlue, bool canSpawnOnBlack)
        {
            Texture2D activePathTexture = m_LevelGenerator.PathTexture;
            Vector3 spawnPosition = new Vector3();

            int numExecutions = 0;
            System.DateTime startTime = System.DateTime.Now;

            // TODO: This is a really bad way of doing this! Could get stuck here for a while!
            bool canSpawnHere = false;
            do
            {
                numExecutions++;

                float randomUCoord = Random.value;
                float randomVCoord = Random.value;

                Color pathTexColour = activePathTexture.GetPixelBilinear(randomUCoord, randomVCoord);

                bool canSpawn = false;
                bool aboveRed = pathTexColour.r > colourCutoff;
                bool aboveGreen = pathTexColour.g > colourCutoff;
                bool aboveBlue = pathTexColour.b > colourCutoff;

                canSpawn |= (canSpawnOnRed && aboveRed);
                canSpawn |= (canSpawnOnGreen && aboveGreen);
                canSpawn |= (canSpawnOnBlue && aboveBlue);

                if (!canSpawn && canSpawnOnBlack)
                {
                    if (!aboveRed && !aboveBlue && !aboveGreen)
                    {
                        canSpawn = true;
                    }
                }

                canSpawnHere = canSpawn;

                spawnPosition.x = Mathf.Lerp(m_BoundsMin.position.x, m_BoundsMax.position.x, randomUCoord);
                spawnPosition.z = Mathf.Lerp(m_BoundsMin.position.z, m_BoundsMax.position.z, randomVCoord);
            }
            while (!canSpawnHere);

            System.DateTime endTime = System.DateTime.Now;
            Debug.LogWarning($"Finding a spawn point took {numExecutions} executions and {endTime.Subtract(startTime).Milliseconds} milliseconds!");
            return spawnPosition;
        }

        private void SpawnZones(bool isDrop)
        {
            if (isDrop)
            {
                DestroyOldZones();

                //DetermineSpawnDrop();
                m_ZoneSpawnPosDrop = DetermineSpawnPositionBasedOnTexture(/*m_LevelGenerator.PathColourCutoff*/0.5f, false, true, false, false);
                m_ActiveZones.Add(Instantiate(m_ZoneDrop, m_ZoneSpawnPosDrop, Quaternion.identity));
            }
            else
            {
                DestroyOldZones();

                //DetermineSpawnCollect();
                m_ZoneSpawnPosCollect = DetermineSpawnPositionBasedOnTexture(m_LevelGenerator.PathColourCutoff, false, true, false, true);
                m_ActiveZones.Add(Instantiate(m_ZoneCollect, m_ZoneSpawnPosCollect, Quaternion.identity));
            }
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
