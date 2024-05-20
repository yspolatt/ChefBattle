using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager: Singleton<GameManager>
{

    private float profit = 0f;

    private float currentPriceOfSteak;


    [SerializeField] private float averageSteakScore = 0f;

    [SerializeField] private float averageCustomerWaitingTime;

    [SerializeField] private float defaultSteakPrice = 10f;

    [SerializeField] private float defaultSteakScore = 1f;

    [SerializeField] private float defaultCustomerWaitingTime = 5f;

    [SerializeField] private float priceCoefficient = 1f;

    [SerializeField] private float steakScoreCoefficient = 1f;

    [SerializeField] private float customerWaitingTimeCoefficient = 1f;

    [SerializeField] private float absoluteMaxChangeDelta = 3f;




    
    private int numOfCustomers = 0; 

    private int numOfSteaks = 0;

    private float totalSteakScore = 0f;

    private float totalCustomerWaitingTime = 0f;

    public TextMeshProUGUI profitText;

    public TextMeshProUGUI currentPriceText;


    protected override void Awake()
    {
        currentPriceOfSteak = defaultSteakPrice;
        base.Awake();
    }


    void Start()
    {   
        Customer.OnWaitingFinished += OnCustomerWaitingFinished;
        Steak.OnSteakCooked += OnSteakCooked;
        Customer.OnEatingFinished += OnCustomerEatingFinished;

       
    }

    private float CalculateCustomerSpawnTimeForSteak(){
        float scoreImpact = (averageSteakScore - defaultSteakScore) * steakScoreCoefficient;
        Debug.Log("Score Impact: " + scoreImpact);
        return scoreImpact;
    }

    private float CalculateCustomerSpawnTimeForCustomer(){
        float waitingTimeImpact = customerWaitingTimeCoefficient * ( averageCustomerWaitingTime - defaultCustomerWaitingTime);
        Debug.Log("Waiting Time Impact: " + waitingTimeImpact);
        return waitingTimeImpact;
        
    }

    private float CalculateCustomerSpawnTimeForPrice(){
        float priceImpact = (currentPriceOfSteak - defaultSteakPrice) * priceCoefficient;
        Debug.Log("Price Impact: " + priceImpact);
        return priceImpact;

    }



    private void ChangeCustomerSpawnIntervalSteak(){
        float delta = CalculateCustomerSpawnTimeForSteak();
        delta = Mathf.Clamp(delta, -absoluteMaxChangeDelta, absoluteMaxChangeDelta);

        CustomerManager.Instance.ChangeCustomerSpawnInterval(delta);
    }
    private void ChangeCustomerSpawnIntervalCustomer(){
        float delta = CalculateCustomerSpawnTimeForCustomer();
        delta = Mathf.Clamp(delta, -absoluteMaxChangeDelta, absoluteMaxChangeDelta);

        CustomerManager.Instance.ChangeCustomerSpawnInterval(delta);
    }


    private void ChangeCustomerSpawnIntervalPrice(int increase = 1){
        float delta = priceCoefficient * increase; 
        delta = Mathf.Clamp(delta, -absoluteMaxChangeDelta, absoluteMaxChangeDelta);
        CustomerManager.Instance.ChangeCustomerSpawnInterval(delta);
    }
     private float CalculateSteakScore(Steak steak){
        float duration = steak.GetCookingDuration();
        float min = 0f;
        float max = Steak.maxCookingDuration;
        float optimum = (min + max) / 2;
        
        return 1 - Mathf.Abs(duration - optimum) / (max - min);
    }

    private void OnSteakCooked(Steak steak){
        float steakScore =  CalculateSteakScore(steak); 
        if(steakScore > 0.9 ){
            Debug.Log("Perfect Steak!");
            steakScore = 1.2f; // Bonus for perfect steak
        }
        Debug.Log("Steak Score: " + steakScore);
        totalSteakScore += steakScore;
        numOfSteaks++;
        averageSteakScore = totalSteakScore / numOfSteaks;
        ChangeCustomerSpawnIntervalSteak();
    }

    private void OnCustomerWaitingFinished(Customer customer){
        numOfCustomers++;
        totalCustomerWaitingTime += customer.waitingTime;
        Debug.Log("Customer Waiting Time: " + customer.waitingTime);
        averageCustomerWaitingTime = totalCustomerWaitingTime / numOfCustomers;
        ChangeCustomerSpawnIntervalCustomer();
    }

    public void SetPriceOfSteak(float price){
        currentPriceOfSteak = price;
    }

    public float GetPriceOfSteak(){
        return currentPriceOfSteak;
    }

    public void IncreasePriceOfSteak(){
        currentPriceOfSteak += 1f;
        ChangeCustomerSpawnIntervalPrice();
        currentPriceText.text = "Current Price: $" + currentPriceOfSteak;
    }

    public void DecreasePriceOfSteak(){
        if(currentPriceOfSteak <= 1){
            return;
        }
        currentPriceOfSteak -= 1f;
        ChangeCustomerSpawnIntervalPrice(-1);
        currentPriceText.text = "Current Price: $" + currentPriceOfSteak;
    }

    private void OnCustomerEatingFinished(Customer customer){
        profit += customer.priceToPay;
        profitText.text = "$" + profit;
    }



   
  
}