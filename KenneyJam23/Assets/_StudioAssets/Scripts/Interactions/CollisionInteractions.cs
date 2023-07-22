/*
 * CollisionInteractions.cs
 * 
 * Description:
 * - 
 * 
 * Author(s): 
 * - 
*/

using UnityEngine;
using UnityEngine.Events;

using System.Collections;

namespace RubberDucks.KenneyJam.Interactions
{
    public class CollisionInteractions : MonoBehaviour
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
        [SerializeField] private GameObject m_CurrentPickup;

        //--- Unity Methods ---//

        //--- Public Methods ---//

        //--- Protected Methods ---//

        //--- Private Methods ---//
        private void OnTriggerEnter(Collider other)
        {
            //Collision checks for entering drop and collect zones
            if (other.gameObject.CompareTag("Drop"))
            {
                DropItem(true, other.gameObject);
            }
            else if (other.gameObject.CompareTag("Collect"))
            {
                CollectItem(true, other.gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                DropItem(false, collision.gameObject);
            }
            else if (collision.gameObject.CompareTag("Pickup"))
            {
                CollectItem(false, collision.gameObject);
            }
        }

        private void DropItem(bool isZone, GameObject zone)
        {
            //Dropping into a drop off zone
            if (isZone)
            {
                if (transform.childCount > 0)
                {
                    Destroy(m_CurrentPickup.gameObject);
                    m_CurrentPickup = null;
                }
            }
            //Dropping from player interaction
            else
            {
                m_CurrentPickup.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
                m_CurrentPickup.SetActive(true);
                m_CurrentPickup.transform.parent = null;

                m_CurrentPickup = null;
            }
        }

        private void CollectItem(bool isZone, GameObject zone)
        {
            if (isZone)
            {
                if (zone.transform.childCount > 0)
                {
                    m_CurrentPickup = zone.transform.GetChild(0).gameObject;

                    m_CurrentPickup.transform.SetParent(this.transform);
                }
            }
            else
            {
                zone.transform.SetParent(this.transform);

                m_CurrentPickup = zone;
                m_CurrentPickup.SetActive(false);
            }

        }
    }
}
