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
        if (transform.childCount == 0)
        {
            Debug.Log("No quedan artículos");
            transition.SetActive(true);
            Invoke("ChangeScene", 1);
        }
    }

    void ChangeScene()
    {
        Debug.Log("Enhorabuena, te lo has pasado");
        //SceneManager.LoadScene("SelectionLevel");
    }
}
