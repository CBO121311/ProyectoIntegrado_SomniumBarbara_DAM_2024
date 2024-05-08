using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenTransition : MonoBehaviour
{
    [SerializeField] private GameObject groupLogo;
    [SerializeField] private GameObject ringLogo;
    [SerializeField] private GameObject imageLogo;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip boingClip;
    [SerializeField] private AudioClip finalClip;

    void Start()
    {
        LeanTween.rotateAroundLocal(ringLogo.GetComponent
           <RectTransform>(), Vector3.forward, 720f, 2.5f);
        MoveUp();
    }

    void MoveUp()
    {
        LeanTween.move(imageLogo.GetComponent<RectTransform>(), new Vector3(0,449,0), 0.3f)
            .setOnComplete(() =>
            {
                PlayBoingSound();

                LeanTween.scaleY(imageLogo, 0.3f, 0.1f)
                 .setOnComplete(() =>
                 {
                     LeanTween.scaleY(imageLogo, 0.5f, 0.1f);
                     MoveRight();
                 });
            }).setDelay(0.4f);

    }

    void MoveRight()
    {
        LeanTween.move(imageLogo.GetComponent<RectTransform>(), new Vector3(886, 0, 0), 0.3f)
          .setOnComplete(() =>
          {
              PlayBoingSound();
           
              LeanTween.scaleX(imageLogo, 0.3f, 0.1f)
              .setOnComplete(() =>
              {
                  LeanTween.scaleX(imageLogo, 0.5f, 0.1f);
                  MoveDown();
              });
          });
    }

    void MoveDown()
    {
        LeanTween.move(imageLogo.GetComponent<RectTransform>(), new Vector3(0, -448, 0), 0.3f)
            .setOnComplete(() =>
            {
                PlayBoingSound();
                
                LeanTween.scaleY(imageLogo, 0.3f, 0.1f)
                   .setOnComplete(() => {
                       LeanTween.scaleY(imageLogo, 0.5f, 0.1f);
                       MoveCenter();
                   });
            });
    }

    void MoveCenter()
    {
        LeanTween.scale(imageLogo.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 0.5f);

        LeanTween.move(imageLogo.GetComponent<RectTransform>(), Vector3.zero, 1f)
            .setEaseOutQuad()       
            .setOnComplete(() =>
            {
                
                PlayFinalSound();
                FadeOutCanvas();
            });
    }


    void FadeOutCanvas()
    {
        LeanTween.alpha(groupLogo.GetComponent<RectTransform>(), 0f, 1f)
            .setOnComplete(() =>
            {
                SceneManager.LoadScene("MainMenuUI");
            }).setDelay(0.8f);
    }

    // Método para reproducir el sonido del bote
    void PlayBoingSound()
    {
        if (audioSource != null && boingClip != null)
        {
            audioSource.clip = boingClip;
            audioSource.Play();
        }
    }

    // Método para reproducir el sonido final
    void PlayFinalSound()
    {
        if (audioSource != null && finalClip != null)
        {
            audioSource.clip = finalClip;
            audioSource.Play();
        }
    }
}
