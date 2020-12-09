using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Playspace : MonoBehaviour {
        [Header("Asset Reference")]
        public GameObject boundaryPrefab;
        public GameObject upperBoundPrefab;

        [Header("Scriptable Reference")]
        public BoundaryData boundary;

        private GameObject upperBound;
        private GameObject leftBound;
        private GameObject rightBound;
        private GameObject bottomBound;

        public void Setup() {
            CreateUpperBoundary();
            CreateLeftBoundary();
            CreateRightBoundary();
            CreateBottomBoundary();
            CenterPlacespace();
        }

        private void CenterPlacespace() {
            int mod = boundary.width % 2;

            if(mod == 0) {
                this.transform.localPosition = new Vector3(0.5f, 0, 0);
            }
            else if (mod == 1) {
                this.transform.localPosition = new Vector3(1f, 0, 0);
            }
        }

        private void CreateUpperBoundary()
        {
            //Instantiate child object
            upperBound = new GameObject();
            upperBound.name = "Upper Bound";
            upperBound.transform.SetParent(this.transform);

            //Set position of child to boundary
            upperBound.transform.localPosition = new Vector3(boundary.leftBound - 1, -1, 0);

            //Instantiate boundary blocks
            for (int i = 0; i <= boundary.width + 1; i++)
            {
                GameObject obj = Instantiate(upperBoundPrefab);
                obj.transform.SetParent(upperBound.transform);
                obj.transform.localPosition = new Vector3(i, 0, 0);
            }
        }

        private void CreateLeftBoundary() {
            //Instantiate child object
            leftBound = new GameObject();
            leftBound.name = "Left Bound";
            leftBound.transform.SetParent(this.transform);

            //Set position of child to boundary
            leftBound.transform.localPosition = new Vector3(boundary.leftBound - 1, 0, 0);

            //Instantiate boundary blocks
            for(int i = 1; i <= boundary.height; i++) {
                GameObject obj = Instantiate(boundaryPrefab);
                obj.transform.SetParent(leftBound.transform);
                obj.transform.localPosition = new Vector3(0, -i, 0);
            }
        }

        private void CreateRightBoundary() {
            rightBound = new GameObject();
            rightBound.name = "Right Bound";
            rightBound.transform.SetParent(this.transform);

            //Set position of child to boundary
            rightBound.transform.localPosition = new Vector3(boundary.rightBound + 1, 0, 0);

            //Instantiate boundary blocks
            for (int i = 1; i <= boundary.height; i++) {
                GameObject obj = Instantiate(boundaryPrefab);
                obj.transform.SetParent(rightBound.transform);
                obj.transform.localPosition = new Vector3(0, -i, 0);
            }
        }

        private void CreateBottomBoundary() {
            bottomBound = new GameObject();
            bottomBound.name = "Bottom Bound";
            bottomBound.transform.SetParent(this.transform);

            //Set position of child to boundary
            bottomBound.transform.localPosition = new Vector3(boundary.leftBound - 1, boundary.bottomBound - 1, 0);

            //Instantiate boundary blocks
            for (int i = 0; i < boundary.width + 2; i++) {
                GameObject obj = Instantiate(boundaryPrefab);
                obj.transform.SetParent(bottomBound.transform);
                obj.transform.localPosition = new Vector3(i, 0, 0);
            }
        }
    }

}
