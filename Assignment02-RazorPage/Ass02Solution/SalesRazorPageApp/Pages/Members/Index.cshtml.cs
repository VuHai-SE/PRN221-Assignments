using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalesBOs;
using SalesDAOs;
using SalesRepositories;

namespace SalesRazorPageApp.Pages.Members
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IMemberRepository _memberRepository;

        public IndexModel(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public IList<Member> Members { get; set; } = new List<Member>();

        public void OnGet()
        {
            if (User.IsInRole("Admin"))
            {
                if (string.IsNullOrWhiteSpace(SearchTerm))
                {
                    Members = _memberRepository.GetAllMembers().ToList();
                }
                else
                {
                    Members = _memberRepository.SearchMembers(SearchTerm).ToList();
                }
            }
            else
            {
                var memberId = int.Parse(User.FindFirst("MemberId").Value);
                Members = new List<Member> { _memberRepository.GetMemberById(memberId) };
            }
        }
    }
}
