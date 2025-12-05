using System.Collections.Generic;
using GymProjectApp.DAL;

namespace GymProjectApp.BLL
{
    public class MemberManager
    {
        private readonly MemberRepository _repo = new MemberRepository();

        public void Add(Member m) => _repo.Insert(m);
        public void Update(Member m) => _repo.Update(m);
        public void Delete(int id) => _repo.Delete(id);
        public List<Member> GetAll() => _repo.GetAll();

        // 🔹 Extra: Buscar por nome (Binary Search pode ser aplicado aqui)
        public Member SearchByName(string name)
        {
            var members = _repo.GetAll();
            return members.Find(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
