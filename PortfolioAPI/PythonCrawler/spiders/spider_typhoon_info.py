import os
import sys
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))
import asyncio
import json
from dotenv import load_dotenv
from playwright.async_api import async_playwright
from utils.response_format import response

load_dotenv()

data = {
    "更新時間": "",
    "颱風名稱": "",
    "資訊": []
}

dgpa_url = 'https://www.dgpa.gov.tw/typh/daily/nds.html'
cwa_url = 'https://www.cwa.gov.tw/V8/C/P/Typhoon/TY_WARN.html'


async def dgpa_parse_page(page):
    await page.goto(dgpa_url, timeout=10000)
    await page.wait_for_selector('tbody.Table_Body tr')

    try:
        # 更新時間
        updated_text = await page.locator('.Content_Updata h4').inner_text()
        data["更新時間"] = updated_text.split('更新時間：')[1].split('\n')[0]

        # 資料列
        rows = page.locator('tbody.Table_Body tr')
        count = await rows.count()

        for i in range(count):
            row = rows.nth(i)
            try:
                city_name_el = row.locator('td[headers="city_Name"] font')
                if await city_name_el.count() > 0:
                    #print("找不到 city_Name")
                    #return  # 或 continue 視你用在哪個函式中
                    city_name = await row.locator('td[headers="city_Name"] font').inner_text()
                    info_els = row.locator('td[headers="StopWorkSchool_Info"] font')
                    if info_els and city_name:
                        info_count = await info_els.count()
                        stop_work_school_info = [await info_els.nth(j).inner_text() for j in range(info_count)]

                        if city_name and stop_work_school_info:
                            data["資訊"].append({
                                "地區": city_name,
                                "資訊": stop_work_school_info
                            })
            except Exception as e:
                print(f"某列解析失敗: {e}")
    except Exception as e:
        print(f"DGPA 資料解析失敗: {e}")


async def cwa_parse_page(page):
    await page.goto(cwa_url, timeout=10000)
    try:
        warn_content = await page.locator('.WarnContent').inner_text()
        data['颱風名稱'] = warn_content
    except Exception as e:
        print(f"CWA 資料解析失敗: {e}")


async def main():
    try:
        async with async_playwright() as p:
            browser = await p.chromium.launch(headless=True)
            context = await browser.new_context()
            page = await context.new_page()

            await dgpa_parse_page(page)
            # await cwa_parse_page(page)

            res = response('1', data)
            print(json.dumps(res, ensure_ascii=False, indent=4))

            await browser.close()
    except Exception as e:
        print(f"Playwright 執行失敗: {e}")
        res = response('0', '撈取異常')
        print(json.dumps(res, ensure_ascii=False, indent=4))


if __name__ == "__main__":
    asyncio.run(main())
