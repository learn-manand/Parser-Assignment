import { test, expect } from '@playwright/test';

test('should parse input and display result', async ({ page }) => {
  await page.goto('http://localhost:4200');

  const inputText = `
    <expense>
      <cost_centre>DEV632</cost_centre>
      <total>1000</total>
    </expense>
  `;

  await page.fill('textarea', inputText);
  await page.click('button:text("Submit")');

  await expect(page.locator('pre')).toBeVisible();
  await expect(page.locator('pre')).toContainText('DEV632');
});