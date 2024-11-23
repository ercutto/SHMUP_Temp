
using UnityEngine;

public class MedalsMenu : Menu
{
    public static MedalsMenu instance = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Medals Menu! ");
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
