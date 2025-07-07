from playwright.sync_api import sync_playwright

from utils.logger_format import setup_logger
import warnings

def init_crawler(url, log_filename='logs/spider.log'):
     with sync_playwright() as p:
          browser = p.chromium.launch(headless=True)
          page = browser.new_page()
          page.goto("https://example.com")
          print(page.title())
