/*
 * LevelPathCutter.cs
 * 
 * Description:
 * - System that allows the player to carve a path through the jungle
 * 
 * Author(s): 
 * - Dan
*/

using RubberDucks.KenneyJam.Interactions;
using RubberDucks.KenneyJam.Player;
using UnityEngine;
using UnityEngine.Events;

namespace RubberDucks.KenneyJam.Level
{
	public class LevelPathCutter : MonoBehaviour
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
        [SerializeField] private PlayerController m_PlayerController = default;
        [SerializeField] private CollisionInteractions m_CollisionInteractions = default;

        private bool m_CanCutTrees = true;

        //--- Unity Methods ---//
        private void Update()
        {
            // TODO: Add an event for picking up so we don't need to poll constantly
            if (m_CollisionInteractions != null)
            {
                m_CanCutTrees = !m_CollisionInteractions.m_IsCarrying;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_CanCutTrees)
            {
                if (other.TryGetComponent<LevelForestCollider>(out LevelForestCollider forestCollider))
                {
                    forestCollider.ClearForest();

                    if (m_PlayerController != null)
                    {
                        m_PlayerController.IsCuttingTrees = true;
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (m_PlayerController != null)
            {
                if (other.TryGetComponent<LevelForestCollider>(out LevelForestCollider forestCollider))
                {
                    m_PlayerController.IsCuttingTrees = false;
                }
            }
        }

        //--- Public Methods ---//

        //--- Protected Methods ---//

        //--- Private Methods ---//
    }
}
