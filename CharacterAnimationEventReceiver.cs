using UnityEngine;

    public class CharacterAnimationEventReceiver : MonoBehaviour
    {
        public Character NotifiedCharacter;

        private void Hit()
        {
            NotifiedCharacter.SendMessage("StopBasicAttack");
        }

        private void FootL()
        {
            
        }

        private void FootR()
        {
            
        }
    }
