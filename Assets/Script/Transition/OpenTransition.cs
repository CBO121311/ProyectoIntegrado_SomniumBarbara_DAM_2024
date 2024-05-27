using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenTransition : MonoBehaviour
{
    [SerializeField] private GameObject groupLogo;
    [SerializeField] private GameObject ringLogo;
    [SerializeField] private GameObject imageLogo;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip boingClip;
    [SerializeField] private AudioClip finalClip;

    [SerializeField] private GameObject fadeGameobject;
    private Image fadeImage;

    private void Awake()
    {
        if (!fadeGameobject.activeSelf)
        {
            fadeGameobject.SetActive(true);
        }

        fadeImage = fadeGameobject.GetComponent<Image>();
    }

    void Start()
    {
        FadeIn();
        LeanTween.rotateAroundLocal(ringLogo.GetComponent<RectTransform>(), Vector3.forward, 720f, 2.5f);
    }

    public void FadeIn()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
        LeanTween.alpha(fadeImage.rectTransform, 0f, 1f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                LeanTween.rotateAroundLocal(ringLogo.GetComponent<RectTransform>(), Vector3.forward, 720f, 2.5f);
                MoveUp();
            });
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        LeanTween.alpha(fadeImage.rectTransform, 1f, 1f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                SceneManager.LoadScene(sceneName);
            }).setDelay(0.8f);
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
                FadeOutAndLoadScene("MainMenuUI");
            });
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
