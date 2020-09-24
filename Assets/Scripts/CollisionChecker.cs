using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox
{
    public class CollisionChecker : MonoBehaviour
    {
        /// <summary>
        /// Check all colliders in position for tag "Block"
        /// </summary>
        /// <returns>true if no Block tag, false if tag found</returns>
        public bool CheckPosition(Vector3 position)
        {
            Vector2 nextPosVector2 = new Vector2(position.x, position.y);
            Collider2D[] cols = Physics2D.OverlapCircleAll(nextPosVector2, 0.25f);

            if ((cols).Length > 0)
            {
                foreach (var col in cols)
                {
                    if (col.gameObject.CompareTag("Block")) return false;
                }
            }

            return true;
        }
    }
}
