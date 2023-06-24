document.addEventListener('DOMContentLoaded', () => {
    const categoryDropdown = document.querySelector('#category');
    const imageFileInput = document.querySelector('#imageFile');
    const classifyButton = document.querySelector('#classifyButton');
    const resultLabel = document.querySelector('#resultLabel');
    const uploadedImage = document.querySelector('#uploadedImage');

    classifyButton.addEventListener('click', () => {
        const category = categoryDropdown.value;
        const imageFile = imageFileInput.files[0];
        
        if (imageFile) {
            const formData = new FormData();
            formData.append('category', category);
            formData.append('imageFile', imageFile);

            fetch('/api/Upload', {
                method: 'POST',
                body: formData
            })
            .then(response => {
                if (response.ok) {
                    resultLabel.textContent = 'Image uploaded successfully';

                    // Update the image source
                    uploadedImage.src = URL.createObjectURL(imageFile);
                    uploadedImage.style.display = 'block'; 
                } else {
                    resultLabel.textContent = 'Error uploading image';
                }
            })
            .catch(error => {
                console.error(error);
                resultLabel.textContent = `Error: ${error.message}`;
            });
        } else {
            resultLabel.textContent = 'No image selected';
        }
    });
});
