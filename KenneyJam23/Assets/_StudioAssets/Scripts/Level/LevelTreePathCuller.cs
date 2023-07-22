/*
 * LevelTreePathCuller.cs
 * 
 * Description:
 * - System that culls trees based on the 2D texture of a path, used to setup the starting level layout
 * 
 * Author(s): 
 * - Dan
*/

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace RubberDucks.KenneyJam.Level
{
	public class LevelTreePathCuller : MonoBehaviour
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
		[Header("Level References")] // TODO Dan: Made some things public for ease of assigning data from the auto-generator but should have getters/setters instead!
		[SerializeField] public Transform m_BottomLeftBoxCorner = default;
        [SerializeField] public Transform m_TopRightBoxCorner = default;
		[SerializeField] private Transform m_SpawnedTreeParent = default;

		[Header("Textures")]
		[SerializeField] private bool m_KeepOnBlack = false;
		[SerializeField] private bool m_KeepOnRed = false;
		[SerializeField] private bool m_KeepOnGreen = false;
		[SerializeField] private bool m_KeepOnBlue = false;
        [HideInInspector][SerializeField] public Texture2D m_ActivePathTexture = default;
		[HideInInspector][Range(0.0f, 1.0f)][SerializeField] public float m_PathColourCutoff = 0.2f;


		//--- Unity Methods ---//

		//--- Public Methods ---//
		[ContextMenu("CullTrees()")]
		public void CullTrees()
		{
			// Keep track of the trees that are to be culled so we can do it at the end without affecting the collection while iterating
			List<GameObject> treesToCull = new List<GameObject>();

			for (int i = 0; i < m_SpawnedTreeParent.childCount; i++) 
			{
				GameObject tree = m_SpawnedTreeParent.GetChild(i).gameObject;

                if (CheckShouldCullTree(tree))
				{
					treesToCull.Add(tree);
				}
			}

			foreach(var tree in treesToCull)
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
		private bool CheckShouldCullTree(GameObject tree)
		{
			// Figure out the equivalent UV coordinates for the tree based on its relative position within the spawn box
			// These UV coords will be used to sample the path texture and determine if the tree should exist or not
			float treeUCoord = Mathf.InverseLerp(m_BottomLeftBoxCorner.position.x, m_TopRightBoxCorner.position.x, tree.transform.position.x);
			float treeVCoord = Mathf.InverseLerp(m_BottomLeftBoxCorner.position.z, m_TopRightBoxCorner.position.z, tree.transform.position.z); 

			// Sample the texture at the matching coordinate 
			Color pathTexColour = m_ActivePathTexture.GetPixelBilinear(treeUCoord, treeVCoord);

			// The colour is black and white so just use a single channel
			// If the colour is below the threshold, it is considered black and is allowed to have a tree
			// If the colour is above, it is considered white and is therefore a path, meaning the tree should be culled
			bool shouldKeep = false;
			bool aboveRed = pathTexColour.r > m_PathColourCutoff;
			bool aboveGreen = pathTexColour.g > m_PathColourCutoff;
			bool aboveBlue = pathTexColour.b > m_PathColourCutoff;

            shouldKeep |= (m_KeepOnRed && aboveRed);
            shouldKeep |= (m_KeepOnGreen && aboveGreen);
            shouldKeep |= (m_KeepOnBlack && aboveBlue);

			if (!shouldKeep && m_KeepOnBlack)
			{
				if (!aboveRed && !aboveBlue && !aboveGreen)
				{
                    shouldKeep = true;
                }
            }

            return !shouldKeep;
        }
	}
}
