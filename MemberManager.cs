using System;
using System.Collections.Generic;
using GymProjectApp.Models;
using GymProjectApp.DAL;

namespace GymProjectApp.BLL
{
    /// <summary>
    /// Business Logic Layer class responsible for managing Member operations.
    /// It delegates persistence tasks to MemberRepository and applies business rules if needed.
    /// </summary>
    public class MemberManager
    {
        // Repository instance for database operations
        private readonly MemberRepository _repo = new MemberRepository();

        /// <summary>
        /// Adds a new member to the database after basic validation.
        /// Returns true if successful, false if validation fails.
        /// </summary>
        public bool Add(Member member)
        {
            if (string.IsNullOrWhiteSpace(member.Name) || member.Age < 12)
                return false;

            _repo.Insert(member);
            return true;
        }

        /// <summary>
        /// Updates an existing member in the database.
        /// Returns true if successful, false if validation fails.
        /// </summary>
        public bool Update(Member member)
        {
            if (member.MemberID <= 0)
                return false;

            _repo.Update(member);
            return true;
        }

        /// <summary>
        /// Deletes a member by ID.
        /// Returns true if successful, false if ID is invalid.
        /// </summary>
        public bool Delete(int id)
        {
            if (id <= 0)
                return false;

            _repo.Delete(id);
            return true;
        }

        /// <summary>
        /// Retrieves all members from the database.
        /// </summary>
        public List<Member> GetAll()
        {
            return _repo.GetAll();
        }

        /// <summary>
        /// Searches for a member by name (case-insensitive).
        /// </summary>
        public Member? SearchByName(string name)
        {
            var members = _repo.GetAll();
            return members.Find(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}