/*
 * LevelAutoGenerator.cs
 * 
 * Description:
 * - System to automatically generate the whole level at the start of the game. Bit hacky but it basically just calls all of the other scripts in the same order that is needed manuallly
 * - TODO Dan: Should really clean up these scripts so there is less repetition and overlap
 * 
 * Author(s): 
 * - Dan
*/

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace RubberDucks.KenneyJam.Level
{
	public class LevelAutoGenerator : MonoBehaviour
	{
		//--- Events ---//
		[System.Serializable]
		public class EventList
		{
		}
		[Header("Events")]
		public EventList Events = default;

		//--- Properties ---//

		//--- Public Variables ---//

		//--- Protected Variables ---//

		//--- Private Variables ---//
		[Header("Data To Assign")]
        [SerializeField] private Transform m_BottomLeftBoxCorner = default;
        [SerializeField] private Transform m_TopRightBoxCorner = default;
        [SerializeField] private Texture2D m_ActivePathTexture = default;
        [Range(0.0f, 1.0f)][SerializeField] private float m_PathColourCutoff = 0.2f;

		[Header("Tree Data")]
		[SerializeField] private LevelTreePlacer m_TreePlacer = default;
        [Range(0, 10000)][SerializeField] private int m_NumTreesToSpawn = 5000;
        [SerializeField] private LevelTreePathCuller m_TreeCuller = default;

        [Header("Rock Data")]
        [SerializeField] private LevelTreePlacer m_RockPlacer = default;
        [Range(0, 10000)][SerializeField] private int m_NumRocksToSpawn = 5000;
		[SerializeField] private LevelTreePathCuller m_RockCuller = default;

        [Header("Collider Data")]
		[SerializeField] private LevelCapsuleColliderPlacer m_CapsulePlacer = default;
        [Range(0, 100)][SerializeField] private int m_NumCollidersPerAxis = 50;

		[Header("Post-Process Data")]
		[SerializeField] private float m_LevelScale = 1.0f;

        //--- Unity Methods ---//
        private void Awake()
        {
			AssignData();
			GenerateLevel();
        }

        //--- Public Methods ---//
        public void AssignData()
		{
			// TODO Dan: This is bad. Should really restructure the scripts to work better but doing it this way to have one central point for assigning data
			m_TreePlacer.m_BottomLeftBoxCorner = m_BottomLeftBoxCorner;
			m_TreePlacer.m_TopRightBoxCorner = m_TopRightBoxCorner;
			m_TreePlacer.m_NumTreesToSpawn = m_NumTreesToSpawn;
            m_TreeCuller.m_ActivePathTexture = m_ActivePathTexture;
            m_TreeCuller.m_PathColourCutoff = m_PathColourCutoff;

            m_RockPlacer.m_BottomLeftBoxCorner = m_BottomLeftBoxCorner;
            m_RockPlacer.m_TopRightBoxCorner = m_TopRightBoxCorner;
            m_RockPlacer.m_NumTreesToSpawn = m_NumRocksToSpawn;
			m_RockCuller.m_ActivePathTexture = m_ActivePathTexture;
			m_RockCuller.m_PathColourCutoff = m_PathColourCutoff;

            m_CapsulePlacer.m_BottomLeftBoxCorner = m_BottomLeftBoxCorner;
            m_CapsulePlacer.m_TopRightBoxCorner = m_TopRightBoxCorner;
			m_CapsulePlacer.m_NumSamplesPerAxis = m_NumCollidersPerAxis;
			m_CapsulePlacer.m_ActivePathTexture = m_ActivePathTexture;
			m_CapsulePlacer.m_PathColourCutoff = m_PathColourCutoff;
		}

		[ContextMenu("Generate Level")]
		public void GenerateLevel()
		{
			DateTime startTime = DateTime.Now;

			m_TreePlacer.SpawnTrees();
			m_TreeCuller.CullTrees();

			m_CapsulePlacer.SpawnCapsules();
			m_CapsulePlacer.RegisterTreesToCapsules();
			m_CapsulePlacer.CullCapsules();

			m_RockPlacer.SpawnTrees();
			m_RockCuller.CullTrees();

			this.transform.localScale = Vector3.one * m_LevelScale;

            DateTime endTime = DateTime.Now;

            Debug.Log($"Generated level in {endTime.Subtract(startTime).Milliseconds} milliseconds");
		}

		[ContextMenu("Clear Level")]
		public void ClearLevel()
		{
			m_TreePlacer.ClearTrees();
			m_CapsulePlacer.ClearCapsules();
		}
		
		//--- Protected Methods ---//
		
		//--- Private Methods ---//
	}
}
