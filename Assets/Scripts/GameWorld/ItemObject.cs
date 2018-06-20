using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.GameWorld
{
    public class ItemObject : MonoBehaviour, IInventory
    {
        [SerializeField] private int amountOfItem = 10;
        [SerializeField] private ItemType _itemType;

        internal List<Commitment> OutgoingCommitments = new List<Commitment>();
        internal List<Commitment> IncomingCommitments = new List<Commitment>();

        public Vector3Int Location
        {
            get { return GetLocation(); }
   
        }

        public ItemType ItemType
        {
            get { return _itemType; }
            set
            {
                if (OutgoingCommitments.Count == 0 && IncomingCommitments.Count == 0)
                    _itemType = value;
                else
                    throw new InventoryException("Cannot change item type, there are already commitments for this item object");

            }
        }
    
        public Vector3Int GetLocation()
        {
            var localCoords = this.gameObject.transform.position;
            return this.gameObject.GetComponentInParent<Grid>().LocalToCell(localCoords);
        }

        private void UpdateDisplay()
        {
            if (GetCurrentAmount(ItemType) == 0 && GetAllIncomingCommits().Count == 0)
            {
                Destroy(gameObject);
            }
            var textMesh = this.gameObject.GetComponentInChildren<TextMesh>();
//            var amountOfItem = GetCurrentAmount();
            textMesh.text = amountOfItem.ToString();
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateDisplay();
        }

        public void AddCommitment(Commitment commitmentToAdd)
        {
            if (commitmentToAdd.committer.Equals(this) && !OutgoingCommitments.Contains(commitmentToAdd))
                OutgoingCommitments.Add(commitmentToAdd);
            if (commitmentToAdd.committedTo.Equals(this) && !IncomingCommitments.Contains(commitmentToAdd))
                IncomingCommitments.Add(commitmentToAdd);
        }

        public void RemoveCommitment(Commitment commitmentToRemove)
        {
            if (commitmentToRemove.committer.Equals(this) && OutgoingCommitments.Contains(commitmentToRemove))
                OutgoingCommitments.Remove(commitmentToRemove);
            if (commitmentToRemove.committedTo.Equals(this) && IncomingCommitments.Contains(commitmentToRemove))
                IncomingCommitments.Remove(commitmentToRemove);
        }

        public void RemoveCommitments(List<Commitment> commitmentsToRemove)
        {
            commitmentsToRemove.ForEach(RemoveCommitment);
        }

        public bool CanAcceptAmount(ItemType itemType, int amount)
        {
            return itemType == ItemType;
        }

        public bool MoveItemFromThisInventoryTo(IInventory targetInventory, ItemType itemType, int amount)
        {
            Commitment mergedCommitement = Commitment.MergeCommitmentsOfThisType(itemType, this, targetInventory);
            if (mergedCommitement == null || mergedCommitement.amount < amount)
            {
                throw new InventoryException(
                    "There is no commitment of this amount: " + amount + " of type " + itemType);
            }

            mergedCommitement.FullfillCommitmentAtomic(amount);
            return true;
        }

        public int GetCurrentAmount(ItemType itemType)
        {
            return (itemType != ItemType) ? 0 : amountOfItem;
        }

        public int GetCurrentUncommittedAmount(ItemType itemType)
        {
            if (itemType != ItemType)
                return 0;
            //TODO- can make this a bit more fficient cause we can assume all items in list are of this type...?
            List<Commitment> outGoingCommitsOfType = OutgoingCommitments.Where(it => it.itemType == itemType).ToList();
            int committedAmount = outGoingCommitsOfType.Sum(it => it.amount);
            return GetCurrentAmount(itemType) - committedAmount;
        }

        public List<Commitment> GetAllOutgoingCommits()
        {
            return OutgoingCommitments.ToList();
        }

        public List<Commitment> GetCommitmentsToInventory(IInventory targetInventory)
        {
            return OutgoingCommitments.FindAll(comm => comm.committedTo == targetInventory);
        }

        public List<Commitment> GetCommitmentsToInventory(IInventory targetInventory, ItemType itemType)
        {
            return OutgoingCommitments.FindAll(comm => comm.committedTo == targetInventory && comm.itemType == itemType);
        }

        public List<Commitment> GetAllIncomingCommits()
        {
            return IncomingCommitments.ToList();
        }

        public List<Commitment> GetCommitmentsFromInventory(IInventory sourceInventory)
        {
            return IncomingCommitments.FindAll(comm => comm.committer == sourceInventory);
        }

        public List<Commitment> GetCommitmentsFromInventory(IInventory sourceInventory, ItemType itemType)
        {
            return IncomingCommitments.FindAll(comm => comm.committer == sourceInventory && comm.itemType == itemType);
        }

        public Commitment CommitToAnInventory(IInventory targetInventory, ItemType itemType, int amount)
        {
            if (itemType != ItemType)
                throw new CommitmentException("Cannot commit because this item object is not of this type");
            if (GetCurrentUncommittedAmount(ItemType) < amount)
                throw new CommitmentException("Cannot commit because item object does not contain enough of this type");
            if (!targetInventory.CanAcceptAmount(ItemType, amount))
                throw new CommitmentException("Cannot commit because target does not have enough space");

            Commitment newCommitment = new Commitment(amount, ItemType, targetInventory, this);
            newCommitment.CommitToComitterAndComitee();
            var merged = Commitment.MergeCommitmentsOfThisType(ItemType, this, targetInventory);
            return merged;
        }

        public void AddToCurrentAmount(ItemType itemType, int amount)
        {
            if (!CanAcceptAmount(itemType, amount))
                throw new InventoryException("Cannot add more of this item to item object -- it is full");
            amountOfItem += amount;
        }

        public void RemoveFromCurrentAmount(ItemType itemType, int amount)
        {
            if (GetCurrentUncommittedAmount(itemType) < amount)
                throw new InventoryException("Cannot remove this amount -- not enough uncommitted");
            amountOfItem -= amount;
        }
    }

    public enum ItemType
    {
        Wood,IronOre
    }
}