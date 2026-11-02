using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solution
{
    public class ZombieParade : OOPEnemy
    {
        private LinkedList<GameObject> Parade = new LinkedList<GameObject>();

        [Header("Zombie Parade Settings")]
        public int SizeParade = 3;
        public GameObject[] bodyPrefab;   // Prefab ของส่วนลำตัว
        public float moveInterval = 0.5f; // เวลาการเดินแต่ละช่วง

        private int timer = 0;
        private Vector3 moveDirection;

        public override void SetUP()
        {
            base.SetUP();
            moveDirection = Vector3.up;

            // 1. ตั้งค่าพิกัดเริ่มต้นให้ถูกต้องทั้ง X และ Y
            positionX = Mathf.RoundToInt(transform.position.x);
            positionY = Mathf.RoundToInt(transform.position.y);

            StartCoroutine(MoveParade());
        }

        private Vector3 RandomizeDirection()
        {
            List<Vector3> possibleDirections = new List<Vector3>
            {
                Vector3.up,
                Vector3.down,
                Vector3.left,
                Vector3.right
            };

            return possibleDirections[Random.Range(0, possibleDirections.Count)];
        }

        IEnumerator MoveParade()
        {
            // เพิ่มส่วนหัวเข้างูเป็นตัวแรก
            Parade.AddFirst(this.gameObject);

            while (isAlive)
            {
                yield return new WaitForSeconds(moveInterval);

                // สุ่มหาทิศทางที่ไม่ชน
                int toX = positionX;
                int toY = positionY;
                bool isCollide = true;
                int countTryToFind = 0;

                while (isCollide && countTryToFind < 10)
                {
                    moveDirection = RandomizeDirection();
                    toX = positionX + (int)moveDirection.x;
                    toY = positionY + (int)moveDirection.y;

                    isCollide = IsCollision(toX, toY);
                    countTryToFind++;
                }

                // ถ้าสุ่มทิศทางได้สำเร็จ (ไม่ชน)
                if (!isCollide)
                {
                    // เก็บพิกัดเดิมของส่วนหัวไว้เพื่อให้ตัวถัดไปตามมา
                    Vector3 previousPosition = transform.position;
                    int prevX = positionX;
                    int prevY = positionY;

                    // 1. ย้ายตำแหน่งส่วนหัว (Head/This)
                    if (mapGenerator != null && mapGenerator.mapdata != null)
                    {
                        mapGenerator.mapdata[positionX, positionY] = null;
                        positionX = toX;
                        positionY = toY;
                        transform.position = new Vector3(positionX, positionY, 0);
                        mapGenerator.mapdata[positionX, positionY] = this;
                    }

                    // 2. ให้ลำตัวส่วนถัดๆ ไปขยับตามตำแหน่งเดิมของส่วนหน้า
                    LinkedListNode<GameObject> current = Parade.First.Next;
                    while (current != null)
                    {
                        Vector3 tempPos = current.Value.transform.position;
                        current.Value.transform.position = previousPosition;

                        // อัปเดตพิกัด Identity ของส่วนลำตัว
                        Identity bodyIdentity = current.Value.GetComponent<Identity>();
                        if (bodyIdentity != null)
                        {
                            bodyIdentity.positionX = (int)previousPosition.x;
                            bodyIdentity.positionY = (int)previousPosition.y;
                        }

                        previousPosition = tempPos;
                        current = current.Next;
                    }

                    // 3. จัดการเรื่องการงอก/เพิ่มขนาดขบวน (Grow)
                    if (Parade.Count < SizeParade)
                    {
                        timer++;
                        if (timer >= 3)
                        {
                            Grow(previousPosition);
                            timer = 0;
                        }
                    }
                }
            }
        }

        private bool IsCollision(int x, int y)
        {
            if (mapGenerator == null) return true;

            // ตรวจสอบว่าพิกัดออกนอกขอบเขตแมพหรือไม่
            if (x < 0 || x >= mapGenerator.X || y < 0 || y >= mapGenerator.Y)
            {
                return true;
            }

            // ตรวจสอบสิ่งกีดขวาง
            return HasPlacement(x, y);
        }

        private void Grow(Vector3 spawnPosition)
        {
            if (bodyPrefab == null || bodyPrefab.Length == 0) return;

            int x = (int)spawnPosition.x;
            int y = (int)spawnPosition.y;

            int r = Random.Range(0, bodyPrefab.Length);
            GameObject newPart = Instantiate(bodyPrefab[r], spawnPosition, Quaternion.identity);

            if (mapGenerator != null)
            {
                mapGenerator.SetUpItem(x, y, newPart, mapGenerator.enemyParent, mapGenerator.enemy);
            }

            // เพิ่มส่วนใหม่ต่อท้ายใน LinkedList
            Parade.AddLast(newPart);
        }
    }
}
