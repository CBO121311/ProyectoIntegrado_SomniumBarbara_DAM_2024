using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("LoadLevelSelection", 1f);
            GameController.instance.DeactivateReaper();
            Debug.Log("Enhorabuena, PassLevelTocado");
        }
    }

    private void LoadLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }

}
