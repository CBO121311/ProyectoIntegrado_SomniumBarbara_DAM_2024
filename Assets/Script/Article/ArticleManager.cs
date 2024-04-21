using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArticleManager : MonoBehaviour
{
    public GameObject transition;

    private void Update()
    {
        AllArticleCollected();
    }

    public void AllArticleCollected()
    {
        if(transform.childCount == 0)
        {
            Debug.Log("No quedan artículos");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
            //levelCreared.gameObject.SetActive(true);
            transition.SetActive(true);
            Invoke("ChangeScene", 1); //Lo invoca un segundo después
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("SelectionLevel");
    }
}
