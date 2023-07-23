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
using RubberDucks.KenneyJam.Interactions;

using UnityEngine;
using UnityEngine.Events;
using RubberDucks.Utilities.Timing;

namespace RubberDucks.KenneyJam.Zones
{
	public class Zone : MonoBehaviour
	{
		//--- Properties ---//

		//--- Public Variables ---//

		//--- Protected Variables ---//

		//--- Private Variables ---//
		[SerializeField] private Pickup m_Pickup;
		[SerializeField] private bool m_IsDrop = true;

		[SerializeField] private Spawning m_ZoneSpawner;

		[Header("Ping System")]
		[SerializeField] private AutomaticTimer m_PingTimer = default;
		[SerializeField] private ParticleSystem m_PingParticles = default;

		//--- Unity Methods ---//
		public void Start()
		{
			m_ZoneSpawner = GameObject.FindObjectOfType<Spawning>();

			m_PingTimer.m_Events.OnFinished.AddListener(OnPingTimerDone);
		}

		//--- Public Methods ---//
		public void CollideWithZone(bool isCarrying)
		{
			if (m_IsDrop && isCarrying)
			{
				//Add function to increase score here

				m_ZoneSpawner.SpawnCollectZone();
			}
			else
			{
				m_ZoneSpawner.SpawnDropZone();

				Vector3 spawnPos = transform.position + (5.0f * Vector3.up);
				Instantiate(m_Pickup, spawnPos, Quaternion.identity);
			}
		}

		//--- Protected Methods ---//

		//--- Private Methods ---//
		private void OnPingTimerDone()
		{
			m_PingParticles.Play();	
		}
	}
}
