// Pages/Members/Create.cshtml.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesBOs;
using SalesRepositories;

namespace SalesRazorPageApp.Pages.Members
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IMemberRepository _memberRepository;

        public CreateModel(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [BindProperty]
        public Member Member { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _memberRepository.Add(Member);
            return RedirectToPage("./Index");
        }
    }
}
