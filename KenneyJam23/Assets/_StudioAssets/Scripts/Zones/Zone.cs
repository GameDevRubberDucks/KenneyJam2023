/*
 * Zone.cs
 * 
 * Description:
 * - 
 * 
 * Author(s): 
 * - 
*/

using RubberDucks.KenneyJam.Zones;

using UnityEngine;
using UnityEngine.Events;

namespace RubberDucks.KenneyJam.Zones
{
	public class Zone : MonoBehaviour
	{
		//--- Properties ---//

		//--- Public Variables ---//

		//--- Protected Variables ---//

		//--- Private Variables ---//
		[SerializeField] private bool m_IsDrop = true;

		[SerializeField] private Spawning m_ZoneSpawner;

        //--- Unity Methods ---//
        public void Start()
        {
			m_ZoneSpawner = GameObject.FindObjectOfType<Spawning>();
        }

        //--- Public Methods ---//
        public void CollideWithZone()
		{
			if (m_IsDrop)
			{
				//Add function to increase score here

				m_ZoneSpawner.SpawnCollectZone();
			}
			else
			{
				m_ZoneSpawner.SpawnDropZone();
			}
		}

		//--- Protected Methods ---//

		//--- Private Methods ---//
	}
}
