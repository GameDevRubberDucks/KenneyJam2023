/*
 * LevelCapsuleColliderPlacer.cs
 * 
 * Description:
 * - System to place a huge grid of capsule colliders that we can use for our game's collision while we cut through them
 * 
 * Author(s): 
 * - Dan
*/

using DG.Tweening;
using RubberDucks.Utilities.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RubberDucks.KenneyJam.Level
{
	public class LevelCapsuleColliderPlacer : MonoBehaviour
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
		[Header("Level Information")]
		[SerializeField] private Transform m_BottomLeftBoxCorner = default;
		[SerializeField] private Transform m_TopRightBoxCorner = default;

		[Header("Spawning Information")]
		[Range(0, 50)][SerializeField] private int m_NumSamplesPerAxis = 25;
		[SerializeField] private Transform m_ColliderParent = default;
        //[SerializeField] private CapsuleCollider m_ColliderPrefab = default;
        [SerializeField] private SphereCollider m_ColliderPrefab = default;

		[Header("Culling Information")]
        [SerializeField] private Texture2D m_ActivePathTexture = default;
        [Range(0.0f, 1.0f)][SerializeField] private float m_PathColourCutoff = 0.2f;

        //--- Unity Methods ---//

        //--- Public Methods ---//
        [ContextMenu("SpawnCapsules()")]
		public void SpawnCapsules()
		{
			ClearCapsules();

			float axisLength = m_TopRightBoxCorner.position.x - m_BottomLeftBoxCorner.position.x;
			float capsuleDiameter = axisLength / (float)m_NumSamplesPerAxis;
            float capsuleRadius = capsuleDiameter / 2.0f;

			for (int x = 0; x < m_NumSamplesPerAxis; ++x)
			{
				for (int y = 0; y < m_NumSamplesPerAxis; ++y)
				{
					Vector3 spawnPositionOffset = new Vector3();
                    spawnPositionOffset.x = capsuleDiameter * x;
                    spawnPositionOffset.z = capsuleDiameter * y;

					Vector3 spawnPosition = m_BottomLeftBoxCorner.position + spawnPositionOffset;

					//CapsuleCollider capsule = Instantiate<CapsuleCollider>(m_ColliderPrefab, spawnPosition, Quaternion.identity, m_ColliderParent);
					//capsule.radius = capsuleRadius;

                    SphereCollider capsule = Instantiate<SphereCollider>(m_ColliderPrefab, spawnPosition, Quaternion.identity, m_ColliderParent);
                    capsule.radius = capsuleRadius;
                }
			}
		}

		[ContextMenu("ClearCapsules()")]
		public void ClearCapsules()
		{
            m_ColliderParent.DestroyChildren();
        }

        [ContextMenu("CullCapsules")]
		public void CullCapsules()
		{
            // Keep track of the trees that are to be culled so we can do it at the end without affecting the collection while iterating
            List<GameObject> capsulesToCull = new List<GameObject>();

            for (int i = 0; i < m_ColliderParent.childCount; i++)
            {
                GameObject capsule = m_ColliderParent.GetChild(i).gameObject;

                if (CheckShouldCullCapsule(capsule))
                {
                    capsulesToCull.Add(capsule);
                }
            }

            foreach (var tree in capsulesToCull)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(tree);
                }
                else
                {
                    Destroy(tree);
                }
            }
        }

        //--- Protected Methods ---//

        //--- Private Methods ---//
        private bool CheckShouldCullCapsule(GameObject tree)
        {
            // Figure out the equivalent UV coordinates for the tree based on its relative position within the spawn box
            // These UV coords will be used to sample the path texture and determine if the tree should exist or not
            float treeUCoord = Mathf.InverseLerp(m_BottomLeftBoxCorner.position.x, m_TopRightBoxCorner.position.x, tree.transform.position.x);
            float treeVCoord = Mathf.InverseLerp(m_BottomLeftBoxCorner.position.z, m_TopRightBoxCorner.position.z, tree.transform.position.z);

            // Sample the texture at the matching coordinate 
            Color pathTexColour = m_ActivePathTexture.GetPixelBilinear(treeUCoord, treeVCoord);

            // The colour is black and white so just use the red channel
            // If the colour is below the threshold, it is considered black and is allowed to have a tree
            // If the colour is above, it is considered white and is therefore a path, meaning the tree should be culled
            float pathTexBrightness = pathTexColour.r;

            return pathTexBrightness > m_PathColourCutoff;
        }
    }
}
