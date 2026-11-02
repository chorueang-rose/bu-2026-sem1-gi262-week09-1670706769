using System.Collections.Generic;
using UnityEngine;

namespace Solution
{
    public class Inventory : MonoBehaviour
    {
        public Dictionary<string, int> inventory = new Dictionary<string, int>();

        // เพิ่มไอเท็ม
        public void AddItem(string item, int amount)
        {
            if (amount <= 0) return;

            // 1. ตรวจสอบว่ามีไอเท็มนี้ในคลังแล้วหรือยัง
            if (inventory.ContainsKey(item))
            {
                inventory[item] += amount;
            }
            else
            {
                // ถ้ายังไม่มี ให้เพิ่มไอเท็มใหม่เข้าไปใน Dictionary
                inventory.Add(item, amount);
            }

            Debug.Log($"Added {amount} {item}. Total: {inventory[item]}");
        }

        // ลบ/ใช้ไอเท็ม
        public void UseItem(string item, int amount)
        {
            if (amount <= 0) return;

            // 1. ตรวจสอบว่ามีไอเท็มเพียงพอต่อการใช้หรือไม่ก่อนทำการหัก
            if (HasItem(item, amount))
            {
                inventory[item] -= amount;

                // ถ้าจำนวนไอเท็มเหลือ 0 ให้ลบออกจาก Dictionary
                if (inventory[item] <= 0)
                {
                    inventory.Remove(item);
                    Debug.Log($"Removed all {item} from inventory.");
                }
                else
                {
                    Debug.Log($"Removed {amount} {item}. Remaining: {inventory[item]}");
                }
            }
            else
            {
                Debug.LogWarning($"Cannot use {item}. Not enough amount or item not found! (Required: {amount}, Has: {GetItemCount(item)})");
            }
        }

        // ตรวจสอบว่ามีไอเท็มในคลังเพียงพอหรือไม่
        public bool HasItem(string item, int amount)
        {
            return inventory.ContainsKey(item) && inventory[item] >= amount;
        }

        // ตรวจสอบจำนวนไอเท็มที่มี
        public int GetItemCount(string item)
        {
            if (inventory.TryGetValue(item, out int count))
            {
                return count;
            }
            return 0;
        }

        // แสดงรายการทั้งหมดในคลัง
        public void PrintInventory()
        {
            Debug.Log("--- Inventory Content ---");
            if (inventory.Count == 0)
            {
                Debug.Log("Inventory is empty.");
                return;
            }

            foreach (var itemEntry in inventory)
            {
                Debug.Log($"{itemEntry.Key}: {itemEntry.Value}");
            }
        }
    }
}
