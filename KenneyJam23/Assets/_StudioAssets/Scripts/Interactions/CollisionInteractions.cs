/*
 * CollisionInteractions.cs
 * 
 * Description:
 * - 
 * 
 * Author(s): 
 * - Alain
 * - Kody Wood
*/

using RubberDucks.KenneyJam.Zones;

using UnityEngine;
using UnityEngine.Events;

using System.Collections.Generic;

using RubberDucks.Utilities.Audio;
using RubberDucks.KenneyJam.Player;
using RubberDucks.Utilities.Timing;

namespace RubberDucks.KenneyJam.Interactions
{
    public class CollisionInteractions : MonoBehaviour
    {
        //--- Properties ---//

        //--- Public Variables ---//
        public bool m_IsCarrying = false;

        //--- Protected Variables ---//

        //--- Private Variables ---//
        [SerializeField] private List<Pickup> m_CurrentPickup;
        [SerializeField] private PlayerController m_PlayerController = default;
        [SerializeField] private PlayerInvincibility m_PlayerInvincibility = default;
        [SerializeField] private Timer m_PickupHeldTimer = default;
        [SerializeField] private GameObject m_HoldingTreasureIndicator = default;

        //--- Unity Methods ---//
        private void Start()
        {
            m_HoldingTreasureIndicator.transform.parent = null;
        }

        private void Update()
        {
            if (m_IsCarrying && m_CurrentPickup.Count != 0 && m_CurrentPickup[0].CarryPointsRemaining != 0)
            {
                m_PickupHeldTimer.UpdateTimer(Time.deltaTime);
            }

            m_HoldingTreasureIndicator.transform.position = transform.position;
            m_HoldingTreasureIndicator.SetActive(m_IsCarrying);
        }

        //--- Public Methods ---//

        //--- Protected Methods ---//

        //--- Private Methods ---//
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Drop"))
            {
                if (m_CurrentPickup.Count > 0)
                {
                    AudioManager.Instance.PlayOneShotAudio(AudioConstant.SFX_DropOff);
                    IncrementScoreDropOff(m_CurrentPickup[0].DropOffPoints + m_CurrentPickup[0].CarryPointsRemaining);
                    Destroy(m_CurrentPickup[0]);

                    m_CurrentPickup.Clear();

                    m_PickupHeldTimer.m_Events.OnFinished.RemoveListener(IncrementScore);
                    m_PickupHeldTimer.StopTimer();

                    other.gameObject.GetComponent<Zone>().CollideWithZone(m_IsCarrying);
                }

                m_IsCarrying = false;
                m_PlayerController.TryTransform(false);
            }
            else if (other.gameObject.CompareTag("Collect"))
            {
                AudioManager.Instance.PlayOneShotAudio(AudioConstant.SFX_PickupZone);
                other.gameObject.GetComponent<Zone>().CollideWithZone(m_IsCarrying);
            }

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                AudioManager.Instance.PlayOneShotAudio(AudioConstant.SFX_Crash);
                Debug.Log("Player Collision");
                //DropItem(false, collision.gameObject);
                if (m_CurrentPickup.Count > 0 && collision.rigidbody.velocity.magnitude >= collision.gameObject.GetComponent<PlayerController>().DashThreshold)
                {
                    AudioManager.Instance.PlayOneShotAudio(AudioConstant.SFX_LosePickup);
                    m_CurrentPickup[0].GetComponent<Pickup>().UpdatePickupStatus();
                    m_CurrentPickup.Clear();

                    m_PickupHeldTimer.m_Events.OnFinished.RemoveListener(IncrementScore);
                    m_PickupHeldTimer.StopTimer();

                    m_IsCarrying = false;

                    m_PlayerController.TryTransform(false);
                }
            }
            else if (collision.gameObject.CompareTag("Pickup"))
            {
                //CollectItem(false, collision.gameObject);
                AudioManager.Instance.PlayOneShotAudio(AudioConstant.SFX_Pickup);
                collision.gameObject.GetComponent<Pickup>().UpdatePickupStatus();
                collision.gameObject.transform.SetParent(transform);

                m_CurrentPickup.Add(collision.gameObject.GetComponent<Pickup>());

                m_PickupHeldTimer.StartTimer();
                m_PickupHeldTimer.m_Events.OnFinished.AddListener(IncrementScore);
                m_IsCarrying = true;

                m_PlayerController.TryTransform(true);
                m_PlayerInvincibility.TriggerInvincibility();
            }
        }

        private void IncrementScore()
        {
            m_PlayerController.Score += 1;
            m_PlayerController.Events.OnPlayerScoreChange.Invoke();
            m_CurrentPickup[0].UpdateCarryPoints(1);
        }
        private void IncrementScoreDropOff(int value)
        {
            m_PlayerController.Score += value;
            m_PlayerController.Events.OnPlayerScoreChange.Invoke();
        }
    }
}
