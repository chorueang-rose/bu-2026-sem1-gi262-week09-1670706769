using UnityEngine;

namespace Solution
{
    public class CollectAbleItem : Identity
    {
        public override bool Hit()
        {
            Debug.Log("Item: " + Name + " has been picked up.");

            // 1. ตรวจสอบความปลอดภัยของ mapGenerator, player และ inventory
            if (mapGenerator != null && mapGenerator.player != null)
            {
                if (mapGenerator.player.inventory != null)
                {
                    mapGenerator.player.inventory.AddItem(Name, 1);
                }
                else
                {
                    Debug.LogWarning("Player Inventory Reference is missing!");
                }

                // 2. เคลียร์ตำแหน่งใน mapdata
                if (mapGenerator.mapdata != null)
                {
                    mapGenerator.mapdata[positionX, positionY] = null;
                }

                // 3. ย้ายตำแหน่งผู้เล่นมายังช่องไอเท็มที่เก็บ
                mapGenerator.player.UpdatePosition(positionX, positionY);
            }
            else
            {
                Debug.LogWarning("MapGenerator or Player Reference is missing in CollectAbleItem!");
            }

            // 4. ทำลายไอเท็มออกจากฉาก
            Destroy(gameObject);

            // คืนค่า true เพื่อยืนยันว่าผู้เล่นสามารถเดินเข้าช่องนี้ได้
            return true;
        }
    }
}

