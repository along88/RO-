using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour
{
    private bool p1Confirmed;
    private bool p2Confirmed;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip navChime;
    [SerializeField]
    private AudioClip navConfirm;
    [SerializeField]
    private Button[] characterSelection;
    [SerializeField]
    private GameObject[] navIcons;
    private int[] maxCharacters;


    private void Awake()
    {
        if (MainGameManager.Instance.ActivePlayers == 2)
            navIcons[2].active = false;
        else
            navIcons[1].active = false;
        

        navIcons[0].transform.position = (characterSelection[0].transform.position - new Vector3(25, 0, 0));

        if(MainGameManager.Instance.ActivePlayers == 2)
            navIcons[1].transform.position = (characterSelection[1].transform.position + new Vector3(125, 0, 0));
        else
            navIcons[2].transform.position = (characterSelection[1].transform.position + new Vector3(125, 0, 0));

        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Attack2") && MainGameManager.Instance.ActivePlayers != 2)
        {
            //change cpu icon to P2
            navIcons[1].transform.position = navIcons[2].transform.position;
            navIcons[2].active = false;
            navIcons[1].active = true;

            ///To Do:
            ///Pass Control of the Selection Indicator to Player2's Controller inputs;

        }

        P1Selection();

        if (p1Confirmed && p2Confirmed)
            LoadLevel("RingMap");
        




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
            if (navIcons[0].transform.position != characterSelection[1].transform.position)
            {
                audioSource.Play();
                navIcons[0].transform.position = characterSelection[1].transform.position - new Vector3(125, 0, 0);
            }
        }
        else if (P1Horizontal() < 0.0f)
        {
            if (navIcons[0].transform.position != characterSelection[0].transform.position)
            {
                audioSource.Play();
                navIcons[0].transform.position = characterSelection[0].transform.position - new Vector3(125, 0, 0);
            }
        }
        else if (P1ConfirmButton())
        {
            if (!p1Confirmed)
            {
                if (navIcons[0].transform.position == characterSelection[0].transform.position)
                {
                    MainGameManager.Instance.Fighters[0] = Fighters.MARIE;
                    p1Confirmed = true;
                }
                else if (navIcons[0].transform.position == characterSelection[1].transform.position)
                {
                    MainGameManager.Instance.Fighters[0] = Fighters.DUKEZ;
                    p1Confirmed = true;
                }
            }else
            {
                p1Confirmed = false;
            }
        }
    }
    private void P2Selection()
    {
        if (P1Horizontal() > 0.0f)
        {
            audioSource.clip = navChime;
            if (navIcons[1].transform.position != characterSelection[1].transform.position)
            {
                audioSource.Play();
                navIcons[1].transform.position = characterSelection[1].transform.position + new Vector3(125, 0, 0);
            }
        }
        else if (P1Horizontal() < 0.0f)
        {
            if (navIcons[1].transform.position != characterSelection[0].transform.position)
            {
                audioSource.Play();
                navIcons[1].transform.position = characterSelection[0].transform.position + new Vector3(125, 0, 0);
            }
        }
        else if (P2ConfirmButton())
        {
            if (!p2Confirmed)
            {
                if (navIcons[1].transform.position == characterSelection[0].transform.position)
                {
                    MainGameManager.Instance.Fighters[1] = Fighters.MARIE;
                    p2Confirmed = true;
                }
                else if (navIcons[1].transform.position == characterSelection[1].transform.position)
                {
                    MainGameManager.Instance.Fighters[1] = Fighters.DUKEZ;
                    p2Confirmed = true;
                }
            }
            else
            {
                p2Confirmed = false;
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
        bool buttonPressed = new bool();
        if (Input.GetButtonDown("Attack2"))
            buttonPressed = Input.GetButtonDown("Attack2");
        return buttonPressed;
    }


}
