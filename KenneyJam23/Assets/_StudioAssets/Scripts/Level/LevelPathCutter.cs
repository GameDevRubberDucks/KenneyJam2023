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
        public bool CanCutTrees
        {
            get => m_CanCutTrees;
            set => m_CanCutTrees = value;
        }

        //--- Public Variables ---//

        //--- Protected Variables ---//

        //--- Private Variables ---//
        [SerializeField] private PlayerController m_PlayerController = default;
        [SerializeField] private CollisionInteractions m_CollisionInteractions = default;
        [SerializeField] private ParticleSystem m_TreeDestroyPS = default;

        private bool m_CanCutTrees = false;

        //--- Unity Methods ---//
        private void OnTriggerEnter(Collider other)
        {
            if (m_CanCutTrees)
            {
                if (other.TryGetComponent<LevelForestCollider>(out LevelForestCollider forestCollider))
                {
                    forestCollider.ClearForest();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (m_CanCutTrees)
            {
                if (other.TryGetComponent<LevelForestCollider>(out LevelForestCollider forestCollider))
                {
                    Instantiate(m_TreeDestroyPS, other.transform.position + new Vector3(0.0f,7.0f,0.0f), Quaternion.Euler(90.0f,0.0f,0.0f)).Play();
                    forestCollider.ClearForest();
                }
            }
        }

        //--- Public Methods ---//

        //--- Protected Methods ---//

        //--- Private Methods ---//
    }
}
