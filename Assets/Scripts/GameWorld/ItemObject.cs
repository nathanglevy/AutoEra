using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.GameWorld
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField]
        private int amountOfItem = 10;
        [SerializeField]
        private int amountOfItemAssigned;
        [SerializeField]
        private ItemType itemType;


        public ItemType ItemType
        {
            get { return itemType; }
            set
            {
                itemType = value;
                //do an additional set here (change the image accordingly)
            }
        }
        public int AmountOfItemUnAssigned
        {
            get
            {
                return amountOfItem-amountOfItemAssigned;
            }
        }
        public void AddAmount(int amount)
        {
            Assert.IsTrue(amount >= 0);
            amountOfItem += amount;
            UpdateDisplay();
        }
        public void AssignAmount(int amount)
        {
            Assert.IsTrue(AmountOfItemUnAssigned >= amount);
            amountOfItemAssigned += amount;
        }
        public void RemoveAssignedAmount(int amount)
        {
            Assert.IsTrue(amountOfItem >= amount);
            Assert.IsTrue(amountOfItemAssigned >= amount);
            amountOfItem -= amount;
            amountOfItemAssigned -= amount;
            if (amount <= 0)
                Destroy(this);
            UpdateDisplay();
        }
    
        public Vector3Int GetLocation()
        {
            var localCoords = this.gameObject.transform.position;
            return this.gameObject.GetComponentInParent<Grid>().LocalToCell(localCoords);
        }

        private void UpdateDisplay()
        {
            if (amountOfItem <= 0)
            {
                Destroy(gameObject);
            }
            var textMesh = this.gameObject.GetComponentInChildren<TextMesh>();
            textMesh.text = amountOfItem.ToString();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

    
    }

    public enum ItemType
    {
        Wood,IronOre
    }
}