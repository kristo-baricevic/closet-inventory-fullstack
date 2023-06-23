import tkinter as tk
from tkinter import filedialog
import requests

def classify_image(category_var):
    # Load and preprocess the image
    image_path = filedialog.askopenfilename(initialdir="/", title="Select Image",
                                            filetypes=(("Image files", "*.jpg *.jpeg *.png *.gif"), ("All files", "*.*")))
    if image_path:
        try:
            # Perform image classification based on the selected category
            category = category_var.get()
            response = send_image_and_category(image_path, category)

            # Handle the response
            if response.status_code == 200:
                predicted_category = response.json()["category"]
                result_label.config(text="Image classified as {}".format(predicted_category))
            else:
                result_label.config(text="Error: {}".format(response.text))
        except Exception as e:
            result_label.config(text="Error loading or processing image: {}".format(str(e)))
    else:
        result_label.config(text="No image selected")

def send_image_and_category(image_path, category):
    # Prepare the API endpoint URL
    url = "http://localhost:5000/upload"

    # Prepare the image data and category
    image = open(image_path, "rb")
    data = {"category": category}

    # Send the image and category to the backend API
    response = requests.post(url, files={"imageFile": image}, data=data)

    return response

# Create the main window
window = tk.Tk()

# Create a variable to store the selected category
category_var = tk.StringVar()

# Add a label and entry for category input
category_label = tk.Label(window, text="Category:")
category_label.pack()
category_dropdown = tk.OptionMenu(window, category_var, "Top", "Bottom", "Shoes", "Hat", "Accessory")
category_dropdown.pack()

# Add a button to select the image and perform classification
classify_button = tk.Button(window, text="Select Image", command=lambda: classify_image(category_var))
classify_button.pack()

# Add a label to display the classification result
result_label = tk.Label(window, text="")
result_label.pack()

# Start the main event loop
window.mainloop()
