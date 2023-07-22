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
        [Header("Level Information")] // TODO Dan: Made some things public for ease of assigning data from the auto-generator but should have getters/
        [HideInInspector][SerializeField] public Transform m_BottomLeftBoxCorner = default;
        [HideInInspector][SerializeField] public Transform m_TopRightBoxCorner = default;

        [Header("Spawning Information")]
        [HideInInspector][Range(0, 50)][SerializeField] public int m_NumSamplesPerAxis = 25;
        [SerializeField] private Transform m_ColliderParent = default;
        //[SerializeField] private CapsuleCollider m_ColliderPrefab = default;
        [SerializeField] private SphereCollider m_ColliderPrefab = default;

        [Header("Culling Information")]
        [HideInInspector][SerializeField] public Texture2D m_ActivePathTexture = default;
        [HideInInspector][Range(0.0f, 1.0f)][SerializeField] public float m_PathColourCutoff = 0.2f;

        [Header("Tree Information For Registration")]
        [SerializeField] private Transform TreeParent = default;

        private Dictionary<Vector2Int, LevelForestCollider> m_ColliderGrid = new Dictionary<Vector2Int, LevelForestCollider>();

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

                    Vector2Int spawnCoords = new Vector2Int(x, y);
                    LevelForestCollider forestComp = capsule.GetComponent<LevelForestCollider>();

                    m_ColliderGrid.Add(spawnCoords, forestComp);
                }
            }
        }

        [ContextMenu("ClearCapsules()")]
        public void ClearCapsules()
        {
            m_ColliderParent.DestroyChildren();
            m_ColliderGrid.Clear();
        }

        [ContextMenu("CullCapsules")]
        public void CullCapsules()
        {
            // Keep track of the trees that are to be culled so we can do it at the end without affecting the collection while iterating
            List<GameObject> capsulesToCull = new List<GameObject>();

            for (int i = 0; i < m_ColliderParent.childCount; i++)
            {
                GameObject capsule = m_ColliderParent.GetChild(i).gameObject;

                if (HandleCapsuleBasedOnLevelTexture(capsule))
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

        [ContextMenu("RegisterTreesToCapsules()")]
        public void RegisterTreesToCapsules()
        {
            for (int i = 0; i < TreeParent.childCount; ++i)
            {
                GameObject tree = TreeParent.GetChild(i).gameObject;

                // Determine the x and y index of the tree and assign it to the appropriate capsule
                float percentX = Mathf.InverseLerp(m_BottomLeftBoxCorner.position.x, m_TopRightBoxCorner.position.x, tree.transform.position.x);
                float percentY = Mathf.InverseLerp(m_BottomLeftBoxCorner.position.z, m_TopRightBoxCorner.position.z, tree.transform.position.z);

                int coordX = Mathf.FloorToInt(percentX * m_NumSamplesPerAxis);
                int coordY = Mathf.FloorToInt(percentY * m_NumSamplesPerAxis);
                Vector2Int coord = new Vector2Int(coordX, coordY);

                if (m_ColliderGrid.ContainsKey(coord))
                {
                    m_ColliderGrid[coord].RegisterTreeAsChild(tree);
                    i--;
                }
                else
                {
                    Debug.LogError($"Tree {tree.name} at position {tree.transform.position} could not be added to grid with coord {coord}");
                }
            }
        }

        //--- Protected Methods ---//

        //--- Private Methods ---//
        private bool HandleCapsuleBasedOnLevelTexture(GameObject capsule)
        {
            // Figure out the equivalent UV coordinates for the tree based on its relative position within the spawn box
            // These UV coords will be used to sample the path texture and determine if the tree should exist or not
            float treeUCoord = Mathf.InverseLerp(m_BottomLeftBoxCorner.position.x, m_TopRightBoxCorner.position.x, capsule.transform.position.x);
            float treeVCoord = Mathf.InverseLerp(m_BottomLeftBoxCorner.position.z, m_TopRightBoxCorner.position.z, capsule.transform.position.z);

            // Sample the texture at the matching coordinate 
            Color pathTexColour = m_ActivePathTexture.GetPixelBilinear(treeUCoord, treeVCoord);

            // The red channel is if the collider is an obstacle, in which case it should not be able to be destroyed
            // The green channel is if the collider is on a path, in which case it should be culled completely
            float texRedChannel = pathTexColour.r;
            float texGreenChannel = pathTexColour.g;

            if (texRedChannel > m_PathColourCutoff)
            {
                if (capsule.TryGetComponent<LevelForestCollider>(out var forestCollider))
                {
                    Destroy(forestCollider); // Deleting this component will prevent the obstacle from being pruned by the bulldoze system
                }

                //return false;
                return true;
            }
            else if (texGreenChannel > m_PathColourCutoff)
            {
                return true;
            }

            return false;
        }
    }
}
