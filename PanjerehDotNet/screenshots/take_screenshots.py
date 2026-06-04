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
        page.goto("http://localhost:5000")
        time.sleep(2)
        page.screenshot(path=os.path.join(screenshots_dir, "home_page.png"))

        # Ad Details
        print("Taking ad_details.png...")
        page.goto("http://localhost:5000/AdDetails/1")
        time.sleep(2)
        page.screenshot(path=os.path.join(screenshots_dir, "ad_details.png"))

        # Login and Chat Room
        print("Logging in and taking chat_room.png...")
        page.goto("http://localhost:5000/Login?returnUrl=/Chat")
        time.sleep(3)
        page.screenshot(path=os.path.join(screenshots_dir, "chat_room.png"))

        # Admin Dashboard
        print("Taking admin_dashboard.png...")
        page.goto("http://localhost:5000/Admin")
        time.sleep(2)
        page.screenshot(path=os.path.join(screenshots_dir, "admin_dashboard.png"))

        browser.close()
        print("Screenshots taken successfully.")

if __name__ == "__main__":
    take_screenshots()
