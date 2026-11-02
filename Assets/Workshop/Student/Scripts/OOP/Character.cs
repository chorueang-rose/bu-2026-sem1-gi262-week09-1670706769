using UnityEngine;

namespace Solution
{
    public class Character : Identity
    {
        [Header("Character Properties")]
        public int energy;
        public int maxEnergy = 100;
        public int AttackPoint = 10;

        protected bool isAlive;
        protected bool isFreeze;

        [Header("Sprite Settings")]
        public SpriteRenderer spriteRenderer;

        [Header("Color Energy Levels")]
        public Color normalColor = Color.white;    // สีปกติ (HP > 66%)
        public Color damagedColor1 = Color.yellow; // ได้รับความเสียหายระดับ 1 (HP 34%-66%)
        public Color damagedColor2 = Color.red;    // ได้รับความเสียหายระดับ 2 (HP <= 33%)
        public Color freezeColor = Color.blue;     // สีเมื่อติดสถานะ Freeze

        public override void SetUP()
        {
            base.SetUP();
            isAlive = true;
            isFreeze = false;

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            energy = maxEnergy;
            UpdateSpriteColorBasedOnHealth();
            Debug.Log("SetUP " + Name);
        }

        protected void GetRemainEnergy()
        {
            Debug.Log(name + " : " + energy);
        }

        public virtual void Move(Vector2 direction)
        {
            // หากติด Freeze จะข้ามการเดิน 1 ตา แล้วละลายน้ำแข็งออก
            if (isFreeze)
            {
                isFreeze = false;
                UpdateSpriteColorBasedOnHealth(); // คืนสีกลับตาม HP
                Debug.Log(name + " unfroze!");
                return;
            }

            int toX = positionX + (int)direction.x;
            int toY = positionY + (int)direction.y;

            // ตรวจสอบว่ามีวัตถุอยู่ในช่องเป้าหมายหรือไม่
            if (HasPlacement(toX, toY))
            {
                bool isCanWalkTo = mapGenerator.GetMapData(toX, toY).Hit();
                if (isCanWalkTo)
                {
                    UpdatePosition(toX, toY);
                }
            }
            else
            {
                // ตรวจสอบว่าพิกัดใหม่อยู่ภายในขอบเขตแผนที่ก่อนย้าย
                if (mapGenerator != null && toX >= 0 && toX < mapGenerator.X && toY >= 0 && toY < mapGenerator.Y)
                {
                    UpdatePosition(toX, toY);
                    TakeDamage(1); // โดนความเสียหายกรณีเดินเข้าช่องว่าง
                }
            }
        }

        public override void UpdatePosition(int toX, int toY)
        {
            if (mapGenerator != null && mapGenerator.mapdata != null)
            {
                // เคลียร์ตำแหน่งเดิม
                if (positionX >= 0 && positionX < mapGenerator.X && positionY >= 0 && positionY < mapGenerator.Y)
                {
                    mapGenerator.mapdata[positionX, positionY] = null;
                }

                // อัปเดตพิกัดใหม่
                positionX = toX;
                positionY = toY;
                transform.position = new Vector3(positionX, positionY, transform.position.z);

                // บันทึกตำแหน่งใหม่ลง MapData
                mapGenerator.mapdata[positionX, positionY] = this;
            }
        }

        public bool HasPlacement(int x, int y)
        {
            if (mapGenerator == null) return false;

            var mapData = mapGenerator.GetMapData(x, y);
            return mapData != null && mapData != mapGenerator.Wall;
        }

        public virtual void TakeDamage(int damage)
        {
            energy -= damage;
            Debug.Log(name + " Current Energy : " + energy);
            UpdateSpriteColorBasedOnHealth();
            CheckDead();
        }

        public virtual void TakeDamage(int damage, bool freeze)
        {
            energy -= damage;
            isFreeze = freeze;
            Debug.Log(name + " Current Energy : " + energy);
            Debug.Log(name + " is Frozen!");

            // ปรับสีสไปรต์ตามสถานะ (หาก Freeze ให้เปลี่ยนสีเป็นสีฟ้า)
            UpdateSpriteColorBasedOnHealth();
            CheckDead();
        }

        public void Heal(int healPoint)
        {
            Heal(healPoint, false);
        }

        public void Heal(int healPoint, bool bonus)
        {
            energy += healPoint * (bonus ? 2 : 1);
            if (energy > maxEnergy)
            {
                energy = maxEnergy;
            }

            Debug.Log("Current Energy : " + energy);
            UpdateSpriteColorBasedOnHealth(); // อัปเดตสีสไปรต์หลังได้รับการฟื้นฟู
        }

        protected virtual void CheckDead()
        {
            if (energy <= 0)
            {
                isAlive = false;
                if (mapGenerator != null && mapGenerator.mapdata != null)
                {
                    if (positionX >= 0 && positionX < mapGenerator.X && positionY >= 0 && positionY < mapGenerator.Y)
                    {
                        mapGenerator.mapdata[positionX, positionY] = null;
                    }
                }
                Destroy(gameObject);
            }
        }

        private void UpdateSpriteColorBasedOnHealth()
        {
            if (spriteRenderer == null) return;

            // ถ้าติด Freeze ให้แสดงสี freezeColor เสมอ
            if (isFreeze)
            {
                spriteRenderer.color = freezeColor;
                return;
            }

            float healthPercentage = (float)energy / maxEnergy;

            if (healthPercentage > 0.66f) // HP > 66%
            {
                spriteRenderer.color = normalColor;
            }
            else if (healthPercentage > 0.33f) // HP 34%-66%
            {
                spriteRenderer.color = damagedColor1;
            }
            else // HP <= 33%
            {
                spriteRenderer.color = damagedColor2;
            }
        }
    }
}