using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using ClothingInventory.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace ClothingInventory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly ClothingInventoryContext _dbContext;

        public UploadController(ClothingInventoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var category = Request.Form["category"];
                var imageFile = Request.Form.Files["imageFile"];

                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream).ConfigureAwait(false);
                        var imageBytes = memoryStream.ToArray();

                        // Preprocess the image
                        var processedImage = PreprocessImage(imageFile);

                        // Classify the category
                        var predictedCategory = ClassifyCategory(category);

                        // Save the image and category to the database
                        SaveImageAndCategory(processedImage, predictedCategory);

                        // Return the predicted category as JSON
                        return Ok(new { category = predictedCategory });
                    }
                }
                else
                {
                    return BadRequest(new { error = "No image selected" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { error = "Error loading or processing image: " + ex.Message });
            }
        }

        private byte[] PreprocessImage(IFormFile imageFile)
        {
            using (var image = Image.Load(imageFile.OpenReadStream()))
            {
                // Perform image processing logic here using the `image` object
                // Example: Resize, Crop, Rotate, etc.

                using (var processedImage = new MemoryStream())
                {
                    image.Save(processedImage, new JpegEncoder()); // Save the processed image to the stream
                    return processedImage.ToArray(); // Convert the processed image to a byte array
                }
            }
        }

        private string ClassifyCategory(string category)
        {
            // Implement the classification logic based on the user's input category
            // Here's an example rule-based approach
            if (category.ToLower() == "top")
                return "Top";
            else if (category.ToLower() == "bottom")
                return "Bottom";
            else if (category.ToLower() == "shoes")
                return "Shoes";
            else if (category.ToLower() == "hat")
                return "Hat";
            else if (category.ToLower() == "accessory")
                return "Accessory";
            else
                return "Unknown";
        }

        private void SaveImageAndCategory(byte[] image, string category)
        {
            // Save the image and category to the database
            var clothingItem = new ClothingItem
            {
                Image = image,
                Category = category
            };

            _dbContext.ClothingItems.Add(clothingItem);
            _dbContext.SaveChanges();
        }
    }
}
