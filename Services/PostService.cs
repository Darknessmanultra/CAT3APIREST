using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CAT3B.Models;
using CAT3B.Data;

namespace CAT3B.Services
{
    public class PostService
    {
        private readonly ApplicationDbContext _context;
        private readonly Cloudinary _cloudinary;

        public PostService(ApplicationDbContext context, Cloudinary cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        // Create a new post
        public async Task<Post> CreatePostAsync(string userId, string title, IFormFile image)
        {
            // Validate title
            if (string.IsNullOrWhiteSpace(title) || title.Length < 5)
                throw new ArgumentException("Title must be at least 5 characters long.");

            // Validate image
            if (image == null)
                throw new ArgumentException("Image is required.");
            if (image.Length > 5 * 1024 * 1024)
                throw new ArgumentException("Image size must not exceed 5MB.");
            if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(System.IO.Path.GetExtension(image.FileName).ToLower()))
                throw new ArgumentException("Only JPG and PNG formats are supported.");

            // Upload image to Cloudinary
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, image.OpenReadStream()),
                Folder = "posts"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
                throw new Exception($"Cloudinary upload failed: {uploadResult.Error.Message}");

            // Create post
            var post = new Post
            {
                Title = title,
                ImageUrl = uploadResult.SecureUrl.ToString(),
                PublishedAt = DateTime.UtcNow,
                UserId = userId
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return post;
        }

        // Get all posts for authenticated user
        public async Task<List<Post>> GetPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.PublishedAt)
                .ToListAsync();
        }
    }
}