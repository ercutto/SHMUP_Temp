
using UnityEngine;

public class PracticeArenaMenu : Menu
{
    public static PracticeArenaMenu instance = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Practice Arena Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
