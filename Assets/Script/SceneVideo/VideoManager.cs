using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    private static VideoManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        videoPlayer.loopPointReached += OnLoopPointReached;
    }

    public static void PlayVideo(VideoClip videoClip)
    {
        instance.videoPlayer.clip = videoClip;
        instance.videoPlayer.Play();
        //Debug.Log("PLAAAY");
    }

    public static void StopVideo()
    {
        instance.videoPlayer.Stop();
    }

    public static bool IsVideoPlaying()
    {
        return instance.videoPlayer.isPlaying;
    }


    //Cuando el finalizado cierra el vídeo
    private void OnLoopPointReached(VideoPlayer vp)
    {
        StopVideo();
        gameObject.SetActive(false);
    }
}
