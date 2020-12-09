using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;

namespace Game {
    public class Droppable : MonoBehaviour, IDroppable {
        public delegate void DropDelegate();
        public event DropDelegate OnDoneDropping;

        [Header("Scriptable Reference")]
        public BoundaryData boundary;
        public BoardSpace board;

        public void Drop() {
            transform.localPosition += Vector3.down;
        }

        public bool CanDrop() {
            //Check if reached the bottom
            int yPos = Mathf.RoundToInt(transform.localPosition.y);
            if (yPos <= boundary.bottomBound) {
                return false;
            }

            //Check for other blocks
            int xPos = Mathf.RoundToInt(transform.localPosition.x);
            if (board.IsOccupied(xPos, yPos - 1)) {
                return false;
            }

            return true;
        }

        public void DoneDropping()
        {
            OnDoneDropping?.Invoke();
        }

        public Transform GetTransform()
        {
            return this.transform;
        }
    }
}
