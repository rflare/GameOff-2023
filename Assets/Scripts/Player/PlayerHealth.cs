using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public void Update()
    {
        if(health > 0)
            return;

        Debug.Log("Dead");
    }
}