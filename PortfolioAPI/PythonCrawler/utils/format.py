import logging
import os

def setup_logger(log_filename="my_app.log", log_level=logging.INFO):
    """
    設置日誌系統
    
    :param log_filename: 日誌文件的名稱或路徑
    :param log_level: 日誌級別 (DEBUG, INFO, WARNING, ERROR, CRITICAL)
    :return: 配置好的 logger
    """
    
    # 確保日誌文件存在
    log_dir = os.path.dirname(log_filename)
    if log_dir and not os.path.exists(log_dir):
        os.makedirs(log_dir)

    """
    這個參數指定了日誌消息的輸出格式。
    format 參數是一個字符串，其中包含了日誌消息中各個部分的佔位符。下面是一些常見的佔位符及其含義：

    %(asctime)s: 日誌事件發生的時間。這個佔位符會被格式化為 datefmt 指定的日期時間格式。
    %(name)s: 日誌記錄器的名稱。通常是配置日誌的模組名稱或類名稱。
    %(levelname)s: 日誌的級別名稱（如 DEBUG、INFO、WARNING、ERROR、CRITICAL）。
    %(message)s: 日誌消息本身。

    datefmt
    這個參數指定了日期和時間的格式。
    它使用 strftime 函數的格式字符串，來控制日期和時間的顯示方式。常見的格式字符包括：

    %Y: 四位數年份（例如，2024）
    %m: 兩位數月份（01 到 12）
    %d: 兩位數日期（01 到 31）
    %H: 24 小時制的兩位數小時（00 到 23）
    %M: 兩位數分鐘（00 到 59）
    %S: 兩位數秒（00 到 59）
    """
    logging.basicConfig(
        filename=log_filename, 
        filemode='w', 
        level=log_level,
        format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',
        datefmt='%Y-%m-%d %H:%M:%S'
    )

    logger = logging.getLogger()

    return logger

