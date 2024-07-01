using SalesBOs;
using SalesDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepos
{
    public class MemberRepository : IMemberRepository
    {
        private readonly MemberDAO _memberDAO;

        public MemberRepository(MemberDAO memberDAO)
        {
            _memberDAO = memberDAO;
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _memberDAO.GetAllMembers();
        }

        public Member GetMemberByEmail(string email)
        {
            return _memberDAO.GetMemberByEmail(email);
        }

        public Member GetMemberById(int id)
        {
            return _memberDAO.GetMemberById(id);
        }

        public IEnumerable<Member> SearchMembers(string keyword)
        {
            return _memberDAO.SearchMembers(keyword);
        }

        public void Add(Member member)
        {
            _memberDAO.AddMember(member);
        }

        public void Update(Member member)
        {
            _memberDAO.UpdateMember(member);
        }

        public void Delete(int memberId)
        {
            _memberDAO.DeleteMember(memberId);
        }

    }
}
