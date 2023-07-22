/*
 * Pickup.cs
 * 
 * Description:
 * - 
 * 
 * Author(s): 
 * - 
*/

using UnityEngine;
using UnityEngine.Events;

namespace RubberDucks.KenneyJam.Interactions
{
	public class Pickup : MonoBehaviour
	{
		//--- Properties ---//

		//--- Public Variables ---//

		//--- Protected Variables ---//

		//--- Private Variables ---//
		[SerializeField] private bool m_IsCarried = false;

		[SerializeField] private int m_ResetHeight;
		
		//--- Unity Methods ---//
		
		//--- Public Methods ---//
		public void UpdatePickupStatus()
		{
			if (m_IsCarried)
			{
				ResetToActive();
			}
			else
			{
				ResetToCarryDefault();
			}
		}

		//--- Protected Methods ---//

		//--- Private Methods ---//
		private void ResetToCarryDefault()
		{
			transform.localPosition = Vector3.up * m_ResetHeight;

			m_IsCarried = true;
			gameObject.SetActive(false);
		}

		private void ResetToActive()
		{
			transform.parent = null;

			m_IsCarried = false;
			gameObject.SetActive(true);
			
			this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Force);
		}
	}
}
