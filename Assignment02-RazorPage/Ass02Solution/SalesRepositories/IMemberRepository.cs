using SalesBOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetAllMembers();
        Member GetMemberByEmail(string email);
        Member GetMemberById(int id);
        IEnumerable<Member> SearchMembers(string keyword);
        void Add(Member member);
        void Update(Member member);
        void Delete(int memberId);
    }
}
