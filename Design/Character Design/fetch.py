from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from bs4 import BeautifulSoup
import requests
import time
import os

# Setup Selenium WebDriver
driver = webdriver.Chrome()  # Assumes chromedriver is in your PATH
driver.get('https://www.hw.com/about/Faculty-Staff-Directory')

# Wait for the button and click it
WebDriverWait(driver, 10).until(EC.presence_of_element_located((By.ID, 'btnAll')))
button = driver.find_element(By.ID, 'btnAll')
button.click()

# Wait for content to load
time.sleep(5)

# Get the page source after the content has loaded
html_content = driver.page_source
driver.quit()

# Parse the HTML content with BeautifulSoup
soup = BeautifulSoup(html_content, 'html.parser')

# Find the unordered list that contains faculty members
directory_list = soup.find('ul', class_='directory-list')

# Base URL for constructing absolute URLs
base_url = 'https://www.hw.com'

# Create a directory to save images
os.makedirs('hw_images', exist_ok=True)

# Iterate over each list item in the unordered list
for li in directory_list.find_all('li'):
    img = li.find('div', class_='image').find('img')
    person_name_div = li.find('div', class_='person-name')
    if img and person_name_div:
        img_url = img['src']
        if not img_url.startswith('http'):
            img_url = base_url + img_url
        img_response = requests.get(img_url)
        person_name = person_name_div.get_text(strip=True).replace(' ', '_')  # Formatting the filename
        with open(f'hw_images/{person_name}.jpg', 'wb') as file:  # Assuming the images are in JPEG format
            file.write(img_response.content)
