using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Vector2 nextPosVector2 = new Vector2(position.x, position.y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(nextPosVector2, 0.25f);
            bool isFree = true;

            if ((colliders).Length > 0)
            {
                foreach (Collider2D collider in colliders)
                {
	                isFree = CompareTags(collider);
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
	        GameObject o = collider.gameObject;

	        if (o.CompareTag("Block")) return false;

	        if (o.CompareTag("Bomb") && gameObject.CompareTag("Player")) return false;

	        if (o.CompareTag("Bomb") && gameObject.CompareTag("Enemy")) return false;

            if (o.CompareTag("Bomb") && gameObject.CompareTag("Explosion")) return ExplodeBomb(o);

	        if (o.CompareTag("Enemy") && gameObject.CompareTag("Explosion")) return KillEnemy(o);

	        return true;
        }
        
        private bool ExplodeBomb(GameObject o)
        {
            print("Explosion found a bomb");
	        o.GetComponent<Bomb>().Explode();
	        return false;
        }

        private bool KillEnemy(GameObject o)
        {
	        // Get the enemy script and call a kill method
	        return true;
        }
    }
}
