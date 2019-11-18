using UnityEngine;
using System.Collections;

public class SquirrelSpawner : MonoBehaviour
{
    public GameObject squirrelModel;

    public int numSquirrels;

    // Use this for initialization
    void Start()
    {
        void SquirrelDied(object sender, ObservableArgs args)
        {
            numSquirrels -= 1;
        }

        for (int i = 0; i < numSquirrels; i++)
        {
            var newSquirrel = Instantiate(squirrelModel, new Vector3(Random.value * 200 - 100, 20, Random.value * 200 - 100), Quaternion.identity);
            var health = newSquirrel.GetComponent<SquirrelHealth>();
            health.SquirrelDied.Register(SquirrelDied);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(20, Screen.height - 70, 100, 50), numSquirrels.ToString() + " squirrels left");
    }
}
