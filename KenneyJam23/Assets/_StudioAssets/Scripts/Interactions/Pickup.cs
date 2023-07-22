/*
 * Pickup.cs
 * 
 * Description:
 * - 
 * 
 * Author(s): 
 * - Alain
 * - Kody Wood
*/

using RubberDucks.KenneyJam.Player;
using UnityEngine;
using UnityEngine.Events;

namespace RubberDucks.KenneyJam.Interactions
{
	public class Pickup : MonoBehaviour
	{
        //--- Properties ---//
		public int DropOffPoints
		{
			get => m_DropOffPoints;
		}
        public int CarryPointsRemaining
        {
            get => m_CarryPointsRemaining;
        }
        //--- Public Variables ---//

        //--- Protected Variables ---//

        //--- Private Variables ---//
        [SerializeField] private bool m_IsCarried = false;
		[SerializeField] private int m_ResetHeight;

		[Header("Pick Up Score Variables")]
		[SerializeField] private int m_MaxCarryPoints = 10;
		[SerializeField] private int m_CarryPointsRemaining = 10;
		[SerializeField] private int m_DropOffPoints = 20;

        [SerializeField] private float m_SecondsToCarryScore = 3.0f;
        private float m_CarryScoreTimer = 3.0f;

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

        public void UpdateCarryPoints(int value)
        {
            m_CarryPointsRemaining -= value;
        }

        //--- Protected Methods ---//

        //--- Private Methods ---//
        private void ResetToCarryDefault()
		{
			//transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			//transform.localPosition = Vector3.up * m_ResetHeight;

			m_IsCarried = true;
			gameObject.SetActive(false);
		}

		private void ResetToActive()
		{
			transform.parent = null;

			transform.localPosition += (Vector3.up * m_ResetHeight);

			m_IsCarried = false;
			gameObject.SetActive(true);

			this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Force);
		}

		
	}
}
