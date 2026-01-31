using UnityEngine;

public class WallCube : MonoBehaviour
{
    public bool isOccupied = false; // To track if the placeholder is occupied

    // Optional: A reference to the cube occupying this placeholder
    public NumberGenerator occupyingCube;
}
