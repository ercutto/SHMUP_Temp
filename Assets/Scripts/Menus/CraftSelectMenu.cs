
using UnityEngine;
using UnityEngine.UI;

public class CraftSelectMenu : Menu
{
    public static CraftSelectMenu instance = null;

    public Image player1ShipA = null;
    public Image player1ShipB = null;
    public Image player1ShipC = null;
    public Image player1ShipX = null;
    public Image player1ShipZ = null;
    
    public Image player2ShipA = null;
    public Image player2ShipB = null;
    public Image player2ShipC = null;
    public Image player2ShipX = null;
    public Image player2ShipZ = null;

    public Slider power_Slider_1 = null;
    public Slider speed_Slider_1 = null;
    public Slider beam_Slider_1 = null;
    public Slider bomb_Slider_1 = null;
    public Slider options_Slider_1 = null;

    public Slider power_Slider_2 = null;
    public Slider speed_Slider_2 = null;
    public Slider beam_Slider_2 = null;
    public Slider bomb_Slider_2 = null;
    public Slider options_Slider_2 = null;

    public Text countDownText = null;
    
    public GameObject player2Panel = null;
    public GameObject countDownOBj = null;

    public Text playerTwoStartPanel = null;

    private float lastUnScaledTime = 0;
    private float timer = 5.9f;
    private bool countDown = false;

    //Ship Selection
    private int selectedShip1 = 0;
    private int selectedShip2 = 0;

    public Sprite[] shpsSprites=new Sprite[5];
    public Sprite[] shpsSpritesSelected=new Sprite[5];
    public Sprite[] shpsSpritesDisAbled=new Sprite[5];

    public CraftConfiguration[] crafts =new CraftConfiguration[5];

    private void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one Craaft Select Menu! ");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Reset()
    {

        playerTwoStartPanel.gameObject.SetActive(true);
        player2Panel.SetActive(false);
        GameManager.Instance.twoPlayer=false;


        countDownOBj.SetActive(false);
        countDown = false;
        timer = 5.9f;
        UpdateShipSelection();
    }

    public override void TurnOn(Menu previous)
    {
        base.TurnOn(previous);
        Reset();
    }

    public void FixedUpdate()
    {
        if (InputManager.instance.playerState[0].shoot)
            StartCountDown();

        if (InputManager.instance.playerState[1].shoot)
        {
            playerTwoStartPanel.gameObject.SetActive(false);
            player2Panel.gameObject.SetActive(true);
            GameManager.Instance.twoPlayer=true;
            HUD.Instance.TurnOnP2(true);
            UpdateShipSelection();
            StopCountDown();
        }
           

        if (!InputManager.instance.playerPrevState[0].left&& InputManager.instance.playerState[0].left)
        {
            if (selectedShip1 > 0)
                selectedShip1--;
            UpdateShipSelection();
        }

        if (!InputManager.instance.playerPrevState[0].right && InputManager.instance.playerState[0].right)
        {
            if (selectedShip1 <2)
                selectedShip1++;
            UpdateShipSelection();
        }

        if (!InputManager.instance.playerPrevState[1].left && InputManager.instance.playerState[1].left)
        {
            if (selectedShip2 > 0)
                selectedShip2--;
            UpdateShipSelection();
        }

        if (!InputManager.instance.playerPrevState[1].right && InputManager.instance.playerState[1].right)
        {
            if (selectedShip2 < 2)
                selectedShip2++;
            UpdateShipSelection();
        }


        if (countDown)
        {
            float dUnScaled=Time.unscaledTime-lastUnScaledTime;
            lastUnScaledTime= Time.unscaledTime;
            timer-=dUnScaled;
            countDownText.text= ((int)timer).ToString();
            if(timer<1)
                GameManager.Instance.StartGame();
        }
    }

    public void UpdateShipSelection()
    {        
        player1ShipA.sprite=shpsSprites[0];
        player1ShipB.sprite=shpsSprites[1];
        player1ShipC.sprite=shpsSprites[2];
        player1ShipX.sprite= shpsSpritesDisAbled[3];
        player1ShipZ.sprite= shpsSpritesDisAbled[4];

        if (selectedShip1 == 0)
            player1ShipA.sprite = shpsSpritesSelected[0];
        else if (selectedShip1 == 1)
            player1ShipB.sprite = shpsSpritesSelected[1];
        else if (selectedShip1 == 2)
            player1ShipC.sprite = shpsSpritesSelected[2];
        else if (selectedShip1 == 3)
            player1ShipX.sprite = shpsSpritesSelected[3];
        else if (selectedShip1 == 4)
            player1ShipZ.sprite = shpsSpritesSelected[4];

        CraftConfiguration config1 = crafts[selectedShip1];

        speed_Slider_1.value = config1.speed;
        power_Slider_1.value =config1.bulletStrength;
        beam_Slider_1.value=config1.beamPower;
        bomb_Slider_1.value = config1.bombPower;
        options_Slider_1.value=config1.optionPower;

        if (GameManager.Instance.twoPlayer)
        {
            player2ShipA.sprite = shpsSprites[0];
            player2ShipB.sprite = shpsSprites[1];
            player2ShipC.sprite = shpsSprites[2];
            player2ShipX.sprite = shpsSpritesDisAbled[3];
            player2ShipZ.sprite = shpsSpritesDisAbled[4];

            if (selectedShip2 == 0)
                player2ShipA.sprite = shpsSpritesSelected[0];
            else if (selectedShip2 == 1)
                player2ShipB.sprite = shpsSpritesSelected[1];
            else if (selectedShip2 == 2)
                player2ShipC.sprite = shpsSpritesSelected[2];
            else if (selectedShip2 == 3)
                player2ShipX.sprite = shpsSpritesSelected[3];
            else if (selectedShip2 == 4)
                player2ShipZ.sprite = shpsSpritesSelected[4];

            CraftConfiguration config2 = crafts[selectedShip2];

            speed_Slider_2.value = config2.speed;
            power_Slider_2.value = config2.bulletStrength;
            beam_Slider_2.value = config2.beamPower;
            bomb_Slider_2.value = config2.bombPower;
            options_Slider_2.value = config2.optionPower;
        }
    }
    public void OnBackButton()
    {
        TurnOff(true); //Simdi bu menuyu kapatiyoruz ve bir oncekine donuyoruz
    }
    public void OnPlayButton()
    {
        StartCountDown();
    }
    private void StartCountDown()
    {
        timer = 5.9f;
        lastUnScaledTime=Time.unscaledTime;
        countDown = true;
        countDownOBj.SetActive(true);

    }
    private void StopCountDown()
    {
        countDown = false;
        countDownOBj.SetActive(false);
    }
    public void OnCraftA_P1Button()
    {

    }
    public void OnCraftB_P1Button()
    {

    }
    public void OnCraftC_P1Button()
    {

    }
}
