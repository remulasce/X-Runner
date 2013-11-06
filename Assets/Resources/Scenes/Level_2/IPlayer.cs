using UnityEngine;
using System.Collections;


//Common player interface for both L2 and L4
public interface IPlayer {

	bool IsDead();
    Vector3 GetPosition();
}
