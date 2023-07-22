/*
 * LevelPathCutter.cs
 * 
 * Description:
 * - System that allows the player to carve a path through the jungle
 * 
 * Author(s): 
 * - Dan
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RubberDucks.KenneyJam.Level
{
    public class LevelForestCollider : MonoBehaviour
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

        //--- Public Methods ---//
        public void ClearForest()
        {
            // TODO: Find the trees that are tied to this object and delete them
            Destroy(this.gameObject);
        }

        public void RegisterTreeAsChild(GameObject tree)
        {
            tree.transform.parent = this.transform;
        }

        //--- Protected Methods ---//

        //--- Private Methods ---//
    }
}
