using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaWisLam.Data;
using SocialMediaWisLam.Models;
using System.Linq;
using System.Security.Claims;

namespace SocialMediaWisLam.Controllers
{
    [Authorize]
    public class SavedPostsController : Controller
    {
        private readonly SocialMediaWisLamContext _context;

        public SavedPostsController(SocialMediaWisLamContext context) {
            _context = context;
        }

        public class PostIQueryable {
            
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? pageIndex)
        {
            var pageSize = 3;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var savedPosts = from savedPost in _context.SavedPost
                             join post in _context.Post on savedPost.PostId equals post.Id
                             join videos in _context.Video on post.Id equals videos.PostOwner.Id
                             join photos in _context.Photo on post.Id equals photos.PostOwner.Id
                             select new 
                             {
                                 Videos = videos,
                                 Photos = photos,
                                 CreatedDate = post.CreatedDate,
                                 UpdatedDate = post.UpdatedDate,
                                 Description = post.Description,
                                 Id = post.Id
                             };

            /*
                var count = await source.CountAsync();
                var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                return new PaginatedList<T>(items, count, pageIndex, pageSize);
             */

            var item = await savedPosts.Skip((pageIndex ?? 1 - 1) * pageSize).Take(pageSize).ToListAsync();
            return View(item);
        }
    }
}
