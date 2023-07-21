/*
 * LevelTreePlacer.cs
 * 
 * Description:
 * - Script to randomly place trees, rotate them, scale them, etc to add some variety to the scene
 * 
 * Author(s): 
 * - Dan
*/

using UnityEngine;
using UnityEngine.Events;

using System.Collections.Generic;

using RubberDucks.Utilities.Extensions;

namespace RubberDucks.KenneyJam.Level
{
	public class LevelTreePlacer : MonoBehaviour
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
		[Header("Tree Visual Randomness")]
		[SerializeField] private Vector2 m_TreeScaleRange = new Vector2(0.8f, 1.2f);
		[SerializeField] private Vector2 m_TreeRotYRange = new Vector2(0.0f, 360.0f);

		[Header("Tree Spawning")]
		[Range(0, 10000)][SerializeField] private int m_NumTreesToSpawn = 500;
		[SerializeField] private Transform m_BottomLeftBoxCorner = default;
		[SerializeField] private Transform m_TopRightBoxCorner = default;
		[SerializeField] private Transform m_SpawnedTreeParent = default;
		[SerializeField] private List<GameObject> m_TreePrefabs = default;

		//--- Unity Methods ---//

		//--- Public Methods ---//
		[ContextMenu("SpawnTrees()")]
		public void SpawnTrees()
		{
			ClearTrees();

			for (int i = 0; i < m_NumTreesToSpawn; ++i) 
			{
				SpawnNewTree();
			}
		}

		[ContextMenu("ClearTrees()")]
		public void ClearTrees()
		{
			m_SpawnedTreeParent.DestroyChildren();
		}

		//--- Protected Methods ---//
		
		//--- Private Methods ---//
		private void SpawnNewTree()
		{
			GameObject treePrefabToSpawn = m_TreePrefabs.GetRandomElement();

			Vector3 spawnPosition = new Vector3();
			spawnPosition.x = Random.Range(m_BottomLeftBoxCorner.position.x, m_TopRightBoxCorner.position.x);
			spawnPosition.z = Random.Range(m_BottomLeftBoxCorner.position.z, m_TopRightBoxCorner.position.z);

			float spawnRotationY = m_TreeRotYRange.RandValBetweenXAndY();
			Quaternion spawnRotation = Quaternion.Euler(0.0f, spawnRotationY, 0.0f);

			float spawnScale = m_TreeRotYRange.RandValBetweenXAndY();
			Vector3 spawnScaleVec = Vector3.one * spawnScale;

			Instantiate(treePrefabToSpawn, spawnPosition, spawnRotation, m_SpawnedTreeParent);
		}
	}
}
