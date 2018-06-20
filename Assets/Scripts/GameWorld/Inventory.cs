using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json.Bson;

namespace Assets.Scripts.GameWorld
{
    public class Inventory : IInventory
    {
        private Dictionary<ItemType, int> _currentAmount = new Dictionary<ItemType, int>();
        private List<Commitment> OutgoingCommitments = new List<Commitment>();
        private List<Commitment> IncomingCommitments = new List<Commitment>();
//        private int _maxSize = 1000;
//        private int _maxTypeCount = 5;

        //TODO -- enforce a MAX amount to place in inventory, currently is not blocked
        //
//        public List<Commitment> GetOutGoingCommitList()
//        {
//            return OutgoingCommitments;
//        }
//
//        public List<Commitment> GetIncomingCommitList()
//        {
//            return IncomingCommitments;
//        }

        public void AddCommitment(Commitment commitmentToAdd)
        {
            if (commitmentToAdd.committer == this && !OutgoingCommitments.Contains(commitmentToAdd))
                OutgoingCommitments.Add(commitmentToAdd);
            if (commitmentToAdd.committedTo == this && !IncomingCommitments.Contains(commitmentToAdd))
                IncomingCommitments.Add(commitmentToAdd);
        }

        public void RemoveCommitment(Commitment commitmentToRemove)
        {
            if (commitmentToRemove.committer == this && OutgoingCommitments.Contains(commitmentToRemove))
                OutgoingCommitments.Remove(commitmentToRemove);
            if (commitmentToRemove.committedTo == this && IncomingCommitments.Contains(commitmentToRemove))
                IncomingCommitments.Remove(commitmentToRemove);
        }

        public void RemoveCommitments(List<Commitment> commitmentsToRemove)
        {
            commitmentsToRemove.ForEach(RemoveCommitment);
        }

        public bool CanAcceptAmount(ItemType itemType, int amount)
        {
            //TODO -- enforce a MAX amount to place in inventory, currently is not blocked
            return true;
        }

        public bool MoveItemFromThisInventoryTo(IInventory targetInventory, ItemType itemType, int amount)
        {
            Commitment mergedCommitement = Commitment.MergeCommitmentsOfThisType(itemType, this, targetInventory);
            if (mergedCommitement == null || mergedCommitement.amount < amount)
            {
                throw new InventoryException(
                    "There is no commitment of this amount: " + amount + " of type " + itemType);
                //TODO -- maybe return false?
                //return false;
            }

            mergedCommitement.FullfillCommitmentAtomic(amount);
            return true;
        }

        public int GetCurrentAmount(ItemType itemType)
        {
            if (!_currentAmount.ContainsKey(itemType))
                return 0;
            return _currentAmount[itemType];
        }
        public int GetCurrentUncommittedAmount(ItemType itemType)
        {
            if (!_currentAmount.ContainsKey(itemType))
                return 0;
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
            if (!_currentAmount.ContainsKey(itemType))
                throw new CommitmentException("Cannot commit because inventory does not contain this type");
            if (GetCurrentUncommittedAmount(itemType) < amount)
                throw new CommitmentException("Cannot commit because inventory does not contain enough of this type");
            if (!targetInventory.CanAcceptAmount(itemType, amount))
                throw new CommitmentException("Cannot commit because target inventory does not have enough space");
        
            Commitment newCommitment = new Commitment(amount, itemType, targetInventory, this);
            newCommitment.CommitToComitterAndComitee();
            var merged = Commitment.MergeCommitmentsOfThisType(itemType, this, targetInventory);
            return merged;
        }

        public void AddToCurrentAmount(ItemType itemType, int amount)
        {
            if (!CanAcceptAmount(itemType, amount))
                throw new InventoryException("Cannot add more of this item to inventory -- it is full");
            _currentAmount[itemType] = GetCurrentAmount(itemType) + amount;
        }

