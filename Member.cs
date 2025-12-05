
namespace GymProjectApp.BLL
{
    public class Member
    {
        public int MemberID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string MembershipType { get; set; } // Ex: Mensal, Anual
        public DateTime JoinDate { get; set; }
    }
}
