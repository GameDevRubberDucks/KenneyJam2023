/*
 * CollisionInteractions.cs
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

using System.Collections.Generic;

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

        //--- Unity Methods ---//

        //--- Public Methods ---//

        //--- Protected Methods ---//

        //--- Private Methods ---//
        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<Zone>().CollideWithZone();

            if (other.gameObject.CompareTag("Drop"))
            {
                Destroy(m_CurrentPickup[0]);
                m_CurrentPickup.Clear();

                m_IsCarrying = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //DropItem(false, collision.gameObject);
                if (m_CurrentPickup.Count > 0)
                {
                    m_CurrentPickup[0].GetComponent<Pickup>().UpdatePickupStatus();
                    m_CurrentPickup.Clear();

                    m_IsCarrying = false;
                }          
            }
            else if (collision.gameObject.CompareTag("Pickup"))
            {
                //CollectItem(false, collision.gameObject);
                collision.gameObject.GetComponent<Pickup>().UpdatePickupStatus();
                collision.gameObject.transform.SetParent(transform);

                m_CurrentPickup.Add(collision.gameObject.GetComponent<Pickup>());
                m_IsCarrying = true;
            }
        }
    }
}
