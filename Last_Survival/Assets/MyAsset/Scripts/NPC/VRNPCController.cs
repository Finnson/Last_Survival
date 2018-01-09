using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRNPCController : MonoBehaviour {
    public Transform character;
    public Transform NPC;
    public int current;
    public int target;
	bool targetChange = false;

	public bool move;
	public bool arrivePoint = true;
	public bool isTalking;
	public bool isMoveing;
	private Animator animator;

    int[][] map;
    int[][] mapDist;

    int wayStep;
    int[] way;

    public float speed = 1;
    Transform[] positions;

	public bool isOut = false;

    // Use this for initialization
    void Start () {
        map = new int[48][];
        mapDist = new int[48][];
        way = new int[48];
        for (int i = 0; i < 48; i++)
        {
            map[i] = new int[8];
            mapDist[i] = new int[8];
        }

        string mapData = Resources.Load("Map").ToString();
        int pos = 0;
        for (int i = 0; i < 48; i++)
            for (int j = 0; j < 8; j++)
            {
                while ((mapData[pos] < '0') || ('9' < mapData[pos])) pos++;
                
                map[i][j] = 0;
                while (('0' <= mapData[pos]) && (mapData[pos]<= '9'))
                {
                    map[i][j] = map[i][j] * 10 + mapData[pos] - '0';
                    pos++;
                }
            }

        string distData = Resources.Load("Dist").ToString();
        pos = 0;
        for (int i = 0; i < 48; i++)
            for (int j = 0; j < 8; j++)
            {
                while ((distData[pos] < '0') || ('9' < distData[pos])) pos++;

                mapDist[i][j] = 0;
                while (('0' <= distData[pos]) && (distData[pos] <= '9'))
                {
                    mapDist[i][j] = mapDist[i][j] * 10 + distData[pos] - '0';
                    pos++;
                }
            }

        move = false;
        isTalking = false;
		isMoveing = false;
		animator= GetComponent<Animator> ();

        positions = WayPoints.positions;
    }
		
	public void NPCMove() {
		move = true;
		isTalking = false;
	}

	public void NPCTalking()
	{
		isTalking = true;
	}

    // Update is called once per frame
    void Update () {
		if (arrivePoint)
		if (move || targetChange) {
			AStar (target);

			move = false;
			isMoveing = true;
		}

		if (isMoveing)
			Move ();
		if (isTalking) {
			Vector3 targetPosition = character.transform.position;
			targetPosition.y = NPC.position.y;
			NPC.LookAt (targetPosition);
		}
	}

	public void setCurrent(int cur) {
		current = cur;
	}

	public void setTarget(int tar) {
		target = tar;
		targetChange = true;
	}

    void AStar(int tar)
    {
        int start = 0, end = 0;
        int[] f = new int[48];
        int[] fFrom = new int[48];
        float[] priority = new float[48];
        int[] fIndex = new int[48];
        int[] dist = new int[48];
        
		f[0] = current;
        fFrom[0] = 0;
        priority[0] = 0;
        for (int i = 0; i < 48; i++)
        {
            fIndex[i] = -1;
            dist[i] = 100;
        }
		fIndex[current] = 0;
		dist[current] = 0;

        int cur, d, fI, tempi;
        float tempf;
        while (start <= end)
        {
            if (f[start] == tar) break;

            cur = f[start];
            for (int i = 0; i < 8; i++)
                if (map[cur][i] != cur)
                {
                    d = dist[cur] + mapDist[cur][i];
                    if (fIndex[map[cur][i]] == -1)
                    {
                        end++;
                        f[end] = map[cur][i];
                        fFrom[end] = start;
                        fIndex[map[cur][i]] = end;
						priority[end] = d + Vector3.Distance(positions[map[cur][i]].position, positions[tar].position);
                        dist[map[cur][i]] = d;
                    }
                    else if (d < dist[map[cur][i]])
                    {
                        fI = fIndex[map[cur][i]];
                        priority[fI] = d + Vector3.Distance(positions[map[cur][i]].position, positions[tar].position);
                        for (int j = fI - 1; j > start; j--)
                            if (priority[fI] < priority[j])
                            {
                                fIndex[f[j]] = fI;

                                tempi = f[fI];
                                f[fI] = f[j];
                                f[j] = tempi;

                                tempi = fFrom[fI];
                                fFrom[fI] = fFrom[j];
                                fFrom[j] = tempi;

                                tempf = priority[fI];
                                priority[fI] = priority[j];
                                priority[j] = tempf;

                                fI = j;
                            }
                        fIndex[map[cur][i]] = fI;
                        dist[map[cur][i]] = d;
                    }
                }
                else break;

            start++;
        }

		wayStep = 0;
        while (start > 0)
        {
            way[wayStep] = f[start];
            start = fFrom[start];
            wayStep++;
        }
		wayStep--;
    }

    void Move()
    {
        if (wayStep < 0)
        {
            isMoveing = false;
			animator.SetFloat ("Speed Input", 0.0f);
			isOut = true;
            return;
        }

		Vector3 targetPosition = positions[way[wayStep]].position;
		targetPosition.y = NPC.position.y;
		NPC.LookAt (targetPosition);

		transform.position += (positions[way[wayStep]].position - transform.position).normalized * Time.deltaTime * speed;
		animator.SetFloat ("Speed Input", 0.2f);
		arrivePoint = false;
		if (Vector3.Distance (positions [way [wayStep]].position, transform.position) < 0.1f) {
			wayStep--;
			arrivePoint = true;
		}
    }
}
