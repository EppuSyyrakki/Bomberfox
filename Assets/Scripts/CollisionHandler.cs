﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bomberfox.Enemies;
using Bomberfox.Player;

namespace Bomberfox
{
    public class CollisionHandler : MonoBehaviour
    {
        /// <summary>
        /// Checks a position for colliders.
        /// </summary>
        /// <param name="position">The position to check for colliders</param>
        /// <returns>true if position is free</returns>
        public bool CheckPosition(Vector3 position)
        {
            Vector2 positionToCheck = new Vector2(position.x, position.y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(positionToCheck, 0.4f);

            // draw debug line to checked place TODO remove debug tool
            Debug.DrawLine(transform.position, position, Color.red, 0.1f);  
            
            bool isFree = true;

            if ((colliders).Length > 0)
            {
                foreach (Collider2D collider in colliders)
                {
	                isFree = CompareTags(collider);

	                if (!isFree) break; // if a non-free object found, don't look further
                }
            }

            return isFree;
        }

        /// <summary>
        /// Examines tag of collider and if needed, compares it with gameObject this component is attached to.
        /// </summary>
        /// <param name="collider">The collider to extract the tag from</param>
        /// <returns>True if the gameObject can be moved over, false if movement blocked</returns>
        private bool CompareTags(Collider2D collider)
        {
	        GameObject o = collider.gameObject; // what we collided with

	        if (o.CompareTag("Block")) return false;

	        if (o.CompareTag("Bomb") && gameObject.CompareTag("Player")) return false;
	        	        
	        if (o.CompareTag("Bomb") && gameObject.CompareTag("Enemy")) return CheckBombType(o);

            if (o.CompareTag("Bomb") && gameObject.CompareTag("ShockWave")) return KillBomb(o);

	        if (o.CompareTag("Enemy") && gameObject.CompareTag("ShockWave") && collider.isTrigger) return KillEnemy(o);

	        if (o.CompareTag("Obstacle") && gameObject.CompareTag("ShockWave")) return KillObstacle(o);

	        if (o.CompareTag("Obstacle") && gameObject.CompareTag("Explosion")) return KillObstacle(o);

            if (o.CompareTag("Obstacle") && gameObject.CompareTag("Player")) return false;

            if (o.CompareTag("Player") && gameObject.CompareTag("ShockWave")) return TakeDamage(o);

            if (o.CompareTag("Obstacle") && gameObject.CompareTag("Enemy")) return false;

            if (o.CompareTag("Enemy") && gameObject.CompareTag("Enemy")) return false;

            if (o.CompareTag("Reserved") && gameObject.CompareTag("Enemy")) return false;

            return true;
        }

        private bool CheckBombType(GameObject o)
        {
	        if (o.TryGetComponent(out Bomb bomb))
	        {
		        if (bomb.type == Bomb.Type.Mine) return true;
	        }

	        return false;
        }

        public bool CheckPigCharge(Vector3 position)
        {
	        Vector2 positionToCheck = new Vector2(position.x, position.y);
	        Collider2D[] colliders = Physics2D.OverlapCircleAll(positionToCheck, 0.25f);
	        bool isFree = true;

            if ((colliders).Length > 0)
	        {
		       foreach (Collider2D collider in colliders)
		       {
			       GameObject o = collider.gameObject;

			       if (o.CompareTag("Block")) isFree = false;

			       if (o.CompareTag("Bomb")) isFree = KillBomb(o);

			       if (o.CompareTag("Enemy")) isFree = false;

			       if (o.CompareTag("Obstacle")) isFree = KillObstacleWithEnemy(o);

			       if (o.CompareTag("ShockWave"))
			       {
				       gameObject.GetComponent<Enemy>().StartDeath();
				       isFree = false;
			       }

			       if (!isFree) break;
		       }
	        }

	        return isFree;
        }

        private bool EnemyGoBack(GameObject o)
        {
	        Enemy enemy = o.GetComponent<Enemy>();
            enemy.GoBack();
            return true;
        }

        private bool KillObstacle(GameObject o)
        {
	        Obstacle obstacle = o.GetComponent<Obstacle>();
	        ShockWave shockWave = GetComponent<ShockWave>();
	        GameManager.Instance.DestroyedBlocks++;
	        obstacle.BlowUp(shockWave.Direction);

	        if (shockWave.Penetration) shockWave.Blocked = false;
	        else shockWave.Blocked = true;

            return true;
        }

        private bool KillObstacleWithEnemy(GameObject o)
        {
	        Obstacle obstacle = o.GetComponent<Obstacle>();
	        Enemy enemy = GetComponent<Enemy>();
	        if (enemy.IsAlive) obstacle.BlowUp(enemy.Direction);
	        return false;
        }

        private bool KillBomb(GameObject o)
        {
	        o.GetComponent<Bomb>().Explode();
	        return false;
        }

        private bool KillEnemy(GameObject o)
        {
	        Enemy enemy = o.GetComponent<Enemy>();
	        ShockWave shockWave = gameObject.GetComponent<ShockWave>();

            if (enemy.IsAlive)
            {
                enemy.StartDeath();
            }

            if (shockWave.Penetration) shockWave.Blocked = false;
            else shockWave.Blocked = true;

            return true;
        }

        private bool TakeDamage(GameObject o)
        {
            PlayerController player = o.GetComponent<PlayerController>();

            if (!player.isInvulnerable)
            {
                player.TakeDamage();
            }

            if (gameObject.TryGetComponent(out ShockWave shockWave))
            {
	            shockWave.Blocked = true;
            }

            return false;
        }
    }
}
