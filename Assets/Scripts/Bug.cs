using System;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour {
    [SerializeField] float speed;

    bool isMoving;
    List<Vector3> waypoints;
    int wpIndex;
    Vector3 direction;

    void Update() {
        Move();
    }

    void Move() {
        if (!isMoving) return;

        if ((waypoints[wpIndex] - transform.position).sqrMagnitude < (speed * Time.deltaTime).ToSqr()) {
            transform.position = waypoints[wpIndex];
            wpIndex++;
            
            if (wpIndex >= waypoints.Count) {
                isMoving = false;
                EditorConsole.Log("Reached target");
                return;
            }

            direction = (waypoints[wpIndex] - transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        transform.position += direction * speed * Time.deltaTime;
    }

    public void StartMove(List<Vector3> waypoints) {
        this.waypoints = waypoints;
        wpIndex = 0;
        isMoving = true;
    }

    public void StopMove() {
        isMoving = false;
    }
}