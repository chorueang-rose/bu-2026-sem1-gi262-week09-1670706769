using Searching;
using UnityEngine;

namespace Solution
{
    public class OOPExit : Identity
    {
        public Leaderboard leaderboard;
        public string ItemToOpen = "Key";
        public int ItemAmountToOpen = 2;

        private void Start()
        {
            // ค้นหา Leaderboard อัตโนมัติถ้ายังไม่ได้ assign ใน Inspector
            if (leaderboard == null)
            {
                leaderboard = FindFirstObjectByType<Leaderboard>();
            }
        }

        public override bool Hit()
        {
            // 1. ตรวจสอบความปลอดภัยของ References
            if (mapGenerator == null || mapGenerator.player == null || mapGenerator.player.inventory == null)
            {
                Debug.LogWarning("MapGenerator, Player, or Inventory is missing in OOPExit!");
                return false;
            }

            // 2. ตรวจสอบว่าผู้เล่นมีไอเท็มเพียงพอหรือไม่
            bool isHasItemAmount = mapGenerator.player.inventory.HasItem(ItemToOpen, ItemAmountToOpen);

            if (isHasItemAmount)
            {
                mapGenerator.player.inventory.UseItem(ItemToOpen, ItemAmountToOpen);

                if (leaderboard != null)
                {
                    leaderboard.gameObject.SetActive(true);

                    Debug.Log("You win!");

                    int scoreReceived = CalculateScore();
                    string playerName = string.IsNullOrEmpty(mapGenerator.player.Name) ? "Player" : mapGenerator.player.Name;

                    leaderboard.RecordScore(new PlayerScore(playerName, scoreReceived));
                    leaderboard.PrintScores();
                    leaderboard.ShowleaderBoard();
                }
                else
                {
                    Debug.LogWarning("Leaderboard Reference is missing!");
                }

                // 3. เคลียร์ตำแหน่งทางออกใน MapData
                if (mapGenerator.mapdata != null)
                {
                    mapGenerator.mapdata[positionX, positionY] = null;
                }

                // 4. อัปเดตตำแหน่งผู้เล่นมายังทางออก
                mapGenerator.player.UpdatePosition(positionX, positionY);

                return true;
            }
            else
            {
                Debug.Log($"Need Item {ItemToOpen} x{ItemAmountToOpen} to Open");
                return false;
            }
        }

        private int CalculateScore()
        {
            if (mapGenerator == null || mapGenerator.player == null) return 0;

            // ป้องกันการหารด้วย 0 หรือค่าน้อยกว่า 1 วินาที
            float elapsedTime = Mathf.Max(Time.time, 1f);
            int score = (int)((mapGenerator.player.energy * 100) / elapsedTime);
            return score;
        }
    }
}