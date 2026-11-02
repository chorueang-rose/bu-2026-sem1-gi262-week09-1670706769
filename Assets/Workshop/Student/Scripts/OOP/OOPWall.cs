using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solution
{
    public class OOPWall : Identity
    {
        public int Damage = 1;
        public bool IsIceWall;

        private void Start()
        {
            // สุ่มโอกาส 20% ที่จะเป็นกำแพงน้ำแข็ง
            IsIceWall = Random.Range(0, 100) < 20;

            if (IsIceWall)
            {
                // เปลี่ยนสีเป็นสีฟ้าถ้าเป็น IceWall (เช็ค null ป้องกัน MissingComponentException)
                if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.color = Color.blue;
                }
            }
        }

        public override bool Hit()
        {
            // 1. เช็คความปลอดภัยของ mapGenerator และ player
            if (mapGenerator != null && mapGenerator.player != null)
            {
                if (IsIceWall)
                {
                    mapGenerator.player.TakeDamage(Damage, IsIceWall);
                }
                else
                {
                    mapGenerator.player.TakeDamage(Damage);
                }

                // 2. เคลียร์ตำแหน่งกำแพงในแมพ
                if (mapGenerator.mapdata != null)
                {
                    mapGenerator.mapdata[positionX, positionY] = null;
                }
            }
            else
            {
                Debug.LogWarning("MapGenerator or Player Reference is missing in OOPWall!");
            }

            // 3. ทำลายกำแพงออกจาก Scene
            Destroy(gameObject);

            // คืนค่า false (ผู้เล่นเดินเข้าช่องนี้ไม่ได้ หรือไม่ผ่าน)
            return false;
        }
    }
}