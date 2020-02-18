using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : Singleton<LayerManager>
{
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask defaultEnvironmentLayer;
    public LayerMask destructableEnvironmentLayer;
    public LayerMask holeLayer;
}
