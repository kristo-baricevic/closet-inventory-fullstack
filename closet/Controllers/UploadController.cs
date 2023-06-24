using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        public ActionResult Upload([FromForm] string category, [FromForm] IFormFile imageFile)
        {
            Console.WriteLine("Upload endpoint called.");
            
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    // Read the image file into a byte array
                    using (var memoryStream = new MemoryStream())
                    {
                        imageFile.CopyTo(memoryStream);
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
                catch (Exception ex)
                {
                    // Return an error message as JSON
                    return BadRequest(new { error = "Error loading or processing image: " + ex.Message });
                }
            }
            else
            {
                // Return an error message as JSON
                return BadRequest(new { error = "No image selected" });
            }
        }


private byte[] PreprocessImage(IFormFile imageFile)
{
    using (var image = Image.Load(imageFile.OpenReadStream()))
    {
        // Perform image processing logic here using the `image` object
        // You can use methods like Resize, Crop, Rotate, etc. from the `ImageSharp` library

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