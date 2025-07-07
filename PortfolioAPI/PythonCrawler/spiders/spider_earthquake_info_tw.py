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
    rows = page.locator(".dataTables_scrollBody tbody tr")

    #取得前三筆
    count = await rows.count()

    for i in range(min(10, count)):
        row = rows.nth(i)
        tr_id = (await row.get_attribute("id")).split("tr_")[1]
        cols = row.locator("td")

        # 點選列
        await row.click()
        await page.wait_for_selector(".eqResultBoxRight", timeout=5000)

        ul = page.locator(".eqResultBoxRight")
        li_group = ul.locator("li")

        time_text = await li_group.nth(1).locator(".text").inner_text()
        position_text = await li_group.nth(2).locator(".text").inner_text()

        #longitude = position_text.split('北緯')[1].split('°')[0].strip()
        #latitude = position_text.split('東經')[1].split('°')[0].strip()
        #print(f"{time_text}|{position_text}|{longitude}|{latitude}")

        now = datetime.now().strftime('%Y%m')

        # Screenshot 和 PDF
        # screenshot_path = os.path.join(OUTPUT_DIR, f"{tr_id}.png")
        # pdf_path = os.path.join(OUTPUT_DIR, f"{tr_id}.pdf")

        # await page.screenshot(path=screenshot_path, full_page=True)
        # await page.pdf(path=pdf_path)

        obj = {
            "id": tr_id,
            "地震時間": time_text.strip(),
            "震央位置": position_text,
            "地震深度": await li_group.nth(3).locator('.text').inner_text(),
            "規模": await li_group.nth(4).locator('.text').inner_text(),
            "相對位置": await li_group.nth(5).locator('.text').inner_text(),
            "圖片": f'https://scweb.cwa.gov.tw/webdata/OLDEQ/{now}/{tr_id}.gif',
            # "經度":  longitude,
            # "緯度":  latitude,
            #"screenshot": screenshot_path,
            #"pdf": pdf_path
        }

        data.append(obj)

        # 回上一頁
        await page.go_back()

    return data

async def main():
    try:
        async with async_playwright() as p:
            browser = await p.chromium.launch(headless=True)
            context = await browser.new_context()
            page = await context.new_page()

            url = 'https://scweb.cwa.gov.tw/zh-tw/earthquake/data/'

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

