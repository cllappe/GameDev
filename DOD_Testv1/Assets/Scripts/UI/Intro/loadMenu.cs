using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadMenu : MonoBehaviour
{
    //public int index;
    public string levelName;
    public Image black;
    public Animator animator;

    void Update()
    {
        if (Input.anyKey)
        {
            StartCoroutine(Fading());
           // SceneManager.LoadScene("MainMenu");

        }
    }

    IEnumerator Fading()
    {
        animator.SetBool("Fade", true);
        yield return new WaitUntil(() => Mathf.Approximately(black.color.a, 1));
        //SceneManager.LoadScene(index);
        SceneManager.LoadScene("MainMenu");
    }

}