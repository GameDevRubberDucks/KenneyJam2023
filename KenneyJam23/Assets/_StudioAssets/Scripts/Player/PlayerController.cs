/*
 * PlayerController.cs
 * 
 * Description:
 * - Controls for the player objects
 * 
 * Author(s): 
 * - Kody Wood
*/


using UnityEngine;
using RubberDucks.KenneyJam.Interactions;

namespace RubberDucks.KenneyJam.Player
{
    public class PlayerController : MonoBehaviour
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
        [SerializeField] private float m_Acceleration = 300.0f;
        private Vector3 m_LastLookDir = Vector3.forward;

        private GameObject teasure;

        //--- Unity Methods ---//


        private void Update()
        {
            Vector3 m_velDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                m_LastLookDir = m_velDir;
            }
            this.GetComponent<Rigidbody>().velocity += m_Acceleration * m_velDir * Time.deltaTime;



            if (Input.GetKey(KeyCode.I))
            {
                this.GetComponent<Rigidbody>().velocity = m_Acceleration / 50.0f * Vector3.forward;
            }
            if (Input.GetKey(KeyCode.K))
            {
                this.GetComponent<Rigidbody>().velocity = m_Acceleration / 50.0f * Vector3.back;
            }
            if (Input.GetKey(KeyCode.L))
            {
                this.GetComponent<Rigidbody>().velocity = m_Acceleration / 50.0f * Vector3.right;
            }
            if (Input.GetKey(KeyCode.J))
            {
                this.GetComponent<Rigidbody>().velocity = m_Acceleration / 50.0f * Vector3.left;
            }

            UpdateRotation(m_LastLookDir);
        }

        //--- Public Methods ---//

        //--- Protected Methods ---//

        //--- Private Methods ---//
        void UpdateRotation(Vector3 lookAt)
        {
            this.transform.LookAt(this.transform.position + lookAt);
        }

        void WayFinder()
        {

            teasure = GameObject.FindGameObjectWithTag("Pickup");
        }
    }
}
