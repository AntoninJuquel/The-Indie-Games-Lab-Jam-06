using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainChecker : MonoBehaviour
{
    Train train;
    TrainEffects trainEffects;
    [SerializeField] Transform locomotive;
    [SerializeField] float detectionDistance;
    [Header("Layer masks")]
    [SerializeField] LayerMask whatIsStart;
    [SerializeField] LayerMask whatIsEnd;
    [SerializeField] LayerMask whatIsTrap;
    [SerializeField] LayerMask whatIsFaster;
    [SerializeField] LayerMask whatIsSlower;
    [SerializeField] LayerMask whatIsTrigger;
    [SerializeField] LayerMask whatIsFruit;
    [SerializeField] LayerMask whatIsHuman;
    [SerializeField] LayerMask whatIsFruitDeposite;
    [SerializeField] LayerMask whatIsHumanDeposite;

    private void Awake()
    {
        trainEffects = GetComponent<TrainEffects>();
        train = GetComponent<Train>();
    }
    public void Checker()
    {
        CheckSpecialNodes();
        CheckFruits();
        CheckHuman();
        CheckFruitsDeposite();
        CheckHumanDeposite();
    }
    public void CheckStartEndNode()
    {
        if (GameManager.Instance.GetTrackStatus() != GameManager.TrackStatus.ended)
        {
            Collider[] endNode = Physics.OverlapSphere(locomotive.position, detectionDistance, whatIsEnd);
            if (endNode.Length > 0)
            {
                GameManager.Instance.EndTrack();
            }
        }
        else
        {
            Collider[] startNode = Physics.OverlapSphere(locomotive.position, detectionDistance, whatIsStart);
            if (startNode.Length > 0)
            {
                GameManager.Instance.ResetTrack();
            }
        }
    }
    void CheckSpecialNodes()
    {
        Collider[] fasterNode = Physics.OverlapSphere(locomotive.position, detectionDistance, whatIsFaster);
        if (fasterNode.Length > 0)
        {
            if (trainEffects.effect != TrainEffects.Effect.fast)
            {
                StartCoroutine(trainEffects.Faster());
            }
        }
        Collider[] slowerNode = Physics.OverlapSphere(locomotive.position, detectionDistance, whatIsSlower);
        if (slowerNode.Length > 0)
        {
            if (trainEffects.effect != TrainEffects.Effect.slow)
            {
                StartCoroutine(trainEffects.Slower());
            }
        }
    }
    void CheckFruits()
    {
        Collider[] fruit = Physics.OverlapSphere(locomotive.position, detectionDistance, whatIsFruit);
        if (fruit.Length > 0)
        {
            train.AddFruit(fruit[0]);
        }
    }
    void CheckFruitsDeposite()
    {
        Collider[] deposit = Physics.OverlapSphere(locomotive.position, detectionDistance, whatIsFruitDeposite);
        if (deposit.Length > 0)
        {
            train.RemoveFruit(deposit[0]);
        }
    }
    void CheckHuman()
    {
        Collider[] human = Physics.OverlapSphere(locomotive.position, detectionDistance, whatIsHuman);
        if (human.Length > 0)
        {
            train.AddHuman(human[0]);
        }
    }
    void CheckHumanDeposite()
    {
        Collider[] deposit = Physics.OverlapSphere(locomotive.position, detectionDistance, whatIsHumanDeposite);
        if (deposit.Length > 0)
        {
            train.RemoveHuman(deposit[0]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(locomotive.position, detectionDistance);
    }

}
