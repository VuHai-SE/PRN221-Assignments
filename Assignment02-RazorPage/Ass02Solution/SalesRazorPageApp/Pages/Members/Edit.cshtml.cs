using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesBOs;
using SalesRepositories;
using System.ComponentModel.DataAnnotations;

namespace SalesRazorPageApp.Pages.Members
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IMemberRepository _memberRepository;

        public EditModel(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [BindProperty]
        public Member Member { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        public IActionResult OnGet(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                var memberId = int.Parse(User.FindFirst("MemberId").Value);
                if (memberId != id)
                {
                    return Forbid();
                }
            }

            Member = _memberRepository.GetMemberById(id);
            if (Member == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var memberToUpdate = _memberRepository.GetMemberById(Member.MemberId);

            if (memberToUpdate == null)
            {
                return NotFound();
            }

            memberToUpdate.Email = Member.Email;
            memberToUpdate.CompanyName = Member.CompanyName;
            memberToUpdate.City = Member.City;
            memberToUpdate.Country = Member.Country;

            // Chỉ cập nhật mật khẩu nếu người dùng đã nhập mật khẩu mới
            if (!string.IsNullOrEmpty(NewPassword))
            {
                memberToUpdate.Password = NewPassword; // Cập nhật mật khẩu mới, nếu cần băm mật khẩu hãy dùng hàm băm ở đây
            }

            _memberRepository.Update(memberToUpdate);
            return RedirectToPage("./Index");
        }
    }
}
