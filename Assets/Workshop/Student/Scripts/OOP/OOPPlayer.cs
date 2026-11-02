using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Solution
{
    public class OOPPlayer : Character
    {
        public Inventory inventory;
        public ActionHistoryManager actionHistoryManager;
        private InputAction moveAction;
        private InputAction fireAction;

        public bool isAutoMoving = false; // Flag to control auto-movement

        public override void SetUP()
        {
            base.SetUP();
            PrintInfo();
            GetRemainEnergy();

            // 1. ดึง Inventory component (ถ้ายังไม่ได้ assign ใน Inspector)
            if (inventory == null)
            {
                inventory = GetComponent<Inventory>();
            }

            // 2. ดึง ActionHistoryManager (ถ้ายังไม่ได้ assign ให้หาในวัตถุเดียวกันหรือค้นหาใน Scene)
            if (actionHistoryManager == null)
            {
                actionHistoryManager = GetComponent<ActionHistoryManager>() ?? FindFirstObjectByType<ActionHistoryManager>();
            }

            // 3. กำหนดค่า Input System
            if (InputSystem.actions != null)
            {
                moveAction = InputSystem.actions.FindAction("Move");
                fireAction = InputSystem.actions.FindAction("Attack");
            }
        }

        private void Start()
        {
            // สั่ง Setup อัตโนมัติเมื่อเริ่มเกม
            SetUP();
        }

        public void Update()
        {
            // Manual input is only processed if not in auto-move mode
            if (isAutoMoving)
            {
                return;
            }

            if (moveAction != null && moveAction.triggered)
            {
                Move(moveAction.ReadValue<Vector2>());
            }

            Keyboard keyboard = Keyboard.current;
            bool firePressed = fireAction != null && fireAction.triggered;

            if (keyboard != null)
            {
                firePressed |= keyboard.eKey.wasPressedThisFrame;
            }

            if (firePressed)
            {
                UseFireStorm();
            }

            if (keyboard == null)
            {
                return;
            }

            // เพิ่มการเช็ค actionHistoryManager != null เพื่อป้องกัน NullReferenceException
            if (actionHistoryManager != null)
            {
                if (keyboard.zKey.wasPressedThisFrame)
                {
                    actionHistoryManager.UndoLastMove(this);
                }
                if (keyboard.xKey.wasPressedThisFrame)
                {
                    actionHistoryManager.RedoLastMove(this);
                }
                if (keyboard.qKey.wasPressedThisFrame)
                {
                    actionHistoryManager.StartAutoMoveSequence(this);
                }
            }
        }

        public override void Move(Vector2 direction)
        {
            base.Move(direction);

            // เช็ค mapGenerator ป้องกัน NullReferenceException
            if (mapGenerator != null)
            {
                mapGenerator.MoveEnemies();
            }

            // Save the state AFTER the move
            if (actionHistoryManager != null)
            {
                actionHistoryManager.SaveStateForUndo(new Vector2(positionX, positionY));
            }
        }

        public void UseFireStorm()
        {
            if (inventory != null && inventory.HasItem("FireStorm", 1))
            {
                inventory.UseItem("FireStorm", 1);

                if (mapGenerator != null)
                {
                    OOPEnemy[] enemies = UtilitySortEnemies.SortEnemiesByRemainningEnergy1(mapGenerator);

                    if (enemies != null)
                    {
                        int count = Mathf.Min(3, enemies.Length);
                        for (int i = 0; i < count; i++)
                        {
                            if (enemies[i] != null)
                            {
                                enemies[i].TakeDamage(10);
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("No FireStorm in inventory");
            }
        }

        public void Attack(OOPEnemy _enemy)
        {
            if (_enemy != null)
            {
                _enemy.TakeDamage(AttackPoint);
                Debug.Log(_enemy.name + " is energy " + _enemy.energy);
            }
        }

        protected override void CheckDead()
        {
            base.CheckDead();
            if (energy <= 0)
            {
                Debug.Log("Player is Dead");
            }
        }
    }
}