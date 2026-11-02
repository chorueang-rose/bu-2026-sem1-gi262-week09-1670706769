using UnityEngine;

namespace Solution
{
    public class OOPItemPotion : Identity
    {
        public int healPoint = 10;
        public bool isBonues;

        private void Start()
        {
            // สุ่มโอกาส 20% ที่จะเป็นโบนัสยาโพชั่น
            isBonues = Random.Range(0, 100) < 20;

            if (isBonues)
            {
                // เปลี่ยนสีเป็นสีฟ้าถ้าเป็น Bonus Potion (เช็ค null ป้องกัน Error)
                if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.color = Color.blue;
                }
            }
        }

        public override bool Hit()
        {
            // 1. ตรวจสอบความปลอดภัยของ mapGenerator และ player
            if (mapGenerator != null && mapGenerator.player != null)
            {
                if (isBonues)
                {
                    mapGenerator.player.Heal(healPoint, isBonues);
                    Debug.Log($"You got {Name} Bonus : {healPoint * 2}");
                }
                else
                {
                    mapGenerator.player.Heal(healPoint);
                    Debug.Log($"You got {Name} : {healPoint}");
                }

                // 2. เคลียร์ตำแหน่งเดิมของโพชั่นใน MapData
                if (mapGenerator.mapdata != null)
                {
                    mapGenerator.mapdata[positionX, positionY] = null;
                }

                // 3. ย้ายตำแหน่งผู้เล่นมายังตำแหน่งไอเท็มที่เก็บ
                mapGenerator.player.UpdatePosition(positionX, positionY);
            }
            else
            {
                Debug.LogWarning("MapGenerator or Player Reference is missing in OOPItemPotion!");
            }

            // 4. ทำลายไอเท็มออกจาก Scene
            Destroy(gameObject);

            // คืนค่า true เพื่อยืนยันว่าผู้เล่นสามารถเดินเข้ามาในตำแหน่งนี้ได้
            return true;
        }
    }
}