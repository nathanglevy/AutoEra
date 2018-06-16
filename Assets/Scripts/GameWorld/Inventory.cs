using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace Assets.Scripts.GameWorld
{
    public class Inventory
    {
        internal Dictionary<ItemType, int> _currentAmount = new Dictionary<ItemType, int>();
        internal List<Commitment> _outgoingAmount = new List<Commitment>();
        internal List<Commitment> _incomingAmount = new List<Commitment>();
        private int _maxSize = 1000;
        private int _maxTypeCount = 5;

        //TODO -- enforce a MAX amount to place in inventory, currently is not blocked
        //
        public bool CanAcceptAmount(ItemType itemType, int amount)
        {
            //TODO -- enforce a MAX amount to place in inventory, currently is not blocked
            return true;
        }

        public bool MoveItemFromThisInventoryTo(Inventory targetInventory, ItemType itemType, int amount)
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
            List<Commitment> outGoingCommitsOfType = _outgoingAmount.Where(it => it.itemType == itemType).ToList();
            int committedAmount = outGoingCommitsOfType.Sum(it => it.amount);
            return GetCurrentAmount(itemType) - committedAmount;
        }

        public List<Commitment> GetAllOutgoingCommits()
        {
            return _outgoingAmount.ToList();
        }

        public List<Commitment> GetCommitmentsToInventory(Inventory targetInventory)
        {
            return _outgoingAmount.FindAll(comm => comm.committedTo == targetInventory);
        }

        public List<Commitment> GetCommitmentsToInventory(Inventory targetInventory, ItemType itemType)
        {
            return _outgoingAmount.FindAll(comm => comm.committedTo == targetInventory && comm.itemType == itemType);
        }

        public List<Commitment> GetAllIncomingCommits()
        {
            return _incomingAmount.ToList();
        }

        public List<Commitment> GetCommitmentsFromInventory(Inventory sourceInventory)
        {
            return _incomingAmount.FindAll(comm => comm.committer == sourceInventory);
        }

        public List<Commitment> GetCommitmentsFromInventory(Inventory sourceInventory, ItemType itemType)
        {
            return _incomingAmount.FindAll(comm => comm.committer == sourceInventory && comm.itemType == itemType);
        }

        public Commitment CommitToAnInventory(Inventory targetInventory, ItemType itemType, int amount)
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
    }

    public class Commitment
    {
        public readonly int amount;
        public readonly ItemType itemType;
        public readonly Inventory committedTo;
        public readonly Inventory committer;

        public Commitment(int amount, ItemType itemType, Inventory committedTo, Inventory committer)
        {
            this.amount = amount;
            this.itemType = itemType;
            this.committedTo = committedTo;
            this.committer = committer;
        }

        public void CommitToComitterAndComitee()
        {
            committer._outgoingAmount.Add(this);
            committedTo._incomingAmount.Add(this);
        }

        public static Commitment MergeCommitmentsOfThisType(ItemType itemType, Inventory committer,
            Inventory committedTo)
        {
            List<Commitment> commitments = committer._outgoingAmount.FindAll(commitment =>
                commitment.itemType == itemType &&
                commitment.committer == committer &&
                commitment.committedTo == committedTo);
            if (commitments.Count == 0)
            {
                return null;
            }

            var merged = new Commitment(commitments.Sum(a => a.amount), itemType, committedTo, committer);
            committer._outgoingAmount.RemoveAll(commitments.Contains);
            committedTo._incomingAmount.RemoveAll(commitments.Contains);
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
            committer._outgoingAmount.Remove(this);
            committedTo._incomingAmount.Remove(this);
            committer._currentAmount[itemType] -= amountToCommit;
            committedTo._currentAmount[itemType] += amountToCommit;
            
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