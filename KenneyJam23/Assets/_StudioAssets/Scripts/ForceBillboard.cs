/*
 * ForceBillboard.cs
 * 
 * Description:
 * - 
 * 
 * Author(s): 
 * - 
*/

using UnityEngine;
using UnityEngine.Events;

namespace RubberDucks.ProjectName.Module
{
	public class ForceBillboard : MonoBehaviour
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
        private void Update()
        {
            this.transform.up = Vector3.up;
        }

        //--- Public Methods ---//

        //--- Protected Methods ---//

        //--- Private Methods ---//
    }
}
