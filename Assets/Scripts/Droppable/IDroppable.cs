using System;
using UnityEngine;

namespace Game {
    public interface IDroppable {
        bool CanDrop();
        void Drop();

        void DoneDropping();

        Transform GetTransform();
    }
}
