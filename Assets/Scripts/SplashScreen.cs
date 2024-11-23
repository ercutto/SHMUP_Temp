
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreen : MonoBehaviour
{
    

    public VideoPlayer player = null;
    void Start()
    {
        player.loopPointReached += EndReached;

    }

    private void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene("MainMenusScene");
    }
}
