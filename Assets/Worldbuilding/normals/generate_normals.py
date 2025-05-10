from PIL import Image
import numpy as np
import cv2

# Load the image
input_path = "idle1.png"  # Change this if your file has a different name
image = Image.open(input_path).convert('L')  # convert to grayscale

# Convert to numpy array
img_array = np.array(image)

# Calculate gradients using Sobel operator
grad_x = cv2.Sobel(img_array, cv2.CV_32F, 1, 0, ksize=1)
grad_y = cv2.Sobel(img_array, cv2.CV_32F, 0, 1, ksize=1)

# Normalize gradients to [-1, 1]
grad_x = grad_x / 255.0
grad_y = grad_y / 255.0

# Compute normal map components
normal_x = grad_x
normal_y = grad_y
normal_z = np.ones_like(grad_x)

# Normalize the normal vectors
length = np.sqrt(normal_x ** 2 + normal_y ** 2 + normal_z ** 2)
normal_x /= length
normal_y /= length
normal_z /= length

# Map from [-1, 1] to [0, 255]
normal_map = np.zeros((img_array.shape[0], img_array.shape[1], 3), dtype=np.uint8)
normal_map[..., 0] = ((normal_x + 1.0) * 0.5 * 255).astype(np.uint8)  # Red channel
normal_map[..., 1] = ((normal_y + 1.0) * 0.5 * 255).astype(np.uint8)  # Green channel
normal_map[..., 2] = (normal_z * 255).astype(np.uint8)                # Blue channel

# Save normal map
output_path = "backWall_normal.png"
Image.fromarray(normal_map).save(output_path)

print(f"Normal map saved as {output_path}")
