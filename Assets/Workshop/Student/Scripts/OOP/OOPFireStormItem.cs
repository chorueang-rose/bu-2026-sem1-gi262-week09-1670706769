using UnityEngine;

namespace Solution
{
    public class OOPFireStormItem : Identity
    {
        public override bool Hit()
        {
            // 1. ตรวจสอบความปลอดภัยของ mapGenerator, player และ inventory
            if (mapGenerator != null && mapGenerator.player != null)
            {
                if (mapGenerator.player.inventory != null)
                {
                    mapGenerator.player.inventory.AddItem("FireStorm", 1);
                    Debug.Log("FireStorm item picked up!");
                }
                else
                {
                    Debug.LogWarning("Player Inventory is missing!");
                }

                // 2. เคลียร์พิกัดใน mapdata
                if (mapGenerator.mapdata != null)
                {
                    mapGenerator.mapdata[positionX, positionY] = null;
                }

                // 3. อัปเดตตำแหน่งของผู้เล่นมายังช่องไอเท็มนี้
                mapGenerator.player.UpdatePosition(positionX, positionY);
            }
            else
            {
                Debug.LogWarning("MapGenerator or Player Reference is missing in OOPFireStormItem!");
            }

            // 4. ทำลาย GameObject ออกจากฉาก
            Destroy(gameObject);

            // คืนค่า true เพื่อให้ผู้เล่นเดินเข้าช่องนี้ได้
            return true;
        }
    }
}