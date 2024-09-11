using SalesBOs;
using SalesDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
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
            member.Password = HashPassword(member.Password);
            _memberDAO.AddMember(member);
        }

        public void Update(Member member)
        {
            member.Password = HashPassword(member.Password);
            _memberDAO.UpdateMember(member);
        }

        public void Delete(int memberId)
        {
            _memberDAO.DeleteMember(memberId);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
