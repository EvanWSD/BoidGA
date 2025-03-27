using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BoidSettings", menuName = "Boid/Settings")]
public class BoidSettings : ScriptableObject {
    [Header("Movement")]
    public float minSpeed = 2;
    public float maxSpeed = 5;
    public float maxTurnRate = 0.26f;

    [Header("Classic Principles")]
    [FormerlySerializedAs("seperateWeight")] public float separationWeight = 1;
    public float alignWeight = 1;
    public float cohesionWeight = 1;

    [Header("Perception")]
    public float perceptionRadius = 2.5f;
    public float separationRadius = 0.75f;
    public float fovAngle = 90f;
    public float rayCount = 10f;

    [FormerlySerializedAs("centralTendencyWeight")] [Header("Central Tendency")]
    public float CTWeight = 20f;
    public float CTIgnoreRadius = 10f;

    [Header ("Obstacle Avoidance")]
    public LayerMask obstacleMask;
    public float avoidCollisionWeight = 10;
    public float collisionAvoidDst = 5;

    [Header("Visualisation")]
    public bool visSeparation;
    public bool visAlignment;
    public bool visCohesion;


}