using System.Collections;
using System.Collections.Generic;
using RubberDucks.KenneyJam.GameManager;
using RubberDucks.Utilities.Audio;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.StopLoopingAudio("BGM");
        AudioManager.Instance.PlayLoopingAudio("Menu", AudioConstant.MUSIC_Menu);
    }

    public void SetPlayerCount(int count)
    {
        GameManager.m_NumPlayers = count;
    }
}
