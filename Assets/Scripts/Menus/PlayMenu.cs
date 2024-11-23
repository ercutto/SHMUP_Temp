
using UnityEngine;

public class PlayMenu : Menu
{
    public static PlayMenu instance = null;
    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Play Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void OnNormalButton()
    {
        TurnOff(false);//eger sadece sayfa icinde islem ise false yapip bu sayfanin kapanmasini engeller
        CraftSelectMenu.instance.TurnOn(this);//bu menuyu istedigimiz icin 
    }
    public void OnBullHellButton()
    {
        TurnOff(false);//eger sadece sayfa icinde islem ise false yapip bu sayfanin kapanmasini engeller
        CraftSelectMenu.instance.TurnOn(this);//bu menuyu istedigimiz icin 
    }
    public void OnBackButton()
    {
        TurnOff(true); //Simdi bu menuyu kapatiyoruz ve bir oncekine donuyoruz
    }
}
