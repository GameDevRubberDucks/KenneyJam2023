/*
discription: Manages the UI

Author: Bo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RubberDucks.KenneyJam;
using TMPro;

namespace RubberDucks.KenneyJam.UI
{
    public class UIManager : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI timerText;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateTimerText();
        }

        void UpdateTimerText()
        {
            float time = GameManager.GameManager.Instance.GameTimer.TimeLeft;
            int seconds = ((int)time % 60);
            int minutes = ((int)time / 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
