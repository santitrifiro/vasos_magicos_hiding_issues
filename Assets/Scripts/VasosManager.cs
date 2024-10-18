using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class VasosManager : MonoBehaviour
{
    
    public VasoManager vaso_default;
    public BolitaManager bolita;
    public bool start;

    public int vasos = 3;

    public int iteraciones = 10;

    public float velocidad = 5f;

    private List <VasoManager> vasos_list;
    private List <Vector3> up_positions;
    private List <Vector3> positions;
    private List <Vector3> down_positions;
    private List <KeyValuePair<int, int>> iteraciones_list;

    private int bolita_gg;

    void Start()
    {

        vaso_default.setId(0);
        vasos_list = new List<VasoManager>{vaso_default};
        iteraciones_list = new List<KeyValuePair<int, int>>();
        
        Vector3 up_position_default = vaso_default.transform.position;
        up_position_default.z += 1;

        Vector3 down_position_default = vaso_default.transform.position;
        down_position_default.z -= 1;

        up_positions = new List<Vector3>{up_position_default};
        positions = new List<Vector3>{vaso_default.transform.position};
        down_positions = new List<Vector3>{down_position_default};

        System.Random random = new System.Random();

        for (int i = 0; i < vasos - 1; i++) {

            VasoManager vaso_nuevo = Instantiate(vaso_default);
            vaso_nuevo.setId(i + 1);
            Vector3 new_position = vaso_default.transform.position;
            new_position.x += 1 + i;
            vaso_nuevo.transform.position = new_position;

            Vector3 up_position = vaso_nuevo.transform.position;
            up_position.z += 1;
            Vector3 down_position = vaso_nuevo.transform.position;
            down_position.z -= 1;

            vasos_list.Add(vaso_nuevo);
            up_positions.Add(up_position);
            positions.Add(vaso_nuevo.transform.position);
            down_positions.Add(down_position);

        }

        for (int i = 0; i < iteraciones; i++) {

            int n1;
            int n2;

            n1 = random.Next(0, vasos);
            
            do {

                n2 = random.Next(0, vasos);

            } while (n1 == n2);

            if (n1 > n2) {

                int aux = n2;
                n2 = n1;
                n1 = aux;

            }

            iteraciones_list.Add(new KeyValuePair<int, int>(n1, n2));

        }

        bolita.generatePos(vasos);

        bolita_gg = bolita.getPos();

        foreach (KeyValuePair <int, int> p in iteraciones_list) {

            if (bolita_gg == p.Key) {

                bolita_gg = p.Value;

            } else if (bolita_gg == p.Value) {

                bolita_gg = p.Key;

            }

        }

        foreach (VasoManager vaso in vasos_list) {

            vaso.setInvisible(true);

        }

    }

    private int iteraciones_contador = 0;
    private bool move1 = false;
    private bool move2 = false;
    private bool move3 = false;
    private bool shuffled = false;

    void Update()
    {

        if (start) {

            bolita.setInvisible(true);

            foreach (VasoManager vaso in vasos_list) {

                vaso.setInvisible(false);

            }

            KeyValuePair <int, int> iteracion = iteraciones_list[iteraciones_contador];
            int v1 = iteracion.Key;
            int v2 = iteracion.Value;

            Transform vaso1 = vasos_list[v1].transform;
            Transform vaso2 = vasos_list[v2].transform;

            Vector3 vaso1_up_position = up_positions[v1];
            Vector3 vaso1_position = positions[v1];
            Vector3 vaso1_down_position = down_positions[v1];
            Vector3 vaso2_up_position = up_positions[v2];
            Vector3 vaso2_position = positions[v2];
            Vector3 vaso2_down_position = down_positions[v2];

            bool check1 = Vector3.Distance(vaso1.position, vaso1_up_position) < 0.1f;
            bool check2 = Vector3.Distance(vaso2.position, vaso2_down_position) < 0.1f;

            if (!move1) {

                vaso1.position = Vector3.Lerp(vaso1.position, vaso1_up_position, Time.deltaTime * velocidad);
                vaso2.position = Vector3.Lerp(vaso2.position, vaso2_down_position, Time.deltaTime * velocidad);

            }

            if (move1 || (check1 && check2)) {

                move1 = true;

                bool check3 = Vector3.Distance(vaso1.position, vaso2_up_position) < 0.1f;
                bool check4 = Vector3.Distance(vaso2.position, vaso1_down_position) < 0.1f;

                if (!move2) {

                    vaso1.position = Vector3.Lerp(vaso1.position, vaso2_up_position, Time.deltaTime * velocidad);
                    vaso2.position = Vector3.Lerp(vaso2.position, vaso1_down_position, Time.deltaTime * velocidad);

                }

                if (move2 || (check3 && check4)) {

                    move2 = true;

                    bool check5 = Vector3.Distance(vaso1.position, vaso2_position) < 0.1f;;
                    bool check6 = Vector3.Distance(vaso2.position, vaso1_position) < 0.1f;

                    if (!move3) {

                        vaso1.position = Vector3.Lerp(vaso1.position, vaso2_position, Time.deltaTime * velocidad);
                        vaso2.position = Vector3.Lerp(vaso2.position, vaso1_position, Time.deltaTime * velocidad);

                    }

                    if (move3 || (check5 && check6)) {
                        
                        move3 = true;
                        vaso1.position = vaso1_position;
                        vaso2.position = vaso2_position;
                        move1 = false;
                        move2 = false;
                        move3 = false;
                        iteraciones_contador += 1;

                    }

                }

            }

            if (iteraciones_contador == iteraciones) {

                start = false;
                shuffled = true;

            }

        }

        if (shuffled) {

            bolita.setPos(bolita_gg);

            foreach (VasoManager vaso in vasos_list) {

                vaso.setChoose(true);

            }

        }

    }

    public void TerminateGame (bool success) {

        bolita.setInvisible(false);

        foreach (VasoManager vaso in vasos_list) {

            vaso.setInvisible(true);

        }

        if (success) {

            bolita.bolitaWinner();

        } else {

            bolita.bolitaLooser();

        }

    }
}
