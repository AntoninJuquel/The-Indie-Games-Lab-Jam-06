using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEditor;

public class Train : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] PathCreator pathCreator;
    [SerializeField] EndOfPathInstruction end;
    float dstTravelled;

    [Header("Stats")]
    [SerializeField] bool moving;
    [SerializeField] float speed;

    [Header("Train")]
    [SerializeField] int beginSize;
    [SerializeField] List<Transform> wagons = new List<Transform>();
    [SerializeField] List<Wagon> m_wagons = new List<Wagon>();
    [SerializeField] float minDistance = 0.25f;
    [SerializeField] GameObject wagonPrefab;

    [Header("Style")]
    [SerializeField] ParticleSystem smoke;
    [SerializeField] Gradient gradient;
    MaterialPropertyBlock block;
    float distance;
    Transform currentWagon;
    Transform previousWagon;

    Vector3 startPosition;
    TrainChecker trainChecker;

    private void Awake()
    {
        trainChecker = GetComponent<TrainChecker>();
        block = new MaterialPropertyBlock();
        startPosition = transform.position;
        if (!moving)
            SetSpeed(50);
        else
        {
            var em = smoke.emission;
            em.rateOverTime = (speed + 5) * 2;
            SetSmokeColor(100);
            smoke.Play();
        }
    }
    private void Start()
    {
        for (int i = 0; i < beginSize - 1; i++)
        {
            AddWagon();
        }
    }
    private void Update()
    {
        if (moving)
        {
            Move();
            if (trainChecker)
                trainChecker.Checker();
        }
        if (trainChecker)
            trainChecker.CheckStartEndNode();
    }

    public void PauseTrain()
    {
        moving = false;
        smoke.Stop();
    }
    public void StartTrain()
    {
        moving = true;
        smoke.Play();
        AudioManager.Instance.Play("train_whistle_" + Random.Range(0, 3));
    }
    public void ResetTrain()
    {
        transform.position = startPosition;
        wagons[0].position = startPosition;
        for (int i = 1; i < wagons.Count; i++)
        {
            wagons[i].position = wagons[i - 1].position + Vector3.right * .1f;
        }
        foreach (Wagon wagon in m_wagons)
        {
            wagon.item = null;
            wagon.cargo = Wagon.Cargo.empty;
        }
        dstTravelled = 0;
        moving = false;
        smoke.Stop();
    }
    public void SetSpeed(float percent)
    {
        speed = percent * 2f / 100f;
        SetSmokeColor(percent);
        var em = smoke.emission;
        em.rateOverTime = (speed + 5) * 2;
    }
    public float GetSpeed()
    {
        return speed / 2f * 100f;
    }
    public void SetSmokeColor(float percent)
    {
        block.SetColor("_BaseColor", gradient.Evaluate(percent / 100f));
        var main = smoke.main;
        main.startColor = block.GetColor("_BaseColor");
    }

    public void Move()
    {
        float currentSpeed = speed;

        dstTravelled += currentSpeed * Time.deltaTime;
        wagons[0].position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
        wagons[0].rotation = pathCreator.path.GetRotationAtDistance(dstTravelled, end);

        for (int i = 1; i < wagons.Count; i++)
        {
            currentWagon = wagons[i];
            previousWagon = wagons[i - 1];

            distance = Vector3.Distance(previousWagon.position, currentWagon.position);

            Vector3 newPos = previousWagon.position;

            newPos.y = wagons[0].position.y;

            float T = Time.deltaTime * distance / minDistance * currentSpeed;

            if (T > .5f)
                T = .5f;

            currentWagon.position = Vector3.Slerp(currentWagon.position, newPos, T);
            currentWagon.rotation = Quaternion.Slerp(currentWagon.rotation, previousWagon.rotation, T);
        }
    }

    public void AddWagon()
    {
        Transform newPart = (Instantiate(wagonPrefab, wagons[wagons.Count - 1].position + Vector3.right * .1f, wagons[wagons.Count - 1].rotation) as GameObject).transform;

        newPart.SetParent(transform);

        wagons.Add(newPart);
        m_wagons.Add(new Wagon());
    }

    public bool AddFruit(Collider fruit)
    {
        for (int i = 0; i < m_wagons.Count; i++)
        {
            if (m_wagons[i].cargo == Wagon.Cargo.empty)
            {
                fruit.GetComponent<Collider>().enabled = false;
                fruit.transform.position = wagons[i + 1].position;
                fruit.transform.rotation = Quaternion.identity;
                fruit.transform.parent = wagons[i + 1];
                m_wagons[i].cargo = Wagon.Cargo.fruit;
                m_wagons[i].item = fruit.transform;
                AudioManager.Instance.Play("drop_pickup");
                return true;
            }
        }
        Debug.Log("No place");
        return false;
    }
    public void RemoveFruit(Collider deposit)
    {
        for (int i = 0; i < m_wagons.Count; i++)
        {
            if (m_wagons[i].cargo == Wagon.Cargo.fruit)
            {
                deposit.GetComponent<Collider>().enabled = false;
                m_wagons[i].item.parent = deposit.transform;
                m_wagons[i].item.position = deposit.transform.GetChild(0).position;
                m_wagons[i].item.rotation = deposit.transform.rotation;
                m_wagons[i].cargo = Wagon.Cargo.empty;
                m_wagons[i].item = null;
                GameManager.Instance.AddFruitDelivery();
                AudioManager.Instance.Play("drop_pickup");
                return;
            }
        }
        Debug.Log("No fruit in cargo");
        return;
    }
    public bool AddHuman(Collider human)
    {
        for (int i = 0; i < m_wagons.Count; i++)
        {
            if (m_wagons[i].cargo == Wagon.Cargo.empty)
            {
                human.GetComponent<Collider>().enabled = false;
                human.transform.position = wagons[i + 1].position;
                human.transform.rotation = Quaternion.identity;
                human.transform.parent = wagons[i + 1];
                m_wagons[i].cargo = Wagon.Cargo.human;
                m_wagons[i].item = human.transform;
                AudioManager.Instance.Play("drop_pickup");
                return true;
            }
        }
        Debug.Log("No place");
        return false;
    }
    public void RemoveHuman(Collider deposit)
    {
        for (int i = 0; i < m_wagons.Count; i++)
        {
            if (m_wagons[i].cargo == Wagon.Cargo.human)
            {
                deposit.GetComponent<Collider>().enabled = false;
                m_wagons[i].item.parent = deposit.transform;
                m_wagons[i].item.position = deposit.transform.GetChild(0).position;
                m_wagons[i].item.rotation = deposit.transform.rotation;
                m_wagons[i].cargo = Wagon.Cargo.empty;
                m_wagons[i].item = null;
                GameManager.Instance.AddHumanDelivered();
                AudioManager.Instance.Play("drop_pickup");
                return;
            }
        }
        Debug.Log("No human in cargo");
        return;
    }
    [System.Serializable]
    class Wagon
    {
        public enum Cargo
        {
            empty,
            fruit,
            human
        }
        public Cargo cargo;
        public Transform item;
    }
}