using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    [CreateAssetMenu(fileName = "BoundaryData", menuName = "Config/Boundary")]
    public class BoundaryData : ScriptableObject {
        public int width;
        public int height;

        public int rightBound;
        public int bottomBound;
        public int leftBound;

        private void CalculateBoundaries() {
            CalcLeftBound();
            CalcRightBound();
            CalcBottomBound();
        }

        private void CalcLeftBound() {
            leftBound = -Mathf.CeilToInt(width / 2.0f);
        }

        private void CalcRightBound() {
            rightBound =  Mathf.FloorToInt(width / 2.0f) - 1;
        }

        private void CalcBottomBound() {
            bottomBound = -height;
        }

        private void OnEnable() {
            CalculateBoundaries();
        }

        private void OnDisable() {

        }


        private void OnValidate() {
            CalculateBoundaries();
        }
    }
}
