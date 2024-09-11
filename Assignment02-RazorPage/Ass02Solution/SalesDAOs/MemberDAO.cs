using SalesBOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesDAOs
{
    public class MemberDAO
    {
        private readonly PRN_Assignment02Context _context;

        public MemberDAO(PRN_Assignment02Context context)
        {
            _context = context;
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members.ToList();
        }

        public Member GetMemberByEmail(string email)
        {
            return _context.Members.SingleOrDefault(m => m.Email == email);
        }

        public Member GetMemberById(int id)
        {
            return _context.Members.SingleOrDefault(m => m.MemberId == id);
        }


        public IEnumerable<Member> SearchMembers(string keyword)
        {
            int.TryParse(keyword, out int id);
            return _context.Members
                .Where(m => m.MemberId == id ||
                            m.Email.Contains(keyword) ||
                            m.CompanyName.Contains(keyword) ||
                            m.City.Contains(keyword) ||
                            m.Country.Contains(keyword))
                .ToList();
        }

        public void AddMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }

        public void UpdateMember(Member member)
        {
            var existingMember = _context.Members.Find(member.MemberId);
            if (existingMember != null)
            {
                _context.Entry(existingMember).CurrentValues.SetValues(member);
                _context.SaveChanges();
            }
            else
            {
                // Handle the case where the member does not exist
                throw new Exception("Member not found.");
            }
        }

        public void DeleteMember(int memberId)
        {
            var member = _context.Members.Find(memberId);
            if (member != null)
            {
                _context.Members.Remove(member);
                _context.SaveChanges();
            }
        }
    }
}
