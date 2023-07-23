/*
discription: Manages the UI

Author: Bo
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RubberDucks.KenneyJam.Player;
using TMPro;


namespace RubberDucks.KenneyJam.UI
{
    public class UIManager : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI timerText;

        [Header("Player Information UI Elements")]
        [SerializeField] private List<GameObject> PlayerInfoUI;

        // Start is called before the first frame update
        void Start()
        {
            int i = 0;
            //using a foreach loop because playerlist is dictionary and refering it in for loop is a giant pain.
            foreach (var player in GameManager.GameManager.Instance.PlayerList)
            {
                PlayerInfoUI[i].transform.Find("Icon").GetComponent<Image>().color =
                    player.Value.GetComponent<PlayerController>().PlayerColour;
                i++;
            }
            if (i < 4)
            {
                for (int j = i; j < 4; j++)
                {
                    PlayerInfoUI[j].GetComponent<Image>().color = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
                    PlayerInfoUI[j].transform.Find("Icon").GetComponent<Image>().color = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
                    PlayerInfoUI[j].transform.Find("Score").GetComponent<TextMeshProUGUI>().color = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdateTimerText();
            UpdatePlayerScore();
        }

        void UpdateTimerText()
        {
            float time = GameManager.GameManager.Instance.GameTimer.TimeLeft;
            int seconds = ((int)time % 60);
            int minutes = ((int)time / 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        void UpdatePlayerScore()
        {
            int i = 0;
            foreach (var player in GameManager.GameManager.Instance.PlayerList)
            {
                PlayerInfoUI[i].transform.Find("Score").GetComponent<TextMeshProUGUI>().text = player.Value.GetComponent<PlayerController>().Score.ToString();

                //restart the counter
                if (i > GameManager.GameManager.Instance.PlayerList.Count)
                {
                    i = 0;
                }
                else
                {
                    i++;
                }
            }

        }
    }
}

