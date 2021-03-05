using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class JavascriptHook : MonoBehaviour
{
    [SerializeField] private WordingMovement wmWordingMovement;
    [SerializeField] private Text tTextRecognizedSpeech;
    [SerializeField] private GameObject goImageResult;
    [SerializeField] private Sprite[] sDirSprites;

    private Image iImageResult;
    private RectTransform rtInfoDir;

    private string sRecognizedSpeech;
    Direction eDirection;
    public enum Direction
    {
        Up, Down, Left, Right, Stop
    };


    private void Start()
    {
        iImageResult = goImageResult.GetComponent<Image>();
        rtInfoDir = goImageResult.GetComponent<RectTransform>();
    }

    //Get Recognized Speech to String
    public void ReturnRecognizeSpeechText(string str)
    {
        sRecognizedSpeech = str;
        StartCoroutine(LuisManager.getInstance().SubmitRequestToLuis(str));
        SetRecognizeSpeechText();
    }

    //Found Direction
    private void FoundDirectionWhithSpeechText()
    {
        foreach (string word in sRecognizedSpeech.Split(' '))
        {
            // Go to the UP
            foreach (string UpWord in wmWordingMovement.Up)
            {
                if (word == UpWord)
                {
                    eDirection = Direction.Up;
                    SendDirInfo();
                    break;
                }
            }

            // Go to the DOWN
            foreach (string DownWord in wmWordingMovement.Down)
            {
                if (word == DownWord)
                {
                    eDirection = Direction.Down;
                    SendDirInfo();
                    break;
                }
            }

            // Go to the LEFT
            foreach (string LeftWord in wmWordingMovement.Left)
            {
                if (word == LeftWord)
                {
                    eDirection = Direction.Left;
                    SendDirInfo();
                    break;
                }
            }

            // Go to the RIGHT
            foreach (string RightWord in wmWordingMovement.Right)
            {
                if (word == RightWord)
                {
                    eDirection = Direction.Right;
                    SendDirInfo();
                    break;
                }
            }

            // STOP
            foreach (string StopWord in wmWordingMovement.Stop)
            {
                if (word == StopWord)
                {
                    eDirection = Direction.Stop;
                    SendDirInfo();
                    break;
                }
            }
        }
    }

    //Set Output speech text in UI
    public void SetRecognizeSpeechText()
    {
        tTextRecognizedSpeech.text = sRecognizedSpeech;
    }


    //GET-SET Direction
    public Direction getDirection() { return eDirection; }
    public void setDirection(Direction newDir) { eDirection = newDir; }


    private void SendDirInfo()
    {
        print(eDirection.ToString());
        if (eDirection == Direction.Up)
        {
            iImageResult.sprite = sDirSprites[0];
            rtInfoDir.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (eDirection == Direction.Down)
        {
            iImageResult.sprite = sDirSprites[0];
            rtInfoDir.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (eDirection == Direction.Left)
        {
            iImageResult.sprite = sDirSprites[0];
            rtInfoDir.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (eDirection == Direction.Right)
        {
            iImageResult.sprite = sDirSprites[0];
            rtInfoDir.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (eDirection == Direction.Stop)
        {
            iImageResult.sprite = sDirSprites[1];
            rtInfoDir.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void SetStopImageInfo()
    {
        iImageResult.sprite = sDirSprites[2];
        rtInfoDir.rotation = Quaternion.Euler(0, 0, 0);
    }
}