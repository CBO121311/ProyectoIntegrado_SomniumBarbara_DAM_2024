using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassLevel : MonoBehaviour
{
    [SerializeField] private GameObject passArea;
    [SerializeField] private GameObject enemyObstacle;
    private Animator enemyObstAnimator;
    private void Awake()
    {
        if (passArea.activeSelf)
        {
            passArea.SetActive(false);
        }
    }

    private void Start()
    {
        enemyObstAnimator = enemyObstacle.GetComponent<Animator>();
    }

    public void CompleteObjective()
    {
        if (!passArea.activeSelf)
        {
            passArea.SetActive(true);
            enemyObstAnimator.SetTrigger("passLevel");
            Invoke("DeactivateEnemyObstacle", 0.3f);
        }
    }


    //Desactiva al enemigo
    private void DeactivateEnemyObstacle()
    {
        enemyObstacle.SetActive(false);
    }
}
