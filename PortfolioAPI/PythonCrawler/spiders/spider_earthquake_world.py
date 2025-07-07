import os
import sys
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
import json
import asyncio

# from playwright.sync_api import sync_playwright
from playwright.async_api import async_playwright
from dotenv import load_dotenv
from datetime import datetime
# from utils.format import setup_logger
# from utils.crawler_setup import init_crawler
from utils.response_format import response

data = []

'以後改寫成 ELK'
# driver, logger = init_crawler(log_filename='PythonCrawler/logs/spider_earthquake_info_tw.log')

async def earthquake_parse_page(page, url):
    await page.goto(url, timeout=10000)
    await page.wait_for_selector('#table', timeout=3000)
    rows = page.locator("tbody tr")
    count = await rows.count()

    for i in range(0, count):
        row = rows.nth(i)

        # 確認可看見在執行
        if not await row.is_visible():
            continue  # 跳過不可見的列

        cols = row.locator("td")

        # print(cols)
        # 等待欄位渲染
        await cols.first.wait_for()

        obj = {
            "地震時間": (await cols.nth(0).inner_text()).strip(),
            "經度": (await cols.nth(1).inner_text()).strip(),
            "緯度": (await cols.nth(2).inner_text()).strip(),
            "深度_公里": (await cols.nth(3).inner_text()).strip(),
            "規模": (await cols.nth(4).inner_text()).strip(),
            "地震位置": (await cols.nth(5).inner_text()).strip(),
        }

        # print(obj)

        data.append(obj)

    return data

async def main():
    try:
        async with async_playwright() as p:
            browser = await p.chromium.launch(headless=True)
            context = await browser.new_context()
            page = await context.new_page()

            url = 'https://scweb.cwa.gov.tw/zh-tw/earthquake/world/'

            try:
                await earthquake_parse_page(page, url)
                res = response('1', data)
            except Exception as e:
                print(f"爬取錯誤: {e}")
                res = response('0', '撈取異常')

            print(json.dumps(res, ensure_ascii=False, indent=4))
            await browser.close()
    except Exception as e:
        print(f"Playwright 啟動失敗: {e}")

if __name__ == "__main__":
    asyncio.run(main())

