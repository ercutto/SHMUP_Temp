
using UnityEngine;

public class PracticeStageMenu : Menu
{
    public static PracticeStageMenu instance = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Practice Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
