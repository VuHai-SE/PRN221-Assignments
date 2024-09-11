// Pages/Members/Delete.cshtml.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesBOs;
using SalesRepositories;

namespace SalesRazorPageApp.Pages.Members
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IMemberRepository _memberRepository;

        public DeleteModel(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [BindProperty]
        public Member Member { get; set; }

        public IActionResult OnGet(int id)
        {
            Member = _memberRepository.GetMemberById(id);
            if (Member == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var memberToDelete = _memberRepository.GetMemberById(id);

            if (memberToDelete == null)
            {
                return NotFound();
            }

            _memberRepository.Delete(id);
            return RedirectToPage("./Index");
        }
    }
}
