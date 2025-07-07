def response (status, data):
    """
    回應格式

    :param status: '1' 表示成功, '0' 表示失敗
    :param data: 包含數據的字典
    :return: 格式化的回應字典
    """
    return {
        "status": status,
        "content": data
    }