        public void RemoveFromCurrentAmount(ItemType itemType, int amount)
        {
            if (GetCurrentUncommittedAmount(itemType) < amount)
                throw new InventoryException("Cannot remove this amount -- not enough uncommitted");
            _currentAmount[itemType] = GetCurrentAmount(itemType) - amount;
        }
    }

    public interface IInventory
    {
//        List<Commitment> GetOutGoingCommitList();
//        List<Commitment> GetIncomingCommitList();
        void AddCommitment(Commitment commitmentToAdd);
        void RemoveCommitment(Commitment commitmentToRemove);
        void RemoveCommitments(List<Commitment> commitmentsToRemove);
        bool CanAcceptAmount(ItemType itemType, int amount);
        bool MoveItemFromThisInventoryTo(IInventory targetInventory, ItemType itemType, int amount);
        int GetCurrentAmount(ItemType itemType);
        int GetCurrentUncommittedAmount(ItemType itemType);
        List<Commitment> GetAllOutgoingCommits();
        List<Commitment> GetCommitmentsToInventory(IInventory targetInventory);
        List<Commitment> GetCommitmentsToInventory(IInventory targetInventory, ItemType itemType);
        List<Commitment> GetAllIncomingCommits();
        List<Commitment> GetCommitmentsFromInventory(IInventory sourceInventory);
        List<Commitment> GetCommitmentsFromInventory(IInventory sourceInventory, ItemType itemType);
        Commitment CommitToAnInventory(IInventory targetInventory, ItemType itemType, int amount);
        void AddToCurrentAmount(ItemType itemType,int amount);
        void RemoveFromCurrentAmount(ItemType itemType, int amount);
    }

    public class Commitment
    {
        public readonly int amount;
        public readonly ItemType itemType;
        public readonly IInventory committedTo;
        public readonly IInventory committer;

        public Commitment(int amount, ItemType itemType, IInventory committedTo, IInventory committer)
        {
            this.amount = amount;
            this.itemType = itemType;
            this.committedTo = committedTo;
            this.committer = committer;
        }

        public void CommitToComitterAndComitee()
        {
            committer.AddCommitment(this);
            committedTo.AddCommitment(this);
        }

        public static Commitment MergeCommitmentsOfThisType(ItemType itemType, IInventory committer,
            IInventory committedTo)
        {
            List<Commitment> commitments = committer.GetAllOutgoingCommits().FindAll(commitment =>
                commitment.itemType == itemType &&
                commitment.committer == committer &&
                commitment.committedTo == committedTo);
            if (commitments.Count == 0)
            {
                return null;
            }

            var merged = new Commitment(commitments.Sum(a => a.amount), itemType, committedTo, committer);

            committer.RemoveCommitments(commitments);
            committedTo.RemoveCommitments(commitments);
            merged.CommitToComitterAndComitee();
            return merged;
        }


        public void FullfillCommitmentAtomic()
        {
            FullfillCommitmentAtomic(amount);
        }

        public void FullfillCommitmentAtomic(int amountToCommit)
        {
            if (amountToCommit > amount)
            {
                throw new InventoryException("Cannot move more than there is in commitment!" +
                                             "Currently committing: " + amountToCommit + " but only committed to: " + amount);
            }
            committer.RemoveCommitment(this);
            committedTo.RemoveCommitment(this);
            committer.RemoveFromCurrentAmount(itemType, amountToCommit);
            committedTo.AddToCurrentAmount(itemType, amountToCommit);
            
            if (amountToCommit < amount)
            {
                var newCommittment = new Commitment(amount - amountToCommit,itemType,committedTo,committer);
                newCommittment.CommitToComitterAndComitee();
            }
        }
    }

    public class InventoryException : Exception
    {
        public InventoryException(String e) : base(e)
        {
        }
    }

    public class CommitmentException : Exception
    {
        public CommitmentException(String e) : base(e)
        {
        }
    }
}