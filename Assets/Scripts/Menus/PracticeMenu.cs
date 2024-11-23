
using UnityEngine;

public class PracticeMenu : Menu
{
    public static PracticeMenu instance = null;

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Practice Stage Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    public void OnBackButton()
    {
        TurnOff(true); //Simdi bu menuyu kapatiyoruz ve bir oncekine donuyoruz
    }
}
