using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Core;

namespace Game {
    [RequireComponent(typeof(Movable))]
    [RequireComponent(typeof(Rotatable))]
    [RequireComponent(typeof(ControlDroppable))]
    public class Controllable : MonoBehaviour {
        public delegate void ActiveDelegate();
        public event ActiveDelegate OnInactive;

        [Header("Scriptable Reference")]
        public FloatReference dropDelay;
        public FloatReference manualDropDelay;
        public BoolReference gamePause;

        [Header("Editor Reference")]
        public ControlDroppable droppable;
        public Movable movable;
        public Rotatable rotatable;

        [Header("Runtime Reference")]
        public List<Block> blocks;

        private bool canControl = true;

        private float dropTimer;
        private float manualDropTimer;

        private void Awake() {
            dropTimer = dropDelay.Value;
            manualDropTimer = 0f;
        }

        // Start is called before the first frame update
        void Start() {
            if(droppable == null) {
                droppable = GetComponent<ControlDroppable>();
            }

            if(movable == null) {
                movable = GetComponent<Movable>();
            }

            if(rotatable == null) {
                rotatable = GetComponent<Rotatable>();
            }
        }

        // Update is called once per frame
        void Update() {
            if (canControl && gamePause.Value == false) {
                GetUserInput();
            }
        }

        public IEnumerator DropCoroutine() {
            while (canControl) {
                if(dropTimer <= 0)
                {
                    if (droppable.CanDrop())
                    {
                        this.droppable.Drop();
                    }
                    else
                    {
                        OnInactive?.Invoke();
                    }
                    dropTimer = dropDelay.Value;
                }

                dropTimer -= Time.deltaTime;
                yield return null;
            }
        }

        public void Setup(List<Block> blocks) {
            this.blocks = blocks;
            rotatable.Setup(blocks);
            movable.Setup(blocks);
            droppable.Setup(blocks);
        }

        public void StartMovement() {
            canControl = true;
            StartCoroutine(DropCoroutine());
        }

        public void StopMovement() {
            canControl = false;
            StopAllCoroutines();
        }

        public void GetUserInput() {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                ButtonUp();
            } 
            else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                ButtonRight();
            } 
            else if (Input.GetKey(KeyCode.DownArrow)) {
                ButtonDown();
            } 
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                ButtonUpDown();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                ButtonLeft();
            }
        }

        public void ButtonUp() {
            //If can regularly rotate
            if (rotatable.CanRotate()) {
                rotatable.RotateBlocks();
            //If can move then rotate
            } else if (movable.CanMove(rotatable.GetDirection())) {
                movable.Move(rotatable.GetDirection());
                rotatable.RotateBlocks();
            }
        }

        public void ButtonRight() {
            if (movable.CanMove(Direction.right)) {
                movable.MoveRight();
            }
        }

        public void ButtonDown() {
            manualDropTimer -= 1.0f * Time.deltaTime;
            if(manualDropTimer <= 0)
            {
                dropTimer = dropDelay.Value;
                manualDropTimer = manualDropDelay.Value;
                if (droppable.CanDrop())
                {
                    droppable.Drop();
                }
                else
                {
                    OnInactive?.Invoke();
                }
            }
        }

        public void ButtonUpDown()
        {
            manualDropTimer = 0f;
        }

        public void ButtonLeft() {
            if (movable.CanMove(Direction.left)) {
                movable.MoveLeft();
            }
        }
    }

}
