using UnityEngine;
using UnityEngine.SceneManagement;

public class Fixed : MonoBehaviour {

    public Animator animator;

    private int levelToLoad;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("r"))
        {
            FadeToLevel(4);
        }
    }

    public void FadeToNextLevel()
    {
        SceneManager.LoadScene("Dungeon_V2");
    }

    public void FadeToLevel (int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
        
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
