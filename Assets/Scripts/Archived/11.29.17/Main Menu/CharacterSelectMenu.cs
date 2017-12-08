using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.UIElements;

public class CharacterSelectMenu : MonoBehaviour
{
    private bool p1Confirmed;
    private bool p2Confirmed;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip navChime;
    [SerializeField]
    private AudioClip navDeselect;
    [SerializeField]
    private AudioClip navConfirm;
    [SerializeField]
    private UnityEngine.UI.Button[] characterSelection;
    [SerializeField]
    private GameObject[] navIcons;
    private int[] maxCharacters;
    [SerializeField]
    private Text[] characterInfo;
    [SerializeField]
    private UnityEngine.UI.Image p2Panel;


    private void Awake()
    {
       audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        p1Confirmed = false;
        p2Confirmed = false;
        if (MainGameManager.Instance.ActivePlayers == 2)
        {
            navIcons[2].active = false;
            p2Panel.color = Color.green;
        }
        else
            navIcons[1].active = false;


        navIcons[0].transform.position = (characterSelection[0].transform.position - new Vector3(125, 0, 0));

        if (MainGameManager.Instance.ActivePlayers == 2)
            navIcons[1].transform.position = (characterSelection[1].transform.position + new Vector3(125, 0, 0));
        else
            navIcons[2].transform.position = (characterSelection[1].transform.position + new Vector3(125, 0, 0));
    }
    private void Update()
    {
        
        if(!p1Confirmed)
            P1Selection();
        if (!p2Confirmed)
            P2Selection();

        if (p1Confirmed || p2Confirmed)
        {
            if (Input.GetButtonDown("Jump1"))
            {
                audioSource.clip = navDeselect;
                audioSource.Play();
                p1Confirmed = false;
            }
            else if (Input.GetButtonDown("Jump2"))
            {
                audioSource.clip = navDeselect;
                audioSource.Play();
                p2Confirmed = false;
            }
        }

        if (p1Confirmed && p2Confirmed)
        {
            if(MainGameManager.Instance.ActivePlayers == 2)
                LoadLevel("Multiplayer");
            else
                LoadLevel("SinglePlayer");

        }
            
    }
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    private void P1Selection()
    {
        
        
            if (P1Horizontal() > 0.0f)
            {
                audioSource.clip = navChime;
                if (navIcons[0].transform.position != characterSelection[1].transform.position - new Vector3(125, 0, 0))
                {
                    audioSource.Play();
                    navIcons[0].transform.position = characterSelection[1].transform.position - new Vector3(125, 0, 0);
                    characterInfo[0].text = "<b><i>Name:</i></b> <b>Dukez</b> \n \n<b><i>Dmg Type:</i></b> <color=#ffff00ff><b> Light</b></color> \n \n<b><i>Hype Attack:</i></b> <b>Palm Blast</b>";
                }
            }
            else if (P1Horizontal() < 0.0f)
            {
                audioSource.clip = navChime;
                if (navIcons[0].transform.position != characterSelection[0].transform.position - new Vector3(125, 0, 0))
                    {
                        audioSource.Play();
                        navIcons[0].transform.position = characterSelection[0].transform.position - new Vector3(125, 0, 0);
                        characterInfo[0].text = "<b><i>Name:</i></b> <b>Marie O'Nett</b> \n \n<b><i>Dmg Type:</i></b> <color=red><b> Heavy</b></color> \n \n<b><i>Hype Attack:</i></b> <b>Tornado Swing</b>";
                }
            }
            else if (P1ConfirmButton())
            {
                audioSource.clip = navConfirm;
                if (navIcons[0].transform.position == (characterSelection[0].transform.position - new Vector3(125, 0, 0)))
                {
                    audioSource.Play();
                    MainGameManager.Instance.Fighters[0] = Fighters.MARIE;
                    p1Confirmed = true;

                }
                else if (navIcons[0].transform.position == (characterSelection[1].transform.position - new Vector3(125, 0, 0)))
                {
                    audioSource.Play();
                    MainGameManager.Instance.Fighters[0] = Fighters.DUKEZ;
                    p1Confirmed = true;
                }
            }
            
    
        
    }
    private void P2Selection()
    {
        if (MainGameManager.Instance.ActivePlayers != 2 && P2ConfirmButton())
        {
                audioSource.clip = navConfirm;
                //change cpu icon to P2
                audioSource.Play();
                navIcons[1].transform.position = navIcons[2].transform.position;
                navIcons[2].active = false;
                navIcons[1].active = true;
                MainGameManager.Instance.ActivePlayers = 2;
                p2Panel.color = Color.green;


        }
        else if(MainGameManager.Instance.ActivePlayers == 2)
        {
            if (P2Horizontal() > 0.0f)
            {
                audioSource.clip = navChime;
                if (navIcons[1].transform.position != characterSelection[1].transform.position)
                {
                    audioSource.Play();
                    navIcons[1].transform.position = characterSelection[1].transform.position + new Vector3(125, 0, 0);
                    characterInfo[1].text = "<b><i>Name:</i></b> <b>Dukez</b> \n \n<b><i>Dmg Type:</i></b> <color=#ffff00ff><b> Light</b></color> \n \n<b><i>Hype Attack:</i></b> <b>Palm Blast</b>";
                }
            }
            else if (P2Horizontal() < 0.0f)
            {
                audioSource.clip = navChime;
                if (navIcons[1].transform.position != characterSelection[0].transform.position)
                {
                    audioSource.Play();
                    navIcons[1].transform.position = characterSelection[0].transform.position + new Vector3(125, 0, 0);
                    characterInfo[1].text = "<b><i>Name:</i></b> <b>Marie O'Nett</b> \n \n<b><i>Dmg Type:</i></b> <color=red><b> Heavy</b></color> \n \n<b><i>Hype Attack:</i></b> <b>Tornado Swing</b>";
                }
            }
            else if (P2ConfirmButton())
            {
                audioSource.clip = navConfirm;

                if (navIcons[1].transform.position == characterSelection[0].transform.position + new Vector3(125, 0, 0))
                {
                    audioSource.Play();
                    MainGameManager.Instance.Fighters[1] = Fighters.MARIE;
                    p2Confirmed = true;
                }
                else if (navIcons[1].transform.position == characterSelection[1].transform.position + new Vector3(125, 0, 0))
                {
                    audioSource.Play();
                    MainGameManager.Instance.Fighters[1] = Fighters.DUKEZ;
                    p2Confirmed = true;
                }

            }
        }
    }


    private float P1Vertical()
    {
        return Input.GetAxis("NavV1");
    }

    private float P2Vertical()
    {
        return Input.GetAxis("NavV2");
    }

    private float P1Horizontal()
    {
        return Input.GetAxis("NavH1");
    }

    private float P2Horizontal()
    {
        return Input.GetAxis("NavH2");
    }

    private bool P1ConfirmButton()
    {
        bool buttonPressed = new bool();
        if (Input.GetButtonDown("Attack1"))
            buttonPressed = Input.GetButtonDown("Attack1");
        return buttonPressed;
    }
    private bool P2ConfirmButton()
    {
        return Input.GetButtonDown("Attack2");
    }


}
