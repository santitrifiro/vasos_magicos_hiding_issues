using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VasoManager : MonoBehaviour {

    public VasosManager vm;
    public BolitaManager bm;
    private int id;
    private bool choose = false;
    private Color default_color;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setId (int id) {

        this.id = id;

    }

    public void setChoose (bool choose) {

        this.choose = choose;

    } 

    public void setInvisible (bool v) {

        GetComponent<Renderer>().enabled = !v;

    }

    public void OnMouseDown () {

        if (choose) {

            Debug.Log(id + " " + bm.getPos());

            vm.TerminateGame(id == bm.getPos());

        }

    }

}
