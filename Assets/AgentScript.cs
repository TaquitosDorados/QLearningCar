using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentScript : Agent
{
    public Transform[] checkpoints;
    private int nextCheckpointIndex = 0;
    private CarScript car;
    private Vector3 startPoint;

    private void Start()
    {
        car = GetComponent<CarScript>();
        startPoint = transform.position;
    }
    public override void OnEpisodeBegin()
    {
        // Reset the car's position and velocity
        transform.position = startPoint;
        car.ResetVel();
        nextCheckpointIndex = 0;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Add car's velocity and direction to observations
        sensor.AddObservation(car.rb.velocity.magnitude);
        sensor.AddObservation(transform.right);

        // Add the direction to the next checkpoint
        Vector3 directionToCheckpoint = (checkpoints[nextCheckpointIndex].position - transform.position).normalized;
        sensor.AddObservation(directionToCheckpoint);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.DiscreteActions[0] == 0)
        {
            car.Backward();
        }
        else if (actions.DiscreteActions[0] == 1)
        {
            car.Forward();
        }

        if (actions.DiscreteActions[1] == 0)
        {
            car.Steer(1);
        }
        else if (actions.DiscreteActions[1] == 1)
        {
            car.Steer(-1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kill"))
        {
            AddReward(-1f);
            Debug.Log("kill");
            EndEpisode();
        } else if (collision.gameObject == checkpoints[nextCheckpointIndex].gameObject)
        {
            AddReward(1f);
            Debug.Log("si");
            nextCheckpointIndex = (nextCheckpointIndex + 1) % checkpoints.Length;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        if(Input.GetKeyDown(KeyCode.W))
        {
            discreteActions[0] = 1;
            Debug.Log("a");
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            discreteActions[0] = 0;
        }
        //discreteActions[1] = (int)Input.GetAxis("Horizontal");
    }
}
