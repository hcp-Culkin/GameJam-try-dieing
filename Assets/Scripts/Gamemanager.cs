using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{

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
    public GameObject XRoadParent;
    private List<RectTransform> XRoad = new List<RectTransform>();

    private List<TruckControls> Trucks = new List<TruckControls>();


    public List<PlayerSaving> SavedData = new List<PlayerSaving>();
    private int dataIndex = 0;
    bool TruckAdded;

    private float TimerTemp = 0;
    private float TimeFirst;
    private int Level = 0;
    private bool GameEnded = true;
    private bool ChickenCouldntReach = false;
    [Header("Hud item")]
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI LevelText;


    public CanvasGroup ChickenWalk;
    public CanvasGroup TruckPlace;
    public Slider TruckTimeCheck;

    public CanvasGroup Gameover;


    private void Awake()
    {
        for (int i = 0; i < RoadParent.transform.childCount; i++)
        {
            RoadList.Add(RoadParent.transform.GetChild(i).GetComponent<RectTransform>());
        }
        for (int i = 0; i < XRoadParent.transform.childCount; i++)
        {
            XRoad.Add(XRoadParent.transform.GetChild(i).GetComponent<RectTransform>());
        }
        StartSize = PlayerObj.transform.localScale;
        PlayerIndexX = 4;
        CheckGameover();



    }




    public void ButtonClick(Transform transformObj)
    {


        if (TruckAdded)
        {
            return;
        }

        Transform marker = transformObj.GetChild(0);

        GameObject TempTruck = GameObject.Instantiate(Truck);
        TempTruck.transform.SetParent(transformObj.transform);
        TempTruck.transform.localScale = Truck.transform.localScale;
        TempTruck.transform.localPosition = marker.localPosition;
        TempTruck.GetComponent<TruckControls>().FromMarker = transformObj.GetChild(0);
        TempTruck.GetComponent<TruckControls>().ToMarker = transformObj.GetChild(1);
        TempTruck.GetComponent<TruckControls>().Delay = TruckTimeCheck.value;

        Trucks.Add(TempTruck.GetComponent<TruckControls>());
        TruckAdded = true;
    }



    public void DieChickenDie()
    {

        GameEnded = true;

        if(Level%2 != 0)
        {
            ChickenCouldntReach = true;
        }

        CheckGameover();

    }


    public void StartGame()
    {

        PlayerObj.transform.localPosition = new Vector2(XRoad[4].transform.localPosition.x, RoadList[0].transform.localPosition.y);


        ChickenWalk.alpha = 0;
        ChickenWalk.blocksRaycasts = false;
        TruckPlace.alpha = 0;
        TruckPlace.blocksRaycasts = false;






        if (GameEnded)
        {
            GameEnded = false;

            dataIndex = 0;
            if (Level % 2 == 0)
            {
                for (int i = 0; i < Trucks.Count; i++)
                {
                    Trucks[i].SetInMotion();
                }

                TimerTemp = TimeFirst;
            }
            else
            {


                if (Level == 1)
                {
                    TimeFirst = TimerTemp;
                }
                else
                {
                    for (int i = 0; i < Trucks.Count; i++)
                    {
                        Trucks[i].SetInMotion();
                    }

                    TimerTemp = TimeFirst;
                    PlayerIndexY = 0;
                    PlayerIndexX = 4;
                }

                SavedData.Clear();
                PlayerSaving temp = new PlayerSaving();
                temp.TimeToSave = 0;
                temp.PositionToSave = PlayerObj.transform.localPosition;
                SavedData.Add(temp);
            }
        }

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
        float RealTimer;
        if (Level == 1)
        {
            RealTimer = TimerTemp;
        }
        else
        {
            RealTimer = TimeFirst - TimerTemp;
        }


        PlayerObj.transform.localPosition = new Vector2(XRoad[PlayerIndexX].transform.localPosition.x, RoadList[PlayerIndexY].transform.localPosition.y);

        PlayerSaving temp = new PlayerSaving();
        temp.TimeToSave = RealTimer;
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

        float RealTimer;
        if (Level == 1)
        {
            RealTimer = TimerTemp;
        }
        else
        {
            RealTimer = TimeFirst - TimerTemp;
        }


            PlayerObj.transform.localPosition = new Vector2(XRoad[PlayerIndexX].transform.localPosition.x, RoadList[PlayerIndexY].transform.localPosition.y);
        PlayerSaving temp = new PlayerSaving();
        temp.TimeToSave = RealTimer;
        temp.PositionToSave = PlayerObj.transform.localPosition;
        SavedData.Add(temp);

        if (PlayerIndexY >= RoadList.Count - 1)
        {
            GameEnded = true;
            Debug.Log("GameOver");
            if (Level == 1)
            {
                TimeFirst = TimerTemp;
            }
            else
            {
                TimerTemp = TimeFirst;
            }

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


        if (ChickenCouldntReach)
        {

            Debug.Log("Run Over");

            Gameover.alpha = 1;
            Gameover.blocksRaycasts = true;




            return;
        }

        Level++;




        if (Level % 2 == 0)
        {
            dataIndex = 0;
            PlayerButtonCanvas.alpha = 0;
            PlayerButtonCanvas.blocksRaycasts = false;
            RoadParent.GetComponent<CanvasGroup>().blocksRaycasts = true;


            TruckTimeCheck.minValue = -0.1f;
            TruckTimeCheck.maxValue = TimeFirst;

            ChickenWalk.alpha = 0;
            ChickenWalk.blocksRaycasts = false;
            TruckPlace.alpha = 1;
            TruckPlace.blocksRaycasts = true;
            TruckAdded = false;
        }
        else
        {
            PlayerButtonCanvas.alpha = 1;
            PlayerButtonCanvas.blocksRaycasts = true;
            RoadParent.GetComponent<CanvasGroup>().blocksRaycasts = false;

            ChickenWalk.alpha = 1;
            ChickenWalk.blocksRaycasts = true;
            TruckPlace.alpha = 0;
            TruckPlace.blocksRaycasts = false;
        }


        PlayerObj.transform.localPosition = new Vector2(XRoad[4].transform.localPosition.x, RoadList[0].transform.localPosition.y);




    }


    public void OnvalueChange()
    {
        if (Level % 2 == 0)
        {
            if (dataIndex >= SavedData.Count)
            {
                dataIndex = SavedData.Count - 1;
            }

            int previous = dataIndex - 1;
            if (previous < 0)
            {
                previous = 0;
            }

            if (TruckTimeCheck.value >= SavedData[dataIndex].TimeToSave)
            {
                PlayerObj.transform.localPosition = SavedData[dataIndex].PositionToSave;
                dataIndex++;
            }
            else if (TruckTimeCheck.value < SavedData[previous].TimeToSave)
            {
                dataIndex--;
                if (dataIndex < 0)
                {
                    dataIndex = 0;
                }
                PlayerObj.transform.localPosition = SavedData[dataIndex].PositionToSave;

            }


        }
    }



    // Update is called once per frame
    void Update()
    {

        if (ChickenCouldntReach)
        {
            return;
        }



        if (GameEnded)
        {

            return;
        }

        LevelText.text = Level.ToString();

        if (Level == 1)
        {
            if (!GameEnded)
            {
                TimerTemp += Time.deltaTime;
            }
            DisplayTime(TimerTemp);

            return;
        }

        if (Level > 1)
        {

            if (!GameEnded)
            {
                TimerTemp -= Time.deltaTime;

            }
            if (TimerTemp <= 0)
            {
                GameEnded = true;
                ChickenCouldntReach = true;
                CheckGameover();
                return;
            }
            DisplayTime(TimerTemp);


            if (Level % 2 == 0)
            {
                if ((TimeFirst - TimerTemp) >= SavedData[dataIndex].TimeToSave)
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
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}


[Serializable]
public class PlayerSaving
{
    public float TimeToSave;
    public Vector2 PositionToSave;
}
