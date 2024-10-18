using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolitaManager : MonoBehaviour
{

    private int pos;

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void generatePos (int n) {

        if (pos == 0) {

            System.Random random = new System.Random();

            pos = random.Next(0, n);

            Vector3 new_position = transform.position;
            new_position.x += pos;

            transform.position = new_position;

        }

    }

    public int getPos () {

        return pos;

    }

    public void setPos (int pos) {

        Vector3 new_position = transform.position;
        new_position.x = new_position.x + (pos - this.pos);
        transform.position = new_position;
        this.pos = pos;

    }

    public void setInvisible (bool v) {

        GetComponent<Renderer>().enabled = !v;

    }

    public void bolitaWinner () {

        GetComponent<Renderer>().material.color = Color.green;

    }

    public void bolitaLooser () {

        GetComponent<Renderer>().material.color = Color.red;

    }

}
