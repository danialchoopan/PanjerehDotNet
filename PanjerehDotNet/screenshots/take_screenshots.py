from playwright.sync_api import sync_playwright
import os
import time

def take_screenshots():
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        page = browser.new_page()

        screenshots_dir = "PanjerehDotNet/screenshots"
        if not os.path.exists(screenshots_dir):
            os.makedirs(screenshots_dir)

        # Home Page
        print("Taking home_page.png...")
        page.goto("http://localhost:5259")
        time.sleep(5)
        page.screenshot(path=os.path.join(screenshots_dir, "home_page.png"))

        # Ad Details
        print("Taking ad_details.png...")
        page.goto("http://localhost:5259/AdDetails/1")
        time.sleep(3)
        page.screenshot(path=os.path.join(screenshots_dir, "ad_details.png"))

        # Login and then Chat
        print("Logging in...")
        page.goto("http://localhost:5259/Login")
        page.fill('input[name="PhoneNumber"]', '09121111111')
        page.click('button[type="submit"]')
        time.sleep(5) # Wait for redirect

        print("Taking chat.png...")
        page.goto("http://localhost:5259/Chat?adId=1&otherUserId=3")
        time.sleep(3)
        page.screenshot(path=os.path.join(screenshots_dir, "chat.png"))

        # Admin Dashboard
        print("Taking admin_dashboard.png...")
        page.goto("http://localhost:5259/Admin")
        time.sleep(3)
        page.screenshot(path=os.path.join(screenshots_dir, "admin_dashboard.png"))

        browser.close()
        print("Screenshots taken successfully.")

if __name__ == "__main__":
    take_screenshots()
