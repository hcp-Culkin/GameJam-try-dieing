using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI objectiveText;
    [SerializeField] Button playButton;

    [Header("Player item")]
    public GameObject PlayerObj;
    public CanvasGroup PlayerButtonCanvas;
    private int PlayerIndexY = 0;
    private int PlayerIndexX = 0;
    private Vector2 StartSize;
    private bool playerMoving;
    public GameObject Truck;


    [Header("Game item")]
    public GameObject RoadParent;
    private List<RectTransform> RoadList = new List<RectTransform>();
    private List<RectTransform> XRoad = new List<RectTransform>();

    public List<PlayerSaving> SavedData = new List<PlayerSaving>();
    private int dataIndex = 0;

    private float TimerTemp = 0;
    public int Level = 0;
    private bool GameEnded = true;
    private bool ChickenCouldntReach = false;
    // public TextMeshProUGUI TimerText;
    // public TextMeshProUGUI LevelText;


    private void Awake()
    {
        for (int i = 0; i < RoadParent.transform.childCount; i++)
        {
            RoadList.Add(RoadParent.transform.GetChild(i).GetComponent<RectTransform>());
        }
        for (int i = 0; i < RoadParent.transform.GetChild(0).transform.childCount; i++)
        {
            XRoad.Add(RoadParent.transform.GetChild(0).transform.GetChild(i).GetComponent<RectTransform>());
        }
        StartSize = PlayerObj.transform.localScale;
        PlayerIndexX = 4;
        CheckGameover();



    }




    public void DieChickenDie()
    {

        GameEnded = true;

        if (Level % 2 != 0)
        {
            ChickenCouldntReach = true;
        }

        CheckGameover();

    }


    public void StartGame()
    {

        PlayerObj.transform.localPosition = new Vector2(XRoad[4].transform.localPosition.x, RoadList[0].transform.localPosition.y);


        if (GameEnded)
        {
            GameEnded = false;
            playButton.interactable = false;
            ItemPlacer.Instance.PlacementUI.SetActive(false);
            dataIndex = 0;
            SetObjectiveText("Cross road");

            if (Level % 2 == 0)
            {
                TimerTemp = 0;
            }
            else
            {
                TimerTemp = 0;

                PlayerIndexY = 0;
                PlayerIndexX = 4;

                SavedData.Clear();
                PlayerSaving temp = new PlayerSaving();
                temp.TimeToSave = 0;
                temp.PositionToSave = PlayerObj.transform.localPosition;
                SavedData.Add(temp);
            }
        }

    }


    void SetObjectiveText(string newText)
    {
        objectiveText.text = $"Objective:\n{newText}";
    }

    public void MovePlayer(bool left = false)
    {

        if (ChickenCouldntReach || GameEnded)
        {
            return;
        }


        if (!left)
        {
            PlayerIndexX++;
            if (PlayerIndexX >= XRoad.Count)
            {
                PlayerIndexX = XRoad.Count - 1;
            }

        }
        else
        {
            PlayerIndexX--;
            if (PlayerIndexX < 0)
            {
                PlayerIndexX = 0;
            }
        }
        //float RealTimer;
        //if (Level == 1)
        //{
        //    RealTimer = TimerTemp;
        //}
        //else
        //{
        //    RealTimer = TimeFirst - TimerTemp;
        //}


        PlayerObj.transform.localPosition = new Vector2(XRoad[PlayerIndexX].transform.localPosition.x, RoadList[PlayerIndexY].transform.localPosition.y);

        PlayerSaving temp = new PlayerSaving();
        temp.TimeToSave = TimerTemp;
        temp.PositionToSave = PlayerObj.transform.localPosition;
        SavedData.Add(temp);
    }



    public void PlayerJump()
    {

        if (ChickenCouldntReach || GameEnded)
        {
            return;
        }

        PlayerIndexY++;
        if (PlayerIndexY >= RoadList.Count)
        {
            Debug.Log("GameOver");
            return;
        }

        //float RealTimer;
        //if (Level == 1)
        //{
        //    RealTimer = TimerTemp;
        //}
        //else
        //{
        //    RealTimer = TimeFirst - TimerTemp;
        //}


        PlayerObj.transform.localPosition = new Vector2(XRoad[PlayerIndexX].transform.localPosition.x, RoadList[PlayerIndexY].transform.localPosition.y);
        PlayerSaving temp = new PlayerSaving();
        temp.TimeToSave = TimerTemp;
        temp.PositionToSave = PlayerObj.transform.localPosition;
        SavedData.Add(temp);

        if (PlayerIndexY >= RoadList.Count - 1)
        {
            GameEnded = true;
            Debug.Log("GameOver");

            CheckGameover();
        }
        // pla




    }

    public void GameEnd()
    {
        GameEnded = true;
        CheckGameover();
    }


    public void Restart()
    {
        SceneManager.LoadScene(0);
    }


    public void CheckGameover()
    {

        ItemPlacer.Instance.StopMovement();
        if (ChickenCouldntReach)
        {

            Debug.Log("Run Over");

            return;
        }

        Level++;




        if (Level % 2 == 0)
        {
            SetObjectiveText("Kill Chicken");

            dataIndex = 0;
            PlayerButtonCanvas.alpha = 0;
            PlayerButtonCanvas.blocksRaycasts = false;
            RoadParent.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            PlayerButtonCanvas.alpha = 1;
            PlayerButtonCanvas.blocksRaycasts = true;
            RoadParent.GetComponent<CanvasGroup>().blocksRaycasts = false;

        }


        PlayerObj.transform.localPosition = new Vector2(XRoad[4].transform.localPosition.x, RoadList[0].transform.localPosition.y);




    }


    public void OnvalueChange()
    {
        //if (Level % 2 == 0)
        //{
        //    if (dataIndex >= SavedData.Count)
        //    {
        //        dataIndex = SavedData.Count - 1;
        //    }

        //    int previous = dataIndex - 1;
        //    if (previous < 0)
        //    {
        //        previous = 0;
        //    }

        //    if (TruckTimeCheck.value >= SavedData[dataIndex].TimeToSave)
        //    {
        //        PlayerObj.transform.localPosition = SavedData[dataIndex].PositionToSave;
        //        dataIndex++;
        //    }
        //    else if (TruckTimeCheck.value < SavedData[previous].TimeToSave)
        //    {
        //        dataIndex--;
        //        if (dataIndex < 0)
        //        {
        //            dataIndex = 0;
        //        }
        //        PlayerObj.transform.localPosition = SavedData[dataIndex].PositionToSave;

        //    }


        //}
    }



    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyUp(KeyCode.W))
        {
            PlayerJump();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            MovePlayer(true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            MovePlayer(false);
        }



        if (ChickenCouldntReach)
        {
            return;
        }



        if (GameEnded)
        {

            return;
        }

        // LevelText.text = Level.ToString();

        if (!GameEnded)
        {
            TimerTemp += Time.deltaTime;
        }

        if (Level == 1)
        {

            DisplayTime(TimerTemp);

            return;
        }

        if (Level > 1)
        {

            //if (TimerTemp <= 0)
            //{
            //    GameEnded = true;
            //    ChickenCouldntReach = true;
            //    CheckGameover();
            //    return;
            //}
            DisplayTime(TimerTemp);


            if (Level % 2 == 0)
            {
                if ((TimerTemp) >= SavedData[dataIndex].TimeToSave)
                {
                    PlayerObj.transform.localPosition = SavedData[dataIndex].PositionToSave;
                    dataIndex++;
                }

                if (dataIndex >= SavedData.Count)
                {
                    GameEnded = true;
                    ChickenCouldntReach = true;
                    CheckGameover();

                }
            }
        }
    }


    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        // TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}


[Serializable]
public class PlayerSaving
{
    public float TimeToSave;
    public Vector2 PositionToSave;
}
