using TMPro;
using UnityEngine;

namespace Solution
{
    public class OOPEnemy : Character
    {
        public TMP_Text txtHp;

        private void Start()
        {
            SetUP();
        }

        public override void SetUP()
        {
            base.SetUP();
            PrintInfo();
            GetRemainEnergy();

            if (txtHp != null)
            {
                txtHp.text = energy.ToString();
            }
        }

        public override bool Hit()
        {
            if (mapGenerator == null || mapGenerator.player == null) return false;

            // 1. ผู้เล่นโจมตีศัตรูก่อน
            mapGenerator.player.Attack(this);

            // 2. ถ้าศัตรูยังไม่ตาย (energy > 0) ถึงจะโจมตีสวนกลับ
            if (energy > 0)
            {
                Attack(mapGenerator.player);
            }

            return false; // เดินทับช่องนี้ไม่ได้
        }

        public void Attack(OOPPlayer _player)
        {
            if (_player != null)
            {
                _player.TakeDamage(AttackPoint);
            }
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            if (txtHp != null)
            {
                txtHp.text = energy.ToString();
            }
        }

        protected override void CheckDead()
        {
            if (energy <= 0)
            {
                // เคลียร์ตำแหน่งในแมพเมื่อศัตรูตาย
                if (mapGenerator != null)
                {
                    if (mapGenerator.mapdata != null &&
                        positionX >= 0 && positionX < mapGenerator.X &&
                        positionY >= 0 && positionY < mapGenerator.Y)
                    {
                        if (mapGenerator.mapdata[positionX, positionY] == (Identity)this)
                        {
                            mapGenerator.mapdata[positionX, positionY] = null;
                        }
                    }

                    if (mapGenerator.EnemysOnMap != null)
                    {
                        mapGenerator.EnemysOnMap.Remove(this);
                    }
                }

                Destroy(gameObject);
            }
        }

        public void RandomMove()
        {
            if (mapGenerator == null) return;

            int toX = positionX;
            int toY = positionY;
            int random = Random.Range(0, 4);

            switch (random)
            {
                case 0: toY += 1; break; // Up
                case 1: toY -= 1; break; // Down
                case 2: toX -= 1; break; // Left
                case 3: toX += 1; break; // Right
            }

            // ตรวจสอบว่าพิกัดใหม่อยู่ภายในขอบเขตของ Map
            if (toX >= 0 && toX < mapGenerator.X && toY >= 0 && toY < mapGenerator.Y)
            {
                // ตรวจสอบว่าช่องใหม่ไม่มีวัตถุอื่นอยู่ (เป็น null)
                if (mapGenerator.mapdata[toX, toY] == null)
                {
                    // เคลียร์ตำแหน่งเดิมใน MapData
                    mapGenerator.mapdata[positionX, positionY] = null;

                    // อัปเดตพิกัดและ Transform
                    UpdatePosition(toX, toY);

                    // จองตำแหน่งใหม่ใน MapData
                    mapGenerator.mapdata[toX, toY] = this;
                }
            }
        }
    }
}