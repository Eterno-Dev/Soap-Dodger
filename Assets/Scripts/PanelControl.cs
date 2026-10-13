using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelControl : MonoBehaviour
{
    [SerializeField]
    GameObject panel;
    public AudioClip musica;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musica;
    }
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    public void Play()
    {
        panel.SetActive(false);
        SceneManager.LoadScene("SampleScene");
    }
    
    public void ExitGame()
    {
        panel.SetActive(false);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void HowToPlay()
    {
        panel.SetActive(true);
    }
}
