/*
 * LevelPathCutter.cs
 * 
 * Description:
 * - System that allows the player to carve a path through the jungle
 * 
 * Author(s): 
 * - Dan
*/

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

        //--- Unity Methods ---//
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<LevelForestCollider>(out LevelForestCollider forestCollider))
            {
                forestCollider.ClearForest();
            }
        }

        //--- Public Methods ---//

        //--- Protected Methods ---//

        //--- Private Methods ---//
    }
}
