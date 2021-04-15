using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using IndieMarc.StealthLOS;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    public Transform mainCamera;

    public GameObject toolBarWhole;

    public CharacterControls2D PlayerMovement;

    public Canvas Jiesuan;

    public AudioSource audioSourceUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        mainCamera.DOMove(new Vector3(-0.86f, -0.18f,-10), 2f).SetEase(Ease.OutCubic);
        toolBarWhole.SetActive(true);
        PlayerMovement.enabled = true;
    }
    
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
        /*
        mainCamera.DOMove(new Vector3(-11.42f, 0,-10), 2f).SetEase(Ease.OutCubic);
        toolBarWhole.SetActive(false);
        PlayerMovement.enabled = false;
        Jiesuan.enabled = false;
        */
    }

    public void PlayAudio()
    {
        audioSourceUI.Play();
    }
}