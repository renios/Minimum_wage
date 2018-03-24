using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class RabbitInformation{
    static List<int> rabbitIndex = new List<int>(4);
    static int numOfRabbit;
    static List<Sprite> spritesOfFemale = new List<Sprite>();
    static List<Sprite> spritesOfMale = new List<Sprite>();

    public Sprite sprite;
    public Gender gender;
    public int index;

    public static void LoadSprites(){
        if (spritesOfFemale.Count < 1){
            var temp = Resources.LoadAll("customers/Female", typeof(Sprite));
            for (int i = 0; i < temp.Length; i++){
                spritesOfFemale.Add(temp[i] as Sprite);
            }
            //spritesOfFemale.Sort();
        } 
        if (spritesOfMale.Count < 1){
            var temp = Resources.LoadAll("customers/Male", typeof(Sprite));
            for (int i = 0; i < temp.Length; i++){
                spritesOfMale.Add(temp[i] as Sprite);
            }
            //spritesOfMale.Sort();
        } 
        numOfRabbit = spritesOfFemale.Count + spritesOfMale.Count;
    }
    public static RabbitInformation SelectRabbit(){
        var selectedRabbit = new RabbitInformation();
        int index = Random.Range(0, numOfRabbit);
        int i = 0;
        while(rabbitIndex.Contains(index)){
            index = Random.Range(0, numOfRabbit);
            i++;
            if(i > 9000){
                Debug.LogWarning("UnexpectedProbabilityException : This code was executed over 9000 times, so conditions are ignored");
                break;
            }
        }
        rabbitIndex.Add(index);
        selectedRabbit.index = index;
        if (index < spritesOfFemale.Count){
            selectedRabbit.sprite = spritesOfFemale[index];
            selectedRabbit.gender = Gender.Female;
        } else {
            index -= spritesOfFemale.Count;
            selectedRabbit.sprite = spritesOfMale[index];
            selectedRabbit.gender = Gender.Male;
        }
        return selectedRabbit;
    }
    public static void RemoveRabbitIndex(int index){
        if(rabbitIndex.Contains(index)){
            rabbitIndex.Remove(index);
        } else {
            Debug.LogWarning("List does't contain that element! : " + index);
        }
    }
}