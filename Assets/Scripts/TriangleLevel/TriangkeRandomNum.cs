using UnityEngine;
using TMPro;
public class TriangkeRandomNum : MonoBehaviour
{
    public TextMeshPro numberText; // Use TextMeshPro for 3D text
    [HideInInspector] public int randomNumber; // Make randomNumber public to access from other scripts

    void Start()
    {
        //GenerateRandomNumber();
    }

    //void GenerateRandomNumber()
    //{
    //    randomNumber = UnityEngine.Random.Range(2, 6); // Generates a random number between 1 and 10
    //    numberText.text = randomNumber.ToString(); // Display the number in 3D TextMesh Pro
    //}
}
