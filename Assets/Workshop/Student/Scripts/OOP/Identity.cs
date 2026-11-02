using UnityEngine;

namespace Solution
{
    public class Identity : MonoBehaviour
    {
        [Header("Identity")]
        public string Name = "IdentityObject";
        public int positionX;
        public int positionY;

        [HideInInspector]
        public OOPMapGenerator mapGenerator;

        protected virtual void Start()
        {
            SetUP();
        }

        public virtual void SetUP()
        {
            // ถูก Override โดยคลาสลูกเพื่อตั้งค่าเริ่มต้นเฉพาะของคลาสนั้นๆ
        }

        public void PrintInfo()
        {
            Debug.Log($"Created {Name} at {positionX}:{positionY}");
        }

        /// <summary>
        /// อัปเดตพิกัด Grid และย้าย Position ของ Transform บน 2D Space
        /// </summary>
        public virtual void UpdatePosition(int newX, int newY)
        {
            positionX = newX;
            positionY = newY;
            transform.position = new Vector3(newX, newY, transform.position.z);
        }

        /// <summary>
        /// ถูกเรียกเมื่อมีวัตถุอื่น (เช่น Player) เดินเข้ามาชนหรือกระทำ
        /// </summary>
        /// <returns>True = อนุญาตให้เดินทับช่องนี้ได้, False = เดินทับไม่ได้</returns>
        public virtual bool Hit()
        {
            return false;
        }
    }
}