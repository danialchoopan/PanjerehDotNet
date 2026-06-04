const { chromium } = require('playwright');
const fs = require('fs');
const path = require('path');

(async () => {
  const browser = await chromium.launch();
  const page = await browser.newPage();
  const screenshotsDir = path.join(__dirname);
  if (!fs.existsSync(screenshotsDir)) {
    fs.mkdirSync(screenshotsDir, { recursive: true });
  }

  // Home Page
  await page.goto('http://localhost:5000');
  await page.waitForTimeout(2000);
  await page.screenshot({ path: path.join(screenshotsDir, 'home_page.png') });

  // Ad Details
  await page.goto('http://localhost:5000/AdDetails?id=1');
  await page.waitForTimeout(2000);
  await page.screenshot({ path: path.join(screenshotsDir, 'ad_details.png') });

  // Chat Room
  await page.goto('http://localhost:5000/Chat?adId=1');
  await page.waitForTimeout(2000);
  await page.screenshot({ path: path.join(screenshotsDir, 'chat_room.png') });

  // Admin Dashboard
  await page.goto('http://localhost:5000/Admin');
  await page.waitForTimeout(2000);
  await page.screenshot({ path: path.join(screenshotsDir, 'admin_dashboard.png') });

  await browser.close();
  console.log('Screenshots taken successfully.');
})();